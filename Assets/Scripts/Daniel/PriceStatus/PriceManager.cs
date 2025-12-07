using UnityEngine;
using System.Collections.Generic;
using Taxes; 
using Payments;
using Prices;

public class BasePriceManager
{
    public BasePriceManager(PriceStatus host)
    {
        this.host = host; 
    }
    PriceStatus host;
    //Method to check if base should be changed when moving back to comission.
    public void CheckBaseComission()
    {
        if(host.extraFees.Count != 0 || host.payments.Count != 0)
        {
            //host.Price.SetBase(PriceType.MINIMUM);
            return;
        }
        else
        {
            host.Price.SetBase(PriceType.BASIS);
        }
    }
    //Method to check if base should be changed when moving to checkout.
    public void CheckBaseFinal()
    {
        if(host.Price.CurrentBase.Name == PriceType.BASIS)
        {
            host.Price.SetBase(MinPriceCheck(host.Price));
        }
    }
    public static void CheckBaseFinalStatic(Price price)
    {
        if(price.CurrentBase.Name == PriceType.BASIS)
        {
            price.SetBase(MinPriceCheck(price));
        }
    }
    //Check minimum price against base price initial fee and costs.
    internal static PriceType MinPriceCheck(Price price)
    {
        if(price.Minimum.Constant == 0)
        {
            return PriceType.BASIS;
        }
        else if(price.Basis.Constant + price.Basis.Costs < price.Minimum.Constant)
        {
            return PriceType.MINIMUM;
        }
        else
        {
            return PriceType.BASIS;
        }
    }
}
