using Input;
using Navigation;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni
namespace Payments
{
    public class PaymentHandler
    {
        public PaymentHandler(PriceStatusManager manager, InputMode mode)
        {
            this.statusManager = manager;
            this.mode = mode;
        }
        public PaymentHandler(PriceStatusManager manager)
        {
            this.statusManager = manager;
        }
        PriceStatusManager statusManager;
        InputMode mode;
        public InputMode Mode => mode;
        IPayment transaction;
        public void InitPay(Method payment)
        {
            transaction = GetSimplePayment(payment, statusManager);
            transaction.AddPayment();
        }
        public void Complete()
        {
            statusManager.HandlePayment();
        }
        public void InitPay(Method payment, int input)
        {
            IPayment transaction = GetComplexPayment(payment, input, statusManager, mode);
            transaction.AddPayment();
        }
        IPayment GetComplexPayment(Method payment, int input, PriceStatusManager manager, InputMode mode)
        {
            return payment switch
            {
                Method.Divided => new DividedPay(input, manager),
                Method.Partial => new PartialPay(input, manager, mode),
                _ => new Payment()
            };
        }
        IPayment GetSimplePayment(Method payment, PriceStatusManager manager)
        {
            return payment switch
            {
                Method.BypassCash => new BypassCash(manager), 
                _ => new SimplePay(payment, manager) 
            };
        }
        /*IPayment GetSimplePayment(Method payment, PriceStatusManager manager)
        {
            return payment switch
            {
                Method.Sote => new SotePay(),
                Method.Suorakorvaus => new Suorakorvaus(),
                Method.Taxcard => new Taxcard(),
                Method.Billing => new Billing(),
                Method.DebitCredit => new DebitCredit(),
                Method.BypassCash => new BypassCash(manager), 
                Method.Cash => new Cash(),
                _ => new Empty() 
            };
        }*/
        
    }

}