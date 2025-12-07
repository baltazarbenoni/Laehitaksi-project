using UnityEngine;
using UnityEngine.UI;
using TMPro;

//C 2025 Daniel Snapir alias Baltazar Benoni

public class TaxButton : MonoBehaviour
{
    Button button;
    [Header("Tax amount (per km)")]
    //[SerializeField] int tax;
    //public int Tax { get { return tax; } }

    [Header("Tax id and info")]
    [SerializeField] string taxName;
    [SerializeField] public string Name { get { return taxName; } }
    int taxIndex;
    public int Index { get { return taxIndex; } }
    int category;
    public int Category { get { return category; } }
    int fullCategory;
    public int FullCategory { get { return fullCategory; } }
    [Header("Tax button component")]
    [SerializeField] GameObject textObj;
    TMP_Text textComp;
    bool initd;

    void Start()
    {
        if (!initd)
        {
            Initialize();
        }
    }
    public void SetData(int category, int fullCat, string name, int num)
    {
        this.category = category;
        Debug.Log("Category for button is " + category);
        this.fullCategory = fullCat;
        Debug.Log("Full category for button is " + fullCat);
        this.taxName = name;
        Debug.Log("Name for button is " + name);
        this.taxIndex = num;
        Debug.Log("Index for button is " + num);
        Initialize();
    }
    //Check and initialize variables.
    void Initialize()
    {
        //Initialize Button.
        button = GetComponent<Button>();
        button.onClick.AddListener(Click);

        //Check if text component is set and establish references.
        if(textObj == null)
        {
            Debug.LogWarning("ASSIGN TEXT-COMPONENT GAMEOBJECT");
        }
        else
        {
            textComp = textObj.GetComponent<TMP_Text>();
            textComp.text = taxName;
        }
        //Check if variables set.
        if(taxName == "")
        {
            Debug.LogWarning("ASSIGN VARIABLES TO SCRIPT");
        }
        initd = true;
    }
    //Invoke tax change.
    void Click()
    {
        Actions.TaxSelect(this);
    }
}
