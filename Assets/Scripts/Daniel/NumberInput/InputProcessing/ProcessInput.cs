using UnityEngine;
using Taxes;
using Input;
using Payments;
using Navigation;
//C 2025 Daniel Snapir alias Baltazar Benoni


public class ProcessInput : MonoBehaviour
{
    [SerializeField] GameObject priceStatusObj;
    PriceStatusManager priceStatus;
    internal PriceStatusManager StatusManager;
    [SerializeField] GameObject paymentManagerObj;
    PaymentManager paymentManager;
    internal PaymentManager PaymentManager => paymentManager;
    public Tax TaxInUse { get { return priceStatus.CurrentTax; } }
    void Awake()
    {
        GetRefs();
        InitializeActions();
    }
    void OnDestroy()
    {
        Unsubscribe();
    }
    void GetRefs()
    {
        priceStatus = References.GetRef(gameObject, priceStatusObj, priceStatus);
        paymentManager = References.GetRef(gameObject, paymentManagerObj, paymentManager);
    }
    void InitializeActions()
    {
        Actions.Input += CheckInput;
    }
    void Unsubscribe()
    {
        Actions.Input -= CheckInput;
    }
    void CheckInput(InputMode mode, int num)
    {
        Debug.Log($"Input type is {mode.inputType}");
        IHandler handler = GetHandler(mode.inputType);
        handler.Process(num, mode);
    }
    IHandler GetHandler(InputType type)
    {
        return type switch
        {
            InputType.MaxPrice => new MaxPriceProcessor(this),
            InputType.MaxTime => new MaxWaitProcessor(this),
            InputType.Customer => new CustomerInputProcessor(this),
            InputType.Partial => new PaymentProcessor(this),
            InputType.Divided => new PaymentProcessor(this),
            _ => new InputHandler(this) 
        };
    }
}

