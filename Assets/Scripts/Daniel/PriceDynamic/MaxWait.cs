using UnityEngine;
using Navigation;
using Taxes;
using Input;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class MaxWait : MonoBehaviour
{
    [SerializeField] GameObject priceManager;
    PriceUpdate priceManagerInstance;
    void Awake()
    {
        Actions.Input += CheckInput;
        priceManagerInstance = References.GetRef(gameObject, priceManager, priceManagerInstance);
    }
    void OnDestroy()
    {
        Actions.Input -= CheckInput;
    }
    void CheckInput(InputMode mode, int num)
    {
        if(mode.inputType == InputType.MaxTime)
        {
            AddMaxWait(mode.Current, num);
        }
    }
    void AddMaxWait(InputFormat format, int num)
    {
        Tax tax = priceManagerInstance.CurrentTax;
        if(tax.WaitTax == 0)
        {
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
        Debug.Log($"max wait: added max wait to current tax {tax}, {seconds}");
        tax.AddMaxWaitTime(seconds);
    }
    static void AddMaxWaitPrice(Tax tax, int price)
    {
        Debug.Log($"max wait: added max price to current tax {tax}, {price}");
        //To avoid decimals, system uses integers multiplied by 100.
        tax.AddMaxWaitPrice(price * 100);
    }
}
