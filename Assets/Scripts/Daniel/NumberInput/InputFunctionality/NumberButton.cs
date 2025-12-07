using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Navigation;
//C 2025 Daniel Snapir alias Baltazar Benoni
public class NumberButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Select the type of the numberButton")]
    [SerializeField] NumType type;
    int numericalValue;
    Button button;
    [SerializeField] TMP_Text textObj;
    bool holding;
    [SerializeField] float holdLimit = 1f;
    float timer;
    string inputModeId;
    bool adjusted;
    public enum NumType
    {
        _0 = 0,
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
        _5 = 5,
        _6 = 6,
        _7 = 7,
        _8 = 8,
        _9 = 9,
        C = 10,
        OK = 11,
        Change = 13,
    }
    //Constructor for class in case instantiation is needed.
    public NumberButton(NumType type)
    {
        this.type = type;
        this.numericalValue = (int)type;
    }
    #region General

    void Awake()
    {
        //Initialize button.
        button = GetComponent<Button>();

        //Check if text component is set and establish references.
        if (textObj == null)
        {
            Debug.LogWarning("ASSIGN TEXT-COMPONENT GAMEOBJECT");
        }
    }
    void Start()
    {
        if (textObj != null)
        {
            textObj.text = GetText(); 
        }
    }
    void Update()
    {
        if(numericalValue == 10)
        {
            ManageButton();
        }
    }
    internal void SetNumber(int num)
    {
        if (num == 12)
        {
            num = 0;
        }
        this.numericalValue = num;
        this.type = (NumType)num;
        AdjustSize();
    }
    internal void SetNumber(int num, InputType inputType)
    {
        this.numericalValue = num;
        this.type = (NumType)num;
        SetId(inputType);
        AdjustSize();
    }
    void SetId(InputType type)
    {
        if(textObj != null)
        {
            string id = Input.InputMode.GetId(type);
            inputModeId = id;
        }
    }
    void AdjustSize()
    {
        if(adjusted)
        {
            return;
        }
        else if(type == NumType.C)
        {
            Debug.Log("Adjusting button size!");
            HorizontalLayoutGroup layout = GetComponent<HorizontalLayoutGroup>();
            layout.padding.left += 100;
            layout.padding.right += 100;
            AdjustPos();
        }
        else if(type == NumType.Change)
        {
            Debug.Log("Adjusting button position!");
            AdjustPos();
        }
    }
    void AdjustPos()
    {
        RectTransform transform = GetComponent<RectTransform>();
        transform.anchoredPosition += new Vector2(-75, 0);
        adjusted = true;
    }
    string GetText()
    {
        if (numericalValue < 10)
        {
            return numericalValue.ToString();
        }
        else if (numericalValue < 11)
        {
            return "C";
        }
        else if(numericalValue < 12)
        {
            return "OK";
        }
        else
        {
            return inputModeId;
        }
    }
    #endregion
    #region Hold
    void ManageButton()
    {
        if(!holding)
        {
            return;
        }
        timer += Time.deltaTime;
        if(timer > holdLimit)
        {
            HoldClick();
            ToggleHoldStatus(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData) => ToggleHoldStatus(true); 
    public void OnPointerUp(PointerEventData eventData)
    {
        Click();
        ToggleHoldStatus(false);
    }
    void ToggleHoldStatus(bool clickDown)
    {
        holding = clickDown;
        if(!holding)
        {
            timer = 0;
        }
    }
    #endregion
    #region Click
    void Click()
    {
        Actions.NumberInput?.Invoke(numericalValue);
        Debug.Log("Pressed number " + numericalValue);
    }
    void HoldClick()
    {
        if(this.numericalValue == 10)
        {
            Actions.Input(new Input.InputMode(InputType.None), 10);
        }
    }
}
#endregion

