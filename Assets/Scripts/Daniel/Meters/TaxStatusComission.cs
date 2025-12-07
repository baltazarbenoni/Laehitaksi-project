using UnityEngine;
using TMPro;
using System;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class TaxStatusComission : MonoBehaviour
{
    TMP_Text textBox;
    [SerializeField] GameObject priceManager;
    PriceUpdate priceInstace;
    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        GetRefs();
        Actions.NewTax += UpdateTax;
        Actions.BackToBase += UpdateTax;
        textBox.text = "";
    }
    void OnDisable()
    {
        Actions.NewTax -= UpdateTax;
        Actions.BackToBase -= UpdateTax;
    }
    void GetRefs()
    {
        if (priceManager != null)
        {
            priceInstace = priceManager.GetComponent<PriceUpdate>();
        }
        else
        {
            Debug.LogWarning("Assign references to " + gameObject.name);
        }
    }
    void OnEnable()
    {
        //UpdateTax();
    }
    void UpdateTax(Taxes.Tax tax)
    {
        Debug.Log("Updated tax name to " + tax.Name);
        textBox.text = tax.Name; 
    }
    void UpdateTax()
    {
        try
        {
            textBox.text = priceInstace.CurrentTax.Name;
            Debug.Log("Updated tax name to " + textBox.text);
        }
        catch (Exception e)
        {
            textBox.text = "Unable to fetch text";
            Debug.LogWarning("Couldn't get tax name : " + e.Message);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
