using UnityEngine;
using Taxes;
public class Cost
{
    public Cost(PriceUpdate manager)
    {
        price = GetCost(manager);
        updateManager = manager;
    }
    public Cost(PriceUpdate manager, PriceStatusManager status)
    {
        price = GetCost(manager);
        updateManager = manager;
        UpdateStatus(status);
    }
    protected PriceUpdate updateManager;
    protected float price;
    public float Price { get { return price; } }
    internal virtual float GetCost(PriceUpdate manager)
    {
        return 0;
    }
    protected virtual void UpdateStatus(PriceStatusManager status) {}
}
public class KmCost : Cost
{
    float distance;
    public float Distance { get { return distance; } }
    public KmCost(PriceUpdate manager) : base (manager){}
    public KmCost(PriceUpdate manager, PriceStatusManager status) : base (manager, status){}
    internal override float GetCost(PriceUpdate manager)
    {
        Tax tax = manager.CurrentTax;
        Debug.Log("Adding Km tax : " + tax.Rate);
        //Calculate the accumulated price according to traveled distance.
        //and add traveled distance to this cost.
        float cost = AddTaxPerKm(manager, out float meters);
        distance = meters;
        Debug.Log("Traveled distance " + distance);
        return cost;
    }
    static float AddTaxPerKm(PriceUpdate manager, out float meters)
    {
        //Calculate the traveled distance in meters.
        meters = manager.Speed / 3.6f * manager.MainTimer.Limit;
        //Calculate price for this distance.
        float taxPerMeter = manager.CurrentTax.Rate / 1000f;
        float cost = meters * taxPerMeter;
        return cost;
    }
    protected override void UpdateStatus(PriceStatusManager status)
    {
        if(status == null)
        {
            Debug.LogWarning("price status manager script is null!");
            return;
        }
        Debug.Log("Updating price status");
        status.UpdateKm(this);
    }
}
public class TimeCost : Cost
{
    public TimeCost(PriceUpdate manager) : base (manager){}
    public TimeCost(PriceUpdate manager, PriceStatusManager status) : base (manager, status){}
    internal override float GetCost(PriceUpdate manager)
    {
        Tax tax = manager.CurrentTax;
        if (tax.BySec != 0)
        {
            Debug.Log("Adding time tax : " + tax.ByMin + ", " + tax.BySec);
            return tax.BySec * manager.MainTimer.Limit;
        }
        else
        {
            return 0;
        }
    }
    protected override void UpdateStatus(PriceStatusManager status)
    {
        if(status == null)
        {
            Debug.LogWarning("price status manager script is null!");
            return;
        }
        Variable.Type costType = updateManager.Drive ? Variable.Type.Time : Variable.Type.Slow;
        Debug.Log("Updating price status");
        status.UpdateTime(costType, price, updateManager.MainTimer.Limit);
    }
}
public class WaitCost : Cost
{
    public WaitCost(PriceUpdate manager) : base (manager){}
    public WaitCost(PriceUpdate manager, PriceStatusManager status) : base (manager, status){}
    internal override float GetCost(PriceUpdate manager)
    {
        manager.CurrentTax.MonitorWait();
        WaitTimer timer = manager.CurrentTax.WaitTimer;
        if (timer == null || timer.WaitHasEnded)
        {
            Debug.Log("Wait timer is null or has ended");
            return 0;
        }
        else
        {
            Debug.Log("Adding wait tax : " + manager.CurrentTax.WaitTax);
            manager.CurrentTax.WaitTimer.Time += manager.MainTimer.Limit;
            float price = manager.CurrentTax.WaitTax * manager.MainTimer.Limit;
            return price;
        }
    }
    protected override void UpdateStatus(PriceStatusManager status)
    {
        if(status == null)
        {
            Debug.LogWarning("price status manager script is null!");
            return;
        }
        Debug.Log("Updating price status");
        status.UpdateTime(Variable.Type.Wait, price, updateManager.MainTimer.Limit);
    }
}

