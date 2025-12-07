using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class RemoveExtraFees : MonoBehaviour
{
    Button button;
    [Header("Text component")]
    [SerializeField] GameObject textObj;
    TMP_Text textComp;
    bool initd;

    void Awake()
    {
        Initialize();
    }
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
            textComp.text = "Poista";
        }
    }
    void Click()
    {
        Actions.RemoveExtras?.Invoke();
        Actions.NavigationButton(Navigation.ButtonType.ExtraFee);
    }
}
