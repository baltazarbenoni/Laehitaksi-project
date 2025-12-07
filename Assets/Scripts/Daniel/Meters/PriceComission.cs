using UnityEngine;
using TMPro;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class PriceComission : MonoBehaviour
{
    TMP_Text textBox;
    float currentPrice;
    readonly string euro = "€";
    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        Actions.UpdatePrice += UpdateMeter;
        Actions.PriceUpdateForceSum += ForceMeterValue;
        Actions.ForcePrice += ForceMeterValue;
        textBox.text = "00,00" + euro;
    }

    void UpdateMeter(float price)
    {
        float difference = price - currentPrice;
        if(difference > 20f || difference < 0)
        {
            ForceMeterValue(price);
            return;
        }
        else if (difference >= 10f)
        {
            Debug.Log("Updating meter: " + difference + ", " + price);
            currentPrice += 10f;
            string sumString = Conversion.FloatToString(currentPrice);
            textBox.text = sumString + euro;
        }
    }
    void ForceMeterValue(float price)
    {
        Debug.Log("Forced new price " + price);
        currentPrice = price;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }
}
