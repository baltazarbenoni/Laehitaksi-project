using UnityEngine;
using TMPro;
using Taxes;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class ExtraFeeText : MonoBehaviour
{
    TMP_Text textObj;
    int counter;
    void Awake()
    {
        textObj = GetComponent<TMP_Text>();
        Actions.AddFee += GetText;
        Actions.RemoveExtras += Remove;
    }
    void GetText(ExtraFee fee)
    {
        string addition = counter == 0 ? "" : ", ";
        textObj.text += addition + fee.Name;
        counter++;
    }
    void Remove()
    {
        Debug.Log("Cleared extra fees!");
        counter = 0;
        textObj.text = "";
    }
}
