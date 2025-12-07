using Input;
using Navigation;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni
namespace Payments
{
    public interface IPayment
    {
        public void AddPayment();
    }
    public class Payment : IPayment
    {
        public Payment(){}
        public Payment(PriceStatusManager manager)
        {
            statusManager = manager;
            this.sum = statusManager.FinalPrice;
            this.isDivided = SetDivisionStatus(payType);
        }
        public Payment(Method payType, PriceStatusManager manager)
        {
            this.payType = payType;
            this.isDivided = SetDivisionStatus(payType);
            statusManager = manager;
            this.sum = statusManager.FinalPrice;
        }
        protected PriceStatusManager statusManager;
        bool isDivided;
        public bool IsDivided => isDivided;
        protected Method payType;
        public Method PayType => payType;
        protected float sum;
        protected float remainder;
        public float Remainder => remainder;
        protected float paySum;
        public float PaySum => paySum;
        protected bool bypassReceipt;
        public bool ByPassReceipt => bypassReceipt;
        static bool SetDivisionStatus(Method payType)
        {
            if(payType == Method.Partial || payType == Method.Divided)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddPayment()
        {
            statusManager.AddPayment(this);
        }
        public void SetAmount(float amount)
        {
            paySum = amount;
        }
        public static Method GetPayMethodFromInputType(InputType type)
        {
            return type switch
            {
                InputType.Divided => Method.Divided,
                InputType.Partial => Method.Partial,
                _ => Method.BypassCash
            };
        }
    }
    public class PartialPay : Payment
    {
        public PartialPay(int amount, PriceStatusManager manager, InputMode mode) : base(manager)
        {
            this.mode = mode;
            this.paySum = GetPartial(amount);
            Debug.Log($"Partial pay format: {mode.Current.Format} and amount: {partialAmount}");
            this.payType = Method.Partial;
            this.remainder = sum - paySum;
            Debug.Log($"Partial pay remainder is {remainder}");
        }
        float partialAmount;
        InputMode mode;
        float GetPartial(int amount)
        {
            if(mode.Current.Format == Format.PERCENT)
            {
                //Now minimum price gets here.
                float partial = statusManager.FinalPrice * amount / 100f;
                return partial;
            }
            else
            {
                return amount;
            }
        }
    }
    public class DividedPay : Payment
    {
        public DividedPay(int divider, PriceStatusManager manager) : base(manager)
        {
            this.divider = divider;
            this.paySum = sum / divider;
            this.payType = Method.Divided;
            this.remainder = sum * (1f - 1f/divider);
            Debug.Log($"Divided pay divider is {divider}, pay sum is {paySum} and remainder is {remainder}");
        }
        int divider;
    }
    public class BypassCash : Payment
    {
        public BypassCash(PriceStatusManager manager) : base(manager)
        {
            bypassReceipt = true;
            this.payType = Method.BypassCash;
            this.remainder = 0f;
        }
    }
    public class SimplePay : Payment
    {
        public SimplePay(Method payType, PriceStatusManager manager) : base (payType, manager)
        {
            bypassReceipt = false;
            this.remainder = 0f;
        }
    }
}