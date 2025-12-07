using UnityEngine;
using Navigation;
using Input;
using Payments;
//C 2025 Daniel Snapir alias Baltazar Benoni

public interface INumberInput
{
    public void UpdateMeter();
    public void AddNumber(int num);
    public void EraseNumber();
    public void ChangeFormat();
    public void Submit();
    public string GetText();
}
#region Parent
public class NumberInput : INumberInput
{
    public NumberInput(InputType type)
    {
        this.mode = new InputMode(type);
        this.text = mode.BaseText + mode.Current.Id;
        SubmitFunction = DefaultSubmit;
    }
    public NumberInput()
    {
        SubmitFunction = DefaultSubmit;
    }
    protected delegate void MyDelegate();
    protected MyDelegate SubmitFunction;
    protected InputMode mode;
    protected InputFormat format => mode.Current;
    public readonly string initialTxt;
    protected string text;
    protected int sum;
    public int Sum => sum;
    protected int sizeLimit = 5;
    #region Functions
    public void UpdateMeter() => NewText();
    public void NewText()
    {
        this.text = format.GetFormattedText(sum);
    }
    //If number is more than ten thousand (meaning more than 1000), return. Else add one digit.
    public void AddNumber(int num)
    {
        if (sum > Mathf.Pow(10, sizeLimit))
        {
            return;
        }
        else
        {
            sum *= 10;
            sum += num;
        }
    }
    //If number is less than ten (meaning less than 0.10), make the sum zero. Else remove one digit.
    public void EraseNumber()
    {
        if (sum >= 10)
        {
            int lastDigit = sum % 10;
            sum -= lastDigit;
            sum /= 10;
        }
        else
        {
            sum = 0;
        }
    }
    protected string GetInitialText()
    {
        return mode.inputType switch
        {
            InputType.MaxTime => "0",
            InputType.Addition => "0,00",
            InputType.Customer => "0",
            InputType.Divided => "0",
            InputType.MaxPrice => "0,00",
            _ => ""
        };
    }
    public string GetText()
    {
        return this.text;
    }
    public void ChangeFormat()
    {
        mode.ChangeFormat();
        this.text = format.GetFormattedText(sum);
        sum = 0;
    }
    public void Submit()
    {
        SubmitFunction();
    }
    protected void DefaultSubmit()
    {
        Actions.Input?.Invoke(mode, sum);
    }
    public static INumberInput GetHandler(InputType type, PriceStatusManager manager)
    {
        return type switch
        {
            InputType.MaxTime => new TimeInput(type),
            InputType.Customer => new CustomerInput(type),
            InputType.Divided => new DivisionInput(type),
            InputType.Partial => new PartialPayInput(type, manager),
            _ => new PriceInput(type)
        };
    }
#endregion
}
#endregion
#region Time input
public class TimeInput : NumberInput
{
    int minimum = 2;
    public TimeInput(InputType type) : base(type)
    {
        SubmitFunction = CheckSubmitAction;
        sizeLimit = 3;
    } 
    void CheckSubmitAction()
    {
        if(format.Format == Format.MIN && sum < minimum)
        {
            Debug.Log("Minimum wait time is 2min!");
            return;
        }
        else
        {
            DefaultSubmit();
        }
    }
}
#endregion
#region Price input
public class PriceInput : NumberInput
{
    public PriceInput(InputType type) : base(type){}
    /*public override void NewText()
    {
        int aboveZero = sum / 100;
        int decimals = sum - aboveZero * 100;
        string addition1 = GetZerosToAdd(decimals);
        string txt = $"{aboveZero},{addition1}{decimals}{mode.Current.Id}";
        text = txt;
    }*/
}
#endregion
#region Customer input
public class CustomerInput : NumberInput
{
    readonly int customerID = 111146;
    public CustomerInput(InputType type) : base(type)
    {
        SubmitFunction = CheckSubmitAction;
    }
    void CheckSubmitAction()
    {
        if(sum != customerID)
        {
            Debug.Log("Insert correct cusmer id!");
            return;
        }
        else
        {
            DefaultSubmit();
        }
    }
}
#endregion
#region Payments
public class DivisionInput : NumberInput
{
    public DivisionInput(InputType type) : base(type) {}

}
public class PartialPayInput : NumberInput
{
    PriceStatusManager manager;
    public PartialPayInput(InputType type, PriceStatusManager manager) : base(type)
    {
        this.manager = manager;
        SubmitFunction = CheckSubmitAction;
        sizeLimit = 3;
    }
    void CheckSubmitAction()
    {
        if(!InputIsValid())
        {
            Debug.Log("Inserted sum larger than the whole price, returning to zero");
        }
        DefaultSubmit();
    }
    //When inserting partial payment as euros, verify the inserted amount does not exceed the current price.
    bool InputIsValid()
    {
        if(mode.Current.Format == Format.EURO)
        {
            bool isValid = manager.IsPaymentValid(sum); 
            return isValid;
        }
        return true;
    }
}
#endregion