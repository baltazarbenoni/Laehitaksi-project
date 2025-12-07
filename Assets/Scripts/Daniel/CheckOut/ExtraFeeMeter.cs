using UnityEngine;
using TMPro;
using Taxes;

public class ExtraFeeMeter : MonoBehaviour
{
    TMP_Text textBox;
    float currentPrice;
    readonly string euro = "€";
    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        Actions.AddFee += UpdateMeter;
        Actions.RemoveExtras += RemoveFees;
        textBox.text = "";
    }
    void UpdateMeter(ExtraFee fee)
    {
        currentPrice += fee.Price;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }
    void RemoveFees()
    {
        Debug.Log("Cleared extra fees!");
        Debug.Log("Restarted extra fees : " + textBox.text);
        currentPrice = 0;
        textBox.text = "";
    }
}
