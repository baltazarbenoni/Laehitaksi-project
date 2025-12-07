using UnityEngine;
using Taxes;
using Input;
//C 2025 Daniel Snapir alias Baltazar Benoni
public interface IHandler
{
    public void Process(int num, InputMode mode);
}
public class InputHandler : IHandler
{
    public InputHandler(ProcessInput manager)
    {
        this.manager = manager;
    }
    public virtual void Process(int num, InputMode mode){}
    protected ProcessInput manager;
}
public class PaymentProcessor : InputHandler
{
    public PaymentProcessor(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {
        if(manager.PaymentManager != null)
        {
            manager.PaymentManager.OnPaymentInput(mode, num);
        }
        else
        {
            Debug.LogWarning("Payment manager is null in input processing OR payment exceeds the current price!");
        }
    }
}
public class MaxPriceProcessor : InputHandler
{
    public MaxPriceProcessor(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {
        Tax tax = manager.TaxInUse;
        Debug.Log("Added max price " + num);
        tax.AddMaxPrice(num);
    }
}
public class MaxWaitProcessor : InputHandler
{
    public MaxWaitProcessor(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {
        Tax tax = manager.TaxInUse;
        InputFormat format = mode.Current;
        Debug.Log($"Max wait: tax: {tax.Name}, format: {format.Format}");
        if(tax.WaitTax == 0)
        {
            Debug.Log("Max wait: Wait tax is zero!");
            return;
        }
        if(format.Format == Format.MIN)
        {
            AddMaxWaitTime(tax, num);
        }
        else if(format.Format == Format.EURO)
        {
            AddMaxWaitPrice(tax, num);
        }
    }
    static void AddMaxWaitTime(Tax tax, int minutes)
    {
        int seconds = minutes * 60;
        Debug.Log($"max wait: added max wait to current tax {tax.Name}, {seconds}");
        tax.AddMaxWaitTime(seconds);
    }
    static void AddMaxWaitPrice(Tax tax, int price)
    {
        Debug.Log($"max wait: added max price to current tax {tax}, {price}");
        //To avoid decimals, system uses integers multiplied by 100.
        tax.AddMaxWaitPrice(price);
    }
}
public class CustomerInputProcessor : InputHandler
{
    public CustomerInputProcessor(ProcessInput manager) : base(manager){} 
    public override void Process(int num, InputMode mode)
    {
    /*    float sum = statusManager.Sum;
        //If first addition, create 2 new customers: the past one and the current one.
        if (customers.Count == 0)
        {
            Customer firstCustomer = new Customer(0, sum);
            customers.Add(firstCustomer);
            Debug.Log("Added customer with number: " + customerNum);
            Customer newCustomer = new Customer(customerNum);
            customers.Add(newCustomer);
        }
        //When creating a new customer, assign the price for the previous one.
        else
        {
            float previousSum = sum - customers[customers.Count - 2].Sum;
            customers[customers.Count - 1].Sum = previousSum;
            Debug.Log("Added customer with number: " + customerNum);
            Customer newCustomer = new Customer(customerNum);
            customers.Add(newCustomer);
        }
        statusManager.UpdateCustomers(customers);
    */
    }
}
public class PaymentInput : InputHandler
{
    public PaymentInput(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {
        Actions.PaymentInput?.Invoke(mode, num);
    }
}
public class DividedPayProcessor : InputHandler
{
    public DividedPayProcessor(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {

    }
}
public class PartialPayProcessor : InputHandler
{
    public PartialPayProcessor(ProcessInput manager) : base(manager){}
    public override void Process(int num, InputMode mode)
    {

    }
}
