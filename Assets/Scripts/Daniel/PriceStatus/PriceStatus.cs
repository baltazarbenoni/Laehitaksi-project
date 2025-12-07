using Taxes;
using Payments;
using System.Collections.Generic;
using UnityEngine;
using Prices;

//C 2025 Daniel Snapir alias Baltazar Benoni
#region Status
public class PriceStatus
{
    public PriceStatus()
    {
        tax = new Tax();
        waitVar = new WaitVariable(Variable.Type.Wait, this);
        taxManager = new();
        basePriceManager = new(this);
    }
    Tax tax;
    public Tax TaxInUse { get { return tax; } }
    public Tax previousTax { get { return GetPrevious(); } }
    public TaxManager taxManager;
    internal List<Tax> taxes = new();
    internal Variable kmVar = new Variable(Variable.Type.Km);
    internal Variable waitVar;
    internal Variable timeVar = new Variable(Variable.Type.Time);
    internal Variable slowDriveVar = new Variable(Variable.Type.Slow);
    public float Distance { get { return kmVar.Amount; } }
    public float FullTime { get { return timeVar.Amount + waitVar.Amount + slowDriveVar.Amount; } }
    internal List<ExtraFee> extraFees = new();
    BasePriceManager basePriceManager;
    internal Price Price = new Price();
    float costs;
    internal float NoFeesPrice { get { return Price.Total; } }
    float fixedPrice;
    float discount;
    float Discount { get { return discount; } }
    float tip;
    float Tip { get { return tip; } }
    internal float initialFee;
    internal List<Payment> payments = new();
    float paidAmount { get { return GetPaidSum(); } }
    internal float PaidAmount => paidAmount;
    internal string PaymentNames => GetPaymentNames();
    #region Methods
    public void ChangeTax(Tax newTax)
    {
        if(tax != null)
        {
            tax = newTax;
            Price.SetMin(tax.MinPrice);
            taxes.Add(newTax);
            UpdateFixed(tax);
        }
    }
    public void ChangeToBaseTax(Tax baseTax)
    {
        if(tax != null)
        {
            tax = baseTax;
            Price.SetMin(tax.MinPrice);
            taxes.Add(baseTax);
        }
        if(taxManager.Previous != null && taxManager.Previous.isFixed)
        {
            Price.SetInitial(taxManager.Previous.FixedPrice);
        }
    }
    public Variable GetVariable(Variable.Type type)
    {
        return type switch
        {
            Variable.Type.Time => this.timeVar,
            Variable.Type.Km => this.kmVar,
            Variable.Type.Wait => this.waitVar,
            Variable.Type.Slow => this.slowDriveVar,
            _ => null 
        };
    }
    public void UpdatePrice()
    {
        UpdateDynamic();
    }
    void UpdateFixed(Tax tax)
    {
        if(tax.isFixed)
        {
            Price.SetFixed(tax.FixedPrice);
            Price.SetBase(PriceType.FIXED);
        }
    }
    public void UpdateDynamic()
    {
        fixedPrice = taxManager.GetFixed();
        costs = kmVar.Price + timeVar.Price + waitVar.Price + slowDriveVar.Price;
        Debug.Log($"Adding costs {costs}");
        //Price.CurrentBase.AddCosts(costs);
        //noFeesPrice = costs + initialFee + fixedPrice;
    }
    public void UpdateCosts(float amount)
    {
        Price.CurrentBase.AddCosts(amount);
    }
    public float GetBasicPrice()
    {
        UpdatePrice();
        float full = Price.Basis.GetSum() + GetExtraFeePrice() - paidAmount;
        Debug.Log("Basic price is " + full);
        return full;
    }
    public float GetPrice()
    {
        UpdatePrice();
        float full = Price.Total + GetExtraFeePrice() - paidAmount;
        return full;
    }
    public float GetFinalPrice()
    {
        UpdatePrice();
        Price finalPrice = new Price(this.Price);
        BasePriceManager.CheckBaseFinalStatic(finalPrice);
        float full = finalPrice.Total + GetExtraFeePrice() - paidAmount;
        Debug.Log("Final price is " + full);
        return full;
    }
    public void UpdateBase(bool toCheckOut)
    {
        if(toCheckOut)
        {
            basePriceManager.CheckBaseFinal();
        }
        else
        {
            basePriceManager.CheckBaseComission();
        }
    }
    public float GetExtraFeePrice()
    {
        float sum = 0;
        foreach (var item in extraFees)
        {
            sum += item.Price;
        }
        return sum;
    }
    public string GetExtraFeeNames()
    {
        string s = "";
        foreach (var item in extraFees)
        {
            s += item.Name + ": " + item.Price.ToString() + ",\n"; 
        }
        return s;
    }
    public float GetPaidSum()
    {
        float sum = 0;
        foreach(var item in payments)
        {
            sum += item.PaySum;
        }
        return sum;
    }
    public string GetPaymentNames()
    {
        string s = "";
        foreach(var item in payments)
        {
            s += item.PayType + ":" + item.PaySum + ",\n"; 
        }
        return s;
    }
    public void AssignSum(float sum)
    {
        Debug.Log("Setting sum to " + sum);
        //BasePrice.SetTo(sum);
    }
    public void SetTaxManager(TaxManager taxManager)
    {
        this.taxManager = taxManager;
    }
    public void PayAmount(Payment payment)
    {
        float verifiedAmount = PaymentCheck(payment.PaySum);
        payment.SetAmount(verifiedAmount);
        payments.Add(payment);
    }
    public float PaymentCheck(float amount)
    {
        if(GetFinalPrice() - amount < 0)
        {
            return GetFinalPrice();
        }
        else
        {
            return amount;
        }        
    }
    Tax GetPrevious()
    {
        int index = taxes.Count > 1 ? taxes.Count - 2 : 0;
        return taxes[index];
    }
    #endregion
}
#endregion
