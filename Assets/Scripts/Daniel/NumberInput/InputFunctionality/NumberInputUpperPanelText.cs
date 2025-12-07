using UnityEngine;
using TMPro;
using Navigation;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class NumberInputUpperPanelText : MonoBehaviour
{
    TMP_Text textObj;
    void Awake()
    {
        textObj = GetComponent<TMP_Text>();
        Actions.SetNumberInputMode += UpdateText;
    }
    void UpdateText(int num)
    {
        InputType mode = (InputType)num;
        string newText = GetText(mode);
        textObj.text = newText;
    }
    string GetText(InputType mode)
    {
        return mode switch
        {
            InputType.Addition => "Syötä hinta lisälle",
            InputType.Customer => "Syötä asiakasnumero 111146",
            InputType.MaxPrice => "Syötä maksimihinta",
            InputType.MaxTime => "Syötä odotusaika",
            InputType.Partial => "Syötä osittaismaksu euroina",
            InputType.Divided => "Jaettu maksu: Henkilömäärä",
            _ => ""
        };
    }
}
