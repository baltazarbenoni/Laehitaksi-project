using UnityEngine;
using Taxes;
using Navigation;
using Input;

public class MaxPrice : MonoBehaviour
{
    [SerializeField] GameObject priceManager;
    PriceUpdate priceUpdate;
    void Awake()
    {
        Actions.Input += CheckInput;
        GetReferences();
    }
    void OnDestroy()
    {
        Actions.Input -= CheckInput;
    }
    void GetReferences()
    {
        if(priceManager == null)
        {
            Debug.LogWarning("Assign price manager to " + gameObject.name);
        }
        priceUpdate = priceManager.GetComponent<PriceUpdate>();
    }
    void CheckInput(InputMode mode, int num)
    {
        if (mode.inputType == InputType.MaxPrice)
        {
            AddMaxPrice(num);
        }
    }
    void AddMaxPrice(int num)
    {
        Tax tax = priceUpdate.CurrentTax;
        Debug.Log("Added max price " + num);
        tax.AddMaxPrice(num);
    }
}
