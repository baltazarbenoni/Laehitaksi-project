using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Navigation;
using Input;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class NumberInputField : MonoBehaviour
{
    [SerializeField] InputMode mode;
    [SerializeField] GameObject statusManagerObj;
    internal PriceStatusManager statusManager;
    TMP_Text textObj;
    Button button;
    string initalTxt;
    int sum;
    public int Sum { get { return sum; } }
    INumberInput inputHandler;

    void Awake()
    {
        textObj = GetComponent<TMP_Text>();
        button = GetComponent<Button>();
        Actions.NumberInput += OnNumberInput;
        Actions.SetNumberInputMode += SetMode;
        statusManager = References.GetRef(gameObject, statusManagerObj, statusManager);
        sum = 0;
    }
    void OnDestroy()
    {
        Actions.NumberInput -= OnNumberInput;
        Actions.SetNumberInputMode -= SetMode;
    }
    void OnDisable()
    {
        sum = 0;
        textObj.text = initalTxt;
    }
    void SetMode(int i)
    {
        InputType type = (InputType)i;
        mode = new InputMode(type);
        inputHandler = NumberInput.GetHandler(type, statusManager);
        textObj.text = inputHandler.GetText();
    }
    void OnNumberInput(int num)
    {
        if(inputHandler == null)
        {
            return;
        }
        if (num < 10)
        {
            inputHandler.AddNumber(num);
        }
        else if (num < 11)
        {
            inputHandler.EraseNumber();
        }
        else if(num > 12)
        {
            inputHandler.ChangeFormat();
        }
        else
        {
            inputHandler.Submit();
        }

        inputHandler.UpdateMeter();
        textObj.text = inputHandler.GetText();
    }
}
