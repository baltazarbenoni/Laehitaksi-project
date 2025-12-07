using Taxes;
using Payments;
using System.Collections.Generic;
using UnityEngine;
/*
//C 2025 Daniel Snapir alias Baltazar Benoni
#region Status
public class PriceStatusOld
{
    public PriceStatusOld()
    {
        tax = new Tax();
        //waitVar = new WaitVariable(Variable.Type.Wait, this);
        taxManager = new();
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
    float price;
    internal float Price { get { UpdatePrice(); return price; } }
    float noFeesPrice;
    internal float NoFeesPrice { get { return noFeesPrice; } }
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
    bool minimumPriceAdded;
    #region Methods
    public void ChangeTax(Tax newTax)
    {
        if(tax != null)
        {
            tax = newTax;
            minimumPriceAdded = false;
            taxes.Add(newTax);
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
        if(tax.isFixed)
        {
            UpdateFixed();
        }
        else
        {
            UpdateDynamic();
        }
    }
    public void UpdateFixed()
    {
        noFeesPrice = tax.FixedPrice;
        price = noFeesPrice - paidAmount;
    }
    public void UpdateDynamic()
    {
        fixedPrice = taxManager.GetFixed();
        noFeesPrice = kmVar.Price + timeVar.Price + waitVar.Price + slowDriveVar.Price + initialFee + fixedPrice;
        price = noFeesPrice + GetExtraFeePrice() - paidAmount;
        Debug.Log("Internal price is " + price);
    }
    public float GetFinalPrice()
    {
        float final = MinPriceVerification();
        return final + GetExtraFeePrice() - paidAmount;
    }
    float MinPriceVerification()
    {
        if(tax.MinPrice == 0 || minimumPriceAdded)
        {
            return noFeesPrice;
        }
        else if(noFeesPrice < tax.MinPrice)
        {
            return tax.MinPrice;
        }
        else
        {
            return noFeesPrice;
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
    public float GetMinimum()
    {
        if(tax != null)
        {
            return tax.MinPrice;
        }
        return 0;
    }
    public void AssignSum(float sum)
    {
        Debug.Log("Setting sum to " + sum);
        price = sum;
    }
    public void SetTaxManager(TaxManager taxManager)
    {
        this.taxManager = taxManager;
    }
    public void PayAmount(Payment payment)
    {
        payments.Add(payment);
        minimumPriceAdded = true;
    }
    public bool PaymentCheck(float amount)
    {
        if(GetFinalPrice() - amount < 0)
        {
            return false;
        }
        else
        {
            return true;
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


*/