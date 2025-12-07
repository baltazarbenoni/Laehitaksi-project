using Payments;
using UnityEngine;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation
{
    #region Parent
    public class PaymentNavigation : NavigationFunction
    {
        protected Method paymentType;
        protected PaymentHandler handler;
        protected bool isSecondary;
        public PaymentNavigation(NavigationManager manager) : base (manager){}
        protected void ExitComissionNavigation()
        {
            new ExitComission(manager).Operate();
        }
        protected void BackToComissionNavigation()
        {
            new ExitToComission(manager).Operate();
        }
        protected void AssignExitFunction()
        {
            if(isSecondary)
            {
                this.function += BackToComissionNavigation;
            }
            else
            {
                this.function += ExitComissionNavigation;
            }
        }
        protected void CreatePayment()
        {
            if(!isSecondary)
            {
                handler.InitPay(paymentType);
            }
        }
    }
    #endregion
    #region Bypass
    public class BypassCash : PaymentNavigation 
    {
        public BypassCash(NavigationManager manager, bool isSecondary) : base (manager)
        {
            this.isSecondary = isSecondary;
            paymentType = Method.BypassCash; 
            handler = new(manager.StatusIntance);
            this.function = OperatePayment;
            AssignExitFunction();
        }
        void OperatePayment()
        {
            CreatePayment();
            handler.Complete();
        }
    }
    #endregion
    #region Simple
    public class SimplePayment : PaymentNavigation 
    {
        public SimplePayment(Method type, NavigationManager manager, bool isSecondary) : base (manager)
        {
            this.isSecondary = isSecondary;
            this.paymentType = type;
            handler = new(manager.StatusIntance);
            this.function += OperatePayment;
            activate = manager.GetPaymentCanvasFromView(PaymentView.Execution);
            deactivate = manager.GetPaymentCanvasFromBool(isSecondary);
            manager.UpdatePaymentView(PaymentView.Execution);
        }
        void OperatePayment()
        {
            CreatePayment();
            handler.Complete();
        }
    }
    #endregion
    #region Execution
    public class PaymentExecution : PaymentNavigation
    {
        public PaymentExecution(NavigationManager manager, bool isSecondary) : base (manager)
        {
            AssignExitFunction();
        }
    }
    #endregion
    #region Partial
    public class PartialPay : NavigationFunction
    {
        public PartialPay(NavigationManager manager) : base(manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            deactivate = manager.GetCanvasFromStatus();
            function += EnterInsertion;
        }
        void EnterInsertion()
        {
            int mode = (int)InputType.Partial;
            manager.ForceNewStatus(State.Payment);
            Actions.SetNumberInputMode?.Invoke(mode);
        }
    }
    #endregion
    #region Divided 
    public class DividedPay : NavigationFunction
    {
        public DividedPay(NavigationManager manager) : base(manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            deactivate = manager.GetCanvasFromStatus();
            function += EnterInsertion;
        }
        void EnterInsertion()
        {
            int mode = (int)InputType.Divided;
            manager.ForceNewStatus(State.Payment);
            Actions.SetNumberInputMode?.Invoke(mode);
        }
    }
    #endregion
}