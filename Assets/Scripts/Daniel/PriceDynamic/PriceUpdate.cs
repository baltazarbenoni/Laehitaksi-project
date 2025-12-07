using UnityEngine;
using Navigation;
using Taxes;
using PriceData;
using System.Collections;
using System;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class PriceUpdate : MonoBehaviour
{
    [SerializeField] GameObject priceStatusObject;
    PriceStatusManager priceStatus;
    Tax tax;
    internal Tax CurrentTax { get { return tax; } }
    Sum sum;
    internal Sum MainSum
    {
        get
        {
            Debug.Log("Returning sum: " + sum);
            return sum;
        }
    }
    [SerializeField] internal float SumAmount { get { return sum.Amount; } }

    //Store previous sum in case there was a mistake in entering max price.
    Sum previousSum;
    Timer timer;
    internal Timer MainTimer { get { Debug.Log("timer " + timer.Limit);  return timer; } }
    float speed;
    internal float Speed { get { Debug.Log("speed " + speed); return speed; } }
    bool drive;
    internal bool Drive { get { Debug.Log("drive " + drive);  return drive; } }
    bool holdOn;
    internal bool HoldOn { get { return holdOn; } }
    [Header("Interval between price updates.")]
    [SerializeField] float timerLimit = 0.5f;
    State status = State.Free;
    TaxManager taxManager;
    void Awake()
    {
        InitializeEvents();
        priceStatus = References.GetRef(gameObject, priceStatusObject, priceStatus);
        InitializeVariables();
    }
    void Start()
    {
        taxManager = priceStatus.taxManager;
    }
    void InitializeVariables()
    {
        sum = new Sum();
        Debug.Log($"New sum amount is: {sum.Amount}");
        sum.InitialPriceAdded = false;
        timer = new Timer(timerLimit);
        tax = null;
        status = State.Free;
    }
    void OnDestroy()
    {
        Unsubscribe();
    }
    void InitializeEvents()
    {
        Actions.SpeedChange += UpdateSpeed;
        Actions.NewTax += ChangeTax;
        Actions.BackToBase += ChangeTax;
        Actions.NavigationButton += ToggleHold;
        Actions.ChangeNavigationState += ChangeStatus;
        Actions.AddExtraFeeOrPayment += AddExtraOrPayment;
        Actions.ComissionCompleted += InitializeVariables;
        Actions.NewPriceStatus += UpdatePriceStatus;
        Actions.ForcePrice += ForcePrice;
    }
    void Unsubscribe()
    {
        Actions.SpeedChange -= UpdateSpeed;
        Actions.NewTax -= ChangeTax;
        Actions.BackToBase -= ChangeTax;
        Actions.NavigationButton -= ToggleHold;
        Actions.ChangeNavigationState -= ChangeStatus;
        Actions.AddExtraFeeOrPayment -= AddExtraOrPayment;
        Actions.ComissionCompleted -= InitializeVariables;
        Actions.NewPriceStatus -= UpdatePriceStatus;
    }
    void Update()
    {
        if(status == State.Comission)
        {
            RunOnComissionMode();
        }
    }
    void AddExtraOrPayment(float price)
    {
        sum.Amount += price;
        Debug.Log($"Changed sum with value {price}, new sum is {sum.Amount}");
    }
    void ForcePrice(float price)
    {
        sum.Amount = price;
    }
    void ChangeTax(Tax newTax)
    {
        taxManager.Change(newTax, tax);
        tax = newTax;
        if(tax.isFixed)
        {
            Debug.Log("using fixed price! " + tax.FixedPrice);
            sum.Amount = tax.FixedPrice;
            StartCoroutine(UpdateMeter());
        }
        else if(!sum.InitialPriceAdded)
        {
            Debug.Log("Adding initial tax");
            sum.Amount += tax.InitialTax;
            sum.InitialPriceAdded = true;
            priceStatus.AddInitial(tax.InitialTax);
            StartCoroutine(UpdateMeter());
        }
        CheckWaitTax();
    }
    public void ChangeStatus(State state)
    {
        status = state;
    }
    void ToggleHold(ButtonType type)
    {
        if(type == ButtonType.Hold)
        {
            holdOn = !holdOn;
        }
    }
    void RunOnComissionMode()
    {
        if(tax == null)
        {
            Debug.LogWarning("Tax has not been set!");
            return;
        }
        if(!holdOn && !tax.isFixed)
        {
            UpdatePrice();
            MonitorMaxPrice();
            MonitorMaxWait();
        }
    }
    void UpdatePrice()
    {
        timer.Time += Time.deltaTime;
        //If last check less than the limit interval, return.
        if (timer.Time < timer.Limit)
        {
            return;
        }
        else
        {
            GetCosts();
            Actions.UpdatePrice?.Invoke(sum.Amount);
            timer.Time = 0;
        }
    }
    void GetCosts()
    {
        //Km cost.
        if(drive)
        {
            KmCost kmCost = new KmCost(this, priceStatus);
            Debug.Log("km cost " + kmCost.Price);
            sum.Amount += kmCost.Price;
        }
        if(tax.WaitTimer != null && !tax.WaitTimer.WaitHasEnded)
        {
            sum.Amount += new WaitCost(this, priceStatus).Price;
            //Don't get the time cost if waiting.
            return;
        }
        //Time cost.
        TimeCost timeCost = new TimeCost(this, priceStatus);
        Debug.Log("Time cost " + timeCost.Price);
        sum.Amount += timeCost.Price;
    }
    void CheckWaitTax()
    {
        if(tax.isWait)
        {
            return;
        }
        else if(tax.taxMode == Mode.Kela)
        {
            UpdateWaitFromPrevious();
        }
   }
    void UpdateWaitFromPrevious()
    {
        Tax previous = taxManager.Previous;
        if(previous != null && previous.WaitTax != 0)
        {
            tax.AddWaitTax(previous);
        }
    }
    void UpdateSpeed(float speed)
    {
        this.speed = speed;
        if (speed == 0 && drive)
        {
            drive = false;
        }
        else if (speed != 0 && !drive)
        {
            drive = true;
        }
    }
    internal void RestartTimer()
    {
        timer.Time = 0;
    }
    void MonitorMaxPrice()
    {
        //If max price has not been set, return.
        if (tax.MaxPrice == 0)
        {
            return;
        }
        else if (sum.Amount >= tax.MaxPrice)
        {
            Debug.Log("Sum was larger than max price, moving to CheckOutGeneral.");
            //Save the sum in case of mistaken max price.
            previousSum = sum;
            sum.Amount = tax.MaxPrice;
            Actions.MoveToThis(ButtonType.Kassa);
        }
    }
    void MonitorMaxWait()
    {
        if(tax.WaitTimer == null)
        {
            return;
        }
        if(tax.WaitTimer.WaitHasEnded)
        {
            Debug.LogWarning("Moving to checkout!");
            previousSum = sum;
            tax.NullTimer();
            Actions.MoveToThis(ButtonType.Kassa);
        }
    }
    IEnumerator UpdateMeter()
    {
        yield return null;
        yield return null;
        Actions.PriceUpdateForceSum(sum.Amount);
    }
    void UpdatePriceStatus(PriceStatus newStatus)
    {
        taxManager = newStatus.taxManager;
    }
}
