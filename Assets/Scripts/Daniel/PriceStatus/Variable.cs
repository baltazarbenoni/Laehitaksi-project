using UnityEngine;
using Taxes;

#region Variables
public class Variable 
{
    public Variable(Type type)
    {
        this.type = type;
    }
    //Price stores the price for a certain type of cost.
    protected float price;
    public float Price { get { return price; } }
    //Amount stores the AMOUNT of this cost: meters for kilometer tax, secods for time tax and wait tax.
    protected float amount;
    public float Amount { get { return amount; } }
    Type type;
    public Type Vtype { get { return type; } }
    public enum Type
    {
        None,
        Km,
        Time,
        Wait,
        Slow
    }
    public void UpdateAll(float price, float amount)
    {
        this.price += price;
        this.amount += amount;
    }
    public void AddPrice(float price)
    {
        this.price += price;
    }
    public void AddAmount(float amount)
    {
        this.amount += amount;
    }

}
#endregion
#region Wait
public class WaitVariable : Variable
{
    PriceStatus parent;
    public WaitVariable(Type type, PriceStatus parent) : base(type)
    {
        this.parent = parent;
    }
    public new void UpdateAll(float price, float amount)
    {
        this.price += price;
        this.amount += amount;
        if(parent.TaxInUse.MaxWaitPrice != 0)
        {
            MonitorMaxPrice();
        }
        if(parent.TaxInUse.MaxWaitTime != 0)
        {
            MonitorMaxTime();
        }
    }
    void MonitorMaxPrice()
    {
        Tax tax = parent.TaxInUse;
        if(this.price >= tax.MaxWaitPrice)
        {
            Debug.Log("Wait price exceeds max, wait has ended");
            tax.StopWait();
        }
    }
    void MonitorMaxTime()
    {
        Tax tax = parent.TaxInUse;
        if(tax.WaitTimer.Time >= tax.MaxWaitTime)
        {
            Debug.Log("Wait time exceeds max wait, wait has ended");
            tax.StopWait();
        }
    }
}
#endregion
