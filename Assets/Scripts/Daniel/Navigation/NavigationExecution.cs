
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation
{
    //Manager to execute navigation. Creates a navigation function -object that inherits from 'INavigation' (through 'NavigationFunction' parent class) and uses its 'Operate'-method.
    public class NavigationExecution 
    {
        public void Apply(ButtonType type, NavigationManager manager)
        {
            INavigation obj = GetObject(type, manager);
            obj.Operate();
        }
        public void Apply(Payments.Method type, bool isSecondary, NavigationManager manager)
        {
            INavigation obj = GetObject(type, isSecondary, manager);
            obj.Operate();
        }
        INavigation GetObject(ButtonType type, NavigationManager manager)
        {
            return type switch
            {
                ButtonType.Back => new BackFunction(manager),
                ButtonType.Next => new NextFunction(manager),
                ButtonType.Tax => new TaxFunction(manager),
                ButtonType.Hold => new HoldFunction(manager),
                ButtonType.Options => new OptionsFunction(manager),
                ButtonType.AddCustomer => new AddCustomerFunction(manager),
                ButtonType.Kassa => new CheckOutFuction(manager),
                ButtonType.MaxPrice => new MaxPriceFunction(manager),
                ButtonType.MaxTime => new MaxTimeFunction(manager),
                ButtonType.ExtraFee => new ExtraFeeFunction(manager),
                ButtonType.Pay => new PayFunction(manager),
                _ => new NavigationFunction() 
            };
        }
        INavigation GetObject(Payments.Method type, bool isSecondary, NavigationManager manager)
        {
            return type switch
            {
                Payments.Method.BypassCash => new BypassCash(manager, isSecondary),
                Payments.Method.Partial => new PartialPay(manager),
                Payments.Method.Divided => new DividedPay(manager),
                _ => new SimplePayment(type, manager, isSecondary)
            };
        }
    }
}
