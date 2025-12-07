using System.Collections;
using Input;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Payments
{
    public class PaymentManager : MonoBehaviour
    {
        [SerializeField] GameObject statusObject;
        PriceStatusManager statusManager;
        public PriceStatusManager StatusManager => statusManager;
        PaymentHandler handler;
        bool paying;
        void Awake()
        {
            statusManager = References.GetRef(gameObject, statusObject, statusManager);
        }
        void OnEnable()
        {
            //InitEvents();
        }
        void OnDestroy()
        {
            //Unsubscribe();
        }
        void InitEvents()
        {
        }
        void Unsubscribe()
        {
        }
        void OnPayment(Method paymentMethod)
        {
            if(!paying)
            {
                handler = new PaymentHandler(statusManager);
                handler.InitPay(paymentMethod);
                StartCoroutine(PaymentEndingDelay());
            }
        }
        public void OnPaymentInput(InputMode mode, int num)
        {
            if(!paying)
            {
                paying = true;
                Method paymentType = Payment.GetPayMethodFromInputType(mode.inputType);
                handler = new PaymentHandler(statusManager, mode);
                handler.InitPay(paymentType, num);
                StartCoroutine(PaymentEndingDelay());
            }
        }
        IEnumerator PaymentEndingDelay()
        {
            yield return new WaitForSeconds(0.1f);
            paying = false;
        }
    }
}