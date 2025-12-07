using Input;
using Taxes;
using Payments;
using UnityEngine;
using Unity.VisualScripting;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation
{
    //Specific navigation processes, inherit the 'INavigation'-interface through parent class 'NavigationFunction'.
    #region Hold
    public class HoldFunction : NavigationFunction 
    {
        public HoldFunction(NavigationManager manager) : base (manager)
        {
            deactivate = manager.GetCanvasFromButtonType(ButtonType.Options);
            activate = manager.GetCanvasFromStatus();
        }
    }
    #endregion
    #region Back
    public class BackFunction : NavigationFunction 
    {
        public BackFunction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromStatus();
            function = ChangeActivation;
        }
    }
    #endregion
    #region Next
    public class NextFunction : NavigationFunction
    {
        public NextFunction(NavigationManager manager) : base (manager)
        {
            this.function = ExitPayment;
        }
        void ExitPayment()
        {
            if(manager.EndComission)
            {
                new ExitComission(manager).Operate();
            }
            else
            {
                new ExitToComission(manager).Operate();
            }
            manager.EndComission = false;
        }
    }
    #endregion
    #region Tax
    public class TaxFunction : NavigationFunction
    {
        float sum;
        public TaxFunction(NavigationManager manager) : base (manager)
        {
            deactivate = manager.GetCanvasFromStatus();
            sum = manager.StatusIntance.Price; 
            if(manager.Status == State.Checkout)
            {
                activate = manager.GetCanvasFromStatus(State.Comission);
                this.function = TaxButtonAtCheckOut;
            }
            else
            {
                activate = manager.GetCanvasFromButtonType(ButtonType.Tax);
                this.function = ActivateDeactivate; 
            }
        }
        //Special function to handle the 'tax' button at checkout mode.
        void TaxButtonAtCheckOut()
        {
            Tax tax = manager.StatusIntance.CurrentTax;
            if (tax.MaxPrice != 0 && sum >= tax.MaxPrice)
            {
                Debug.LogWarning("Max price has been reached. Cannot enter comission mode.");
                return;
            }
            if(tax.isFixed)
            {
                Debug.Log("Tax is fixed!");
                TaxButtonFixedPrice();
            }
            else
            {
                Debug.Log("No max price or  fixed price, return to comission");
                ReturnToComission();
            }
        }
        void ReturnToComission()
        {
            //Actions.ReturnToComission?.Invoke();
            manager.StatusIntance.currentStatus.UpdateBase(false);
            sum = manager.StatusIntance.Price;
            Debug.Log("Return to comission price is " + sum);
            Actions.ForcePrice(sum);
            ActivateDeactivate();
            manager.ForceNewStatus(State.Comission);
        }
        void TaxButtonFixedPrice()
        {
            if(manager.StatusIntance.taxManager.GetBaseStatus())
            {
                Tax baseTax = manager.StatusIntance.taxManager.GetBase();
                //Actions.NewTax(baseTax);
                Actions.BackToBase(baseTax);
                ReturnToComission();
            }
            else
            {
                Debug.LogWarning($"Cannot return to comission, no base tax selected!");
            }
        }
    }
    #endregion
    #region Tax select
    public class TaxSelectFunction : NavigationFunction
    {
        Tax newTax;
        public TaxSelectFunction(NavigationManager manager, Tax newTax) : base (manager)
        {
            this.newTax = newTax;
            deactivate = manager.GetTaxCanvasFromStatus();
            if(newTax.isFixed)
            {
                activate = manager.GetCanvasFromButtonType(ButtonType.Kassa);
                this.function += UpdateCheckOut;
            }
            else
            {
                activate = manager.GetCanvasFromStatus(State.Comission);
                manager.ForceNewStatus(State.Comission);
            }
        }
        void UpdateCheckOut()
        {
            manager.ForceNewStatus(State.Checkout);
            Actions.ForcePrice(newTax.FixedPrice);
        }
    }
    #endregion
    #region Options
    public class OptionsFunction : NavigationFunction
    {
        public OptionsFunction(NavigationManager manager) : base (manager)
        {
            if(manager.Status == State.Comission || manager.Status == State.Checkout)
            {
                activate = manager.GetCanvasFromButtonType(ButtonType.Options);
                this.function = ChangeActivation;
            }
            else
            {
                //Add functionality here.
                this.function = Empty;
            }
        }
    }
    #endregion
    #region Customer
    public class AddCustomerFunction : NavigationFunction
    {
        public AddCustomerFunction(NavigationManager manager) : base (manager)
        {
            if(manager.PriceInstance.Drive)
            {
                Debug.LogWarning("You must stop to add a new customer!");
                this.function = Empty;
            }
            else
            {
                activate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
                this.function = AddCustomer;
            }
        }
        void AddCustomer()
        {
            activate.SetActive(true); //setactive(!activate.activeSelf);
            int mode = (int)InputType.Customer;
            Actions.SetNumberInputMode?.Invoke(mode);
        }
    }
    #endregion
    #region Checkout
    public class CheckOutFuction : NavigationFunction
    {
        public CheckOutFuction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.Kassa);
            deactivate = manager.GetCanvasFromStatus();
            this.function += EnterCheckOut;
        }
        void EnterCheckOut()
        {
            manager.StatusIntance.currentStatus.UpdateBase(true);
            float price = manager.StatusIntance.FinalPrice;
            Debug.Log("Entering checkout final price is " + price);
            Actions.ForcePrice?.Invoke(price);
            manager.ForceNewStatus(State.Checkout);
        }
    }
    #endregion
    #region Max price
    public class MaxPriceFunction : NavigationFunction
    {
        public MaxPriceFunction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            deactivate = manager.GetCanvasFromButtonType(ButtonType.Options);
            this.function += EnterMaxPriceInsertion;
        }
        void EnterMaxPriceInsertion()
        {
            int mode = (int)InputType.MaxPrice;
            Actions.SetNumberInputMode?.Invoke(mode);
        }
    }
    #endregion
    #region Max time
    public class MaxTimeFunction : NavigationFunction
    {
        public MaxTimeFunction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            deactivate = manager.GetCanvasFromButtonType(ButtonType.Options);
            this.function += EnterMaxTimeInsertion;
        }
        void EnterMaxTimeInsertion()
        {
            int mode = (int)InputType.MaxTime;
            Actions.SetNumberInputMode?.Invoke(mode);
        }
    }
    #endregion
    #region Extras
    public class ExtraFeeFunction : NavigationFunction
    {
        public ExtraFeeFunction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.ExtraFee);
            deactivate = manager.GetCanvasFromStatus();
            this.function = ChangeActivationBoth;
            manager.ForceNewStatus(State.Checkout);
        }
    }
    #endregion
    #region Pay
    public class PayFunction : NavigationFunction
    {
        public PayFunction(NavigationManager manager) : base (manager)
        {
            activate = manager.GetCanvasFromButtonType(ButtonType.Pay);
            deactivate = manager.GetCanvasFromStatus();
            this.function += EnterPayment;
        }
        void EnterPayment()
        {
            float price = manager.StatusIntance.FinalPrice;
            Actions.ForcePrice?.Invoke(price);
            manager.UpdatePaymentView(PaymentView.Payment);
            manager.ForceNewStatus(State.Payment);
        }
    }
    #endregion
    #region ExitInput
    public class ExitInput : NavigationFunction
    {
        public ExitInput(NavigationManager manager) : base(manager)
        {
            activate = manager.GetCanvasFromStatus();
            deactivate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
        }
        public ExitInput(NavigationManager manager, InputMode mode) : base(manager)
        {
            activate = manager.GetCanvasFromStatus();
            deactivate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            AssignValues(mode.inputType);
        }
        void AssignValues(InputType type)
        {
            if(type == InputType.Partial || type == InputType.Divided)
            {
                Debug.Log("Exiting to comission");
                this.function = new ExitPaymentInput(manager, type).Operate;
            }
            else
            {
                Debug.Log("Normal input exit!");
            }
        }
    }
    #endregion
    #region Payment input
    public class ExitPaymentInput : NavigationFunction
    {
        InputType inputType;
        public ExitPaymentInput(NavigationManager manager, InputType type) : base(manager)
        {
            activate = manager.GetPaymentCanvasFromView(PaymentView.Secondary);
            deactivate = manager.GetCanvasFromButtonType(ButtonType.NumberInput);
            this.inputType = type;
            Debug.Log($"activate {activate.name}, deactivate {deactivate.name}");
            this.function += EnterSecondaryPayment;
        }
        void EnterSecondaryPayment()
        {
            manager.UpdatePaymentView(PaymentView.Secondary);
            Actions.SecondaryPayment?.Invoke(inputType);
        }
    }
    #endregion
    public class ExitComission : NavigationFunction
    {
        public ExitComission(NavigationManager manager) : base(manager)
        {
            activate = manager.GetCanvasFromStatus(State.Free);
            deactivate = manager.GetPaymentCanvasFromView();
            manager.ForceNewStatus(State.Free);
            manager.UpdatePaymentView(PaymentView.None);
        }
    }
    public class ExitToComission : NavigationFunction
    {
        public ExitToComission(NavigationManager manager) : base(manager)
        {
            activate = manager.GetCanvasFromStatus(State.Comission);
            deactivate = manager.GetPaymentCanvasFromView();
            manager.ForceNewStatus(State.Comission);
            manager.UpdatePaymentView(PaymentView.None);
        }
    }
}
