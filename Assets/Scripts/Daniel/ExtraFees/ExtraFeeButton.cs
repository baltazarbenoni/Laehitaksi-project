using UnityEngine;
using UnityEngine.UI;
using TMPro;
//C 2025 Daniel Snapir alias Baltazar Benoni

namespace Taxes
{
    public class ExtraFeeButton : MonoBehaviour 
    {
        Button button;
        [Header("Text component")]
        [SerializeField] GameObject textObj;
        TMP_Text textComp;
        ExtraFee fee;
        bool initd;

        void Awake()
        {
            Initialize();
        }
        void OnEnable()
        {
            if(!initd)
            {
                InitializeData();
            }
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
            }
        }
        void InitializeData()
        {
            if(fee != null)
            {
                textComp.text = fee.Name;
                initd = true;
            }
            else
            {
                Debug.Log("Couldn't fetch data");
            }
        }
        void Click()
        {
            Actions.AddFee?.Invoke(fee);
            Actions.NavigationButton(Navigation.ButtonType.ExtraFee);
        }
        public void SetData(ExtraFee fee)
        {
            this.fee = fee;
            textComp.text = fee.Name;
        }
    }
}