using UnityEngine;
using UnityEngine.UI;
using TMPro;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Navigation 
{
    public class NavigButton : MonoBehaviour
    {
        [Header("Action")]
        [SerializeField] internal ButtonType type;

        [Header("Tax button component")]
        [SerializeField] GameObject textObj;
        TMP_Text textComp;

        [Header("References")]
        [Tooltip("Assign correct references for navigation between menus.")]
        Button button;
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
                textComp.text = EnumNames.GetText(type);
            }
        }
        public void UpdateText()
        {
            if (textComp != null)
            {
                textComp.text = EnumNames.GetText(type);
            }
        }
        //Get the canvas to activate on click.

        void OnClick()
        {
            Actions.NavigationButton(type);
        }
    }
}