using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Navigation 
{

    public class TaxNextBack : MonoBehaviour
    {
        [Header("Action")]
        [SerializeField] Type buttonType;

        [Header("Tax button component")]
        [SerializeField] GameObject textObj;
        TMP_Text textComp;

        [Header("References")]
        [SerializeField] GameObject nextCanvas;
        [SerializeField] GameObject thisCanvas;
        Button button;

        public enum Type
        {
            Back,
            Next
        }
        void Awake()
        {
            Initialize();
        }
        void Initialize()
        {
            //Initialize button.
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);

            //Check if text component is set and establish references.
            if (textObj == null)
            {
                Debug.LogWarning("ASSIGN TEXT-COMPONENT GAMEOBJECT");
            }
            //Set text component and its text.
            else
            {
                textComp = textObj.GetComponent<TMP_Text>();
                textComp.text = (int)buttonType == 0 ? "Peruuta" : "Seuraava";
            }
        }
        void OnClick()
        {
            if (this.buttonType == Type.Back && thisCanvas != null)
            {
                Actions.NavigationButton?.Invoke(ButtonType.Back);
                thisCanvas.SetActive(false);
            }
            else if (nextCanvas != null && thisCanvas != null)
            {
                nextCanvas.SetActive(true);
                thisCanvas.SetActive(false);
            }
            else
            {
                Debug.LogWarning("'nextCanvas' or 'thisCanvas' is null, cannot go next");
            }
        }

    }
}