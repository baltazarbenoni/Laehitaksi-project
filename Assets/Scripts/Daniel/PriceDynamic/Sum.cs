using UnityEngine;

public class Sum
{
    bool usingFixedPrice;
    bool initialPriceAdded;
    internal bool InitialPriceAdded {
        get
        {
            return initialPriceAdded;
        }
        set
        {
            initialPriceAdded = value;
        }
    }
    bool hasMaxPrice;
    float costs;
    internal float Costs
    {
        get
        {
            return costs;
        }
        set
        {
            costs = value;
        }
    }
    float basis;
    internal float Basis
    {
        get
        {
            return basis;
        }
        set
        {
            basis = value;
        }
    }
    float amount;
    internal float Amount
    {
        get
        {
            return amount;

        }
        set
        {
            amount = value;
        }
    }
}
