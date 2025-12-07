using UnityEngine;

namespace Prices
{
    public class Price
    {

        public Price()
        {
            basis = new BasePrice(PriceType.BASIS);
            minimum = new BasePrice(PriceType.MINIMUM);
            fixedPrice = new BasePrice(PriceType.FIXED);
            currentBase = basis;
        }
        public Price(Price price)
        {
            this.basis = price.basis;
            this.minimum = price.minimum;
            currentBase = price.currentBase;
        }
        BasePrice basis;
        internal BasePrice Basis => basis;
        BasePrice minimum;
        internal BasePrice Minimum => minimum;
        BasePrice fixedPrice;
        internal BasePrice Fixed => fixedPrice;
        BasePrice currentBase;
        internal BasePrice CurrentBase => currentBase;
        internal float Total
        {
            get
            {
                return currentBase.GetSum();
            }
        }
        public void SetBase(PriceType type)
        {
            Debug.Log($"Base: Setting base to {type}");
            if(type == PriceType.BASIS)
            {
                currentBase = basis;
            }
            else if(type == PriceType.MINIMUM)
            {
                currentBase = minimum;
            }
            else if(type == PriceType.FIXED)
            {
                currentBase = fixedPrice;
            }
        }
        public void SetMin(float amount)
        {
            Debug.Log($"Base: Minimum set to {amount}");
            minimum.SetTo(amount);
        }
        public void SetInitial(float amount)
        {
            Debug.Log($"Base: Initial set to {amount}");
            basis.SetTo(amount);
        }
        public void SetFixed(float amount)
        {
            Debug.Log($"Base: Fixed set to {amount}");
            fixedPrice.SetTo(amount);
        }
    }
    public class BasePrice
    {
        public BasePrice(PriceType type)
        {
            this.name = type;
        }
        public BasePrice(PriceType type, float amount)
        {
            this.name = type;
            this.constant = amount;
        }
        PriceType name;
        public PriceType Name => name;
        float constant;
        internal float Constant => constant;
        float costs;
        internal float Costs => costs;
        public void AddCosts(float num)
        {
            Debug.Log($"Base is {name}, Costs: Added {num}, costs are {costs}");
            costs += num;
        }
 
        public void SetTo(float num)
        {
            constant = num;
        }
        public float GetSum()
        {
            Debug.Log($"costs: {costs}, constant {constant}");
            return costs + constant;
        }
    }
    public enum PriceType
    {
        NONE,
        BASIS,
        MINIMUM,
        FIXED
    }
}