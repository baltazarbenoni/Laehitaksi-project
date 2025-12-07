using UnityEngine;
using Taxes;
using Navigation;
using Payments;
using System;
using System.Collections;
using System.Collections.Generic;
using Prices;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class PriceStatusManager : MonoBehaviour
{
    //Fields to show data in the inspector.
    [SerializeField] State generalState;
    public State GeneralState => generalState;
    [SerializeField] float time;
    [SerializeField] float waitTime;
    [SerializeField] float distance;
    [SerializeField] float kmCost;
    [SerializeField] float timeCost;
    [SerializeField] float waitCost;
    [SerializeField] float slowDriveCost;
    [SerializeField] float noFeesPrice;
    [SerializeField] float basicPrice;
    [SerializeField] float initialFee;
    [SerializeField] bool initialAdded;
    [SerializeField] float fixedPrice;
    [SerializeField] bool isFixed;
    [SerializeField] string taxName;
    [SerializeField] bool baseTaxSet;
    [SerializeField] string baseTaxName;
    [SerializeField] float extraFees;
    [SerializeField] float finalPrice;
    [SerializeField] float currentPrice;
    [SerializeField] float maxWaitPrice;
    [SerializeField] float maxWaitTime;
    [SerializeField] float maxPrice;
    [SerializeField] float minPrice;
    [SerializeField] float paidAmount;
    [SerializeField] string payments;
    [SerializeField] PriceType basePrice;
    PriceStatus status;
    public PriceStatus currentStatus { get { return status; } }
    public float BasicPrice { get { return status.GetBasicPrice(); } }
    public float FinalPrice { get { return status.GetFinalPrice(); } }
    public float Price { get { return status.GetPrice(); } }
    public Tax CurrentTax { get { return status.TaxInUse; } }
    public TaxManager taxManager { get { return status.taxManager; } }
    List<Customer> customers = new();
    List<PriceStatus> statuses = new();
    bool completing;
    Payment paymentToComplete;
    void Awake()
    {
        status = new PriceStatus();
        statuses.Add(status);
    }
    void OnEnable()
    {
        InitializeEvents();
    }
    void OnDestroy()
    {
        Actions.AddFee -= UpdateExtra;
        Actions.RemoveExtras -= UpdateExtra;
        Actions.NewTax -= UpdateTax;
        Actions.BackToBase -= BackToBaseTax;
        Actions.ChangeNavigationState -= UpdateNavigationState;
    }
    void InitializeEvents()
    {
        Actions.AddFee += UpdateExtra;
        Actions.RemoveExtras += UpdateExtra;
        Actions.NewTax += UpdateTax;
        Actions.BackToBase += BackToBaseTax;
        Actions.ChangeNavigationState += UpdateNavigationState;
    }
    #region Methods
    public void UpdateKm(KmCost kmCost)
    {
        status.kmVar.UpdateAll(kmCost.Price, kmCost.Distance);
        status.UpdateCosts(kmCost.Price);
        UpdateDebugField();
    }
    public void UpdateTime(Variable.Type type, float price, float amount)
    {
        Variable obj = status.GetVariable(type);
        if(obj == null)
        {
            Debug.LogWarning("Object to update is null");
            return;
        }
        Debug.Log("Updating variable!");
        obj.UpdateAll(price, amount);
        status.UpdateCosts(price);
        UpdateDebugField();
    }
    void UpdateExtra(ExtraFee fee)
    {
        Actions.AddExtraFeeOrPayment(fee.Price);
        status.extraFees.Add(fee);
        Actions.ForcePrice(status.GetPrice());
        UpdateDebugField();
    }
    void UpdateExtra()
    {
        Actions.AddExtraFeeOrPayment(-status.GetExtraFeePrice());
        status.extraFees.Clear();
        Actions.ForcePrice(status.GetPrice());
        UpdateDebugField();
    }
    void UpdateTax(Tax newTax)
    {
        status.ChangeTax(newTax);
        UpdateDebugField();
    }
    void BackToBaseTax(Tax newTax)
    {
        status.ChangeToBaseTax(newTax);
        UpdateDebugField();
    }
    public void UpdateCustomers(List<Customer> customerList)
    {
        customers = customerList;
    }
    void CompleteComission()
    {
        if(!completing)
        {
            completing = true;
            StartCoroutine(Complete());
        }
    }
    IEnumerator Complete()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Restarting variables!");
        Actions.ComissionCompleted?.Invoke();
        status = new PriceStatus();
        Actions.NewPriceStatus?.Invoke(status);
        statuses.Add(status);
        paymentToComplete = null;
        UpdateDebugField();
        completing = false;
    }
    public void AddPayment(Payment payment)
    {
        if(paymentToComplete == null)
        {
            paymentToComplete = payment;
        }
        else
        {
            Debug.LogWarning($"There already exists a payment to complete! {paymentToComplete.PayType}: {paymentToComplete.PaySum}");
            paymentToComplete = payment;
        }
    }
    //WHEN PAYMENT SELECTED, CREATE A PAYMENT AND ASSIGN IT TO PAYMENT TO COMPLETE --> CREATE AN EVENT WHEN PAYMENT COMPLETED --> HANDLE THE PAYMENT TO COMPLETE
    public void HandlePayment()
    {
        Debug.LogWarning("Entered payment handling!");
        if(paymentToComplete == null)
        {
            Debug.LogWarning("No payment to handle!");
            return;
        }
        else if(paymentToComplete.Remainder >= 5f)
        {
            UpdatePayments(paymentToComplete);
        }
        else if(paymentToComplete.ByPassReceipt)
        {
            CompleteComission();
        }
        else
        {
            //Complete comission, restart all values and set receipt.
            CreateReceipt();
            CompleteComission();
        }
        paymentToComplete = null;
        Actions.ForcePrice(status.GetPrice());
        UpdateDebugField();
    }
    public void UpdatePayments(Payment payment)
    {
        //Add new sum but continue comission with previous values;
        Debug.Log("Paying partial or divided sum!");
        status.PayAmount(payment);
        status.UpdatePrice();
        CreatePartialPayReceipt();
        Actions.AddExtraFeeOrPayment(-payment.PaySum);
        paymentToComplete = null;
    }
    public void UpdatePrice(float price)
    {
        Debug.LogWarning($"Updating price {price} in statusmanager");
        status.AssignSum(price);
    }
    public void AddInitial(float price)
    {
        status.Price.SetInitial(price);
    }
    public void UpdateNavigationState(State status)
    {
        generalState = status;
    }
    public bool IsPaymentValid(float amount)
    {
        float verifiedAmount = status.PaymentCheck(amount);
        bool valid = verifiedAmount == amount;
        return valid;
    }
    #endregion
    #region Debug
    void UpdateDebugField()
    {
        status.UpdatePrice();
        taxName = status.TaxInUse.Name; 
        kmCost = status.kmVar.Price;
        timeCost = status.timeVar.Price;
        time = status.FullTime;
        distance = status.kmVar.Amount;
        waitCost = status.waitVar.Price;
        waitTime = status.waitVar.Amount;
        slowDriveCost = status.slowDriveVar.Price;
        initialFee = status.Price.Basis.Constant;
        fixedPrice = status.TaxInUse.FixedPrice;
        isFixed = status.TaxInUse.isFixed;
        extraFees = status.GetExtraFeePrice();
        noFeesPrice = status.NoFeesPrice;
        currentPrice = status.GetPrice();
        basicPrice = status.GetBasicPrice();
        finalPrice = status.GetFinalPrice();
        maxPrice = status.taxManager.GetBase().MaxPrice;
        minPrice = status.taxManager.GetBase().MinPrice;
        maxWaitPrice = status.TaxInUse.MaxWaitPrice;
        maxWaitTime = status.TaxInUse.MaxWaitTime;
        baseTaxName = status.taxManager.GetBase().Name;
        baseTaxSet = status.taxManager.GetBaseStatus();
        paidAmount = status.PaidAmount;
        payments = status.GetPaymentNames();
        basePrice = status.Price.CurrentBase.Name;
    }
    #endregion
    void CreateReceipt()
    {
        string payMethod = EnumNames.PaymentName(paymentToComplete.PayType); 
        string msg = $"Maksukuitti: {payMethod}\n{DateTime.Now}\nMatka: {Conversion.FloatToString(distance)}\nAika: {time}\nHinta: {Conversion.FloatToString(basicPrice)}\nMatkakulut: {Conversion.FloatToString(kmCost)}\nAikaveloitus: {Conversion.FloatToString(timeCost)}\nOdotusaika: {Conversion.FloatToString(waitTime)}\nOdotushinta: {Conversion.FloatToString(waitCost)}\nLisämaksut:\n";
        msg += status.GetExtraFeeNames();
        msg += $"Yhteensä: {extraFees}\n";
        //Debug.Log($"Receipt:\n{msg}");
        Actions.CreateReceipt?.Invoke(msg);
    }
    void CreatePartialPayReceipt()
    {
        string payMethod = EnumNames.PaymentName(paymentToComplete.PayType); 
        string msg = $"Maksukuitti: {payMethod}\n{DateTime.Now}\nMatka: {Conversion.FloatToString(distance)}m\nAika: {Conversion.GetFormattedTime(time)}\nOsahinta: {Conversion.FloatToString(paymentToComplete.PaySum)}€\nMatkakulut: {Conversion.FloatToString(kmCost)}€\nAikaveloitus: {Conversion.FloatToString(timeCost)}€\nOdotusaika: {Conversion.GetFormattedTime(waitTime)}\nOdotushinta: {Conversion.FloatToString(waitCost)}€\nLisämaksut:\n";
        msg += status.GetExtraFeeNames();
        msg += $"Yhteensä: {extraFees}\n€";
        //Debug.Log($"Receipt:\n{msg}");
        Actions.CreateReceipt?.Invoke(msg);
    }
}
