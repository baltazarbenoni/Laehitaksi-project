using UnityEngine;
using Instantiation;
using Navigation;
using Input;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class InstantiateNumberButtons : MonoBehaviour
{
    bool buttonsInstantiated;
    [SerializeField] GameObject numberButton;
    NumberButtonInstantiation instantiator;
    [Header("Assign size and anchors for the grid")]
    [SerializeField] float xAnchor = -600;
    [SerializeField] float yAnchor = 50;
    [SerializeField] float gridWidth = 300f;
    [SerializeField] float gridHeight = 175f;
    GameObject formatChangeButton;

    void Awake()
    {
        instantiator = new NumberButtonInstantiation(new XY(xAnchor, yAnchor), new XY(gridWidth, gridHeight));
        Actions.SetNumberInputMode += InstantiateFormatChangeButton;
    }
    void Start()
    {
        if (!buttonsInstantiated)
        {
            CreateButtons();
        }
    }

    //Create buttons for numbers from 1 to 9, 'C' for erase and 'OK' to enter the numbers. 
    //If 'input mode' so necessitates, instantiate also button to change format.
    void CreateButtons()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject obj = InstantiateButton(i);
            SetData(obj, i);
        }
        buttonsInstantiated = true;
    }
    #region Instantiation
    //Instantiate one button.
    GameObject InstantiateButton(int buttonNum)
    {
        GameObject instButton = Instantiate(numberButton);
        instButton.transform.localScale = new Vector3(1, 1, 1);
        instButton.transform.SetParent(this.transform, false);
        instButton.name = $"NumberButton{buttonNum}";

        RectTransform buttonPos = instButton.GetComponent<RectTransform>();
        buttonPos.anchoredPosition = GetButtonPosition(buttonNum);
        Debug.Log("instantiated button " + buttonNum + " at " + buttonPos.anchoredPosition);
        return instButton;
    }
    void SetData(GameObject obj, int index)
    {
        NumberButton buttonScript = obj.GetComponent<NumberButton>();
        if (buttonScript != null)
        {
            buttonScript.SetNumber(index + 1);
        }
    }
    void SetData(GameObject obj, int index, InputType type)
    {
        NumberButton buttonScript = obj.GetComponent<NumberButton>();
        if (buttonScript != null)
        {
            buttonScript.SetNumber(index + 1, type);
        }
    }
    #endregion
    #region New function
    Vector2 GetButtonPosition(int buttonNumber)
    {
        Vector2 pos = instantiator.GetButtonPosition(buttonNumber);
        return pos;
    }
    void InstantiateFormatChangeButton(int i)
    {
        Debug.Log("Instantiating format change button");
        InputType type = (InputType)i;
        if(InputMode.HasMultipleFormats(type) == false)
        {
            ToggleFormatChangeActivation(false);
        }
        else if(formatChangeButton != null)
        {
            ToggleFormatChangeActivation(true);
            SetData(formatChangeButton, 12, type);
        }
        else
        {
            GameObject obj = InstantiateButton(12);
            formatChangeButton = obj;
            SetData(obj, 12, type);
        }
    }
    void ToggleFormatChangeActivation(bool val)
    {
        if(formatChangeButton != null)
        {
            formatChangeButton.SetActive(val);
        }
    }
    #endregion
}
