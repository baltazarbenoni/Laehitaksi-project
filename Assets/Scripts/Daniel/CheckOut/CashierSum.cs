using UnityEngine;
using TMPro;
using Taxes;

public class CashierSum : MonoBehaviour
{
    TMP_Text textBox;
    float currentPrice;
    readonly string euro = "€";
    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        Actions.ForcePrice += ForceMeterValue;
        //Actions.AddFee += AddExtraFee;
        //Actions.RemoveExtras += RemoveExtras;
        textBox.text = "00,00" + euro;
    }
    void Restart()
    {
        ForceMeterValue(0f);
    }
    void ForceMeterValue(float price)
    {
        Debug.Log("Forced new price " + price);
        currentPrice = price;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }
    /*void AddExtraFee(ExtraFee fee)
    {
        extraFees += fee.Price;
        currentPrice += fee.Price;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }
    void RemoveExtras()
    {
        currentPrice -= extraFees;
        Debug.Log("Removing extras " + extraFees + "current price is " + currentPrice);
        extraFees = 0;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }
    void ForceMeter(float price)
    {
        int roundedPrice = (int)(price / 10);
        float newPrice = roundedPrice * 10;
        currentPrice += newPrice;
        string sumString = Conversion.FloatToString(currentPrice);
        textBox.text = sumString + euro;
    }*/
}
