using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Payments;
//C 2025 Daniel Snapir alias Baltazar Benoni
public class PaymentMethodButton : MonoBehaviour
{
    Button button;
    [SerializeField] GameObject textObj;
    TMP_Text textComp;
    [SerializeField] internal Method paymentMethod;
    bool isSecondary;

    void OnEnable()
    {
        Initialize();
    }
    void Initialize()
    {

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
            textComp.text = EnumNames.PaymentName(paymentMethod);
        }
    }
    //This method is used only on secondary payment selection canvas --> make 'isSecondary' true.
    public void SetData(Method method)
    {
        this.paymentMethod = method;
        isSecondary = true;
        textComp.text = EnumNames.PaymentName(paymentMethod);
    }
    void OnClick()
    {
        Actions.PaymentSelection?.Invoke(paymentMethod, isSecondary);
    }
}
