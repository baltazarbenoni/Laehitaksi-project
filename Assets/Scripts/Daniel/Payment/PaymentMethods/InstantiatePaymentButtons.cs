using Instantiation;
using UnityEngine;
using Navigation;
using Payments;
using System.Collections.Generic;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class InstantiatePaymentButtons : MonoBehaviour
{
    [SerializeField] GameObject paymentButton;
    [SerializeField] float yAnchor = 0f;
    [SerializeField] float spacingY = 50f;
    FixedSizeInstantiation instantiator;
    Method[] dividedMethods = new Method[] { Method.Suorakorvaus, Method.Billing, Method.Taxcard, Method.Sote, Method.Cash, Method.DebitCredit, Method.BypassCash };
    Method[] partialMethods = new Method[] { Method.Suorakorvaus, Method.Billing, Method.Taxcard, Method.Sote, Method.Cash, Method.DebitCredit };
    Method[] paymentMethods;
    InputType previous = InputType.None; 
    List<GameObject> childList = new();
    void Awake()
    {
        InitInstantiator();
        Actions.SecondaryPayment += UpdateInstantiation;
    }
    void OnDestroy()
    {
        Actions.SecondaryPayment -= UpdateInstantiation;
    }
    void InitInstantiator()
    {
        instantiator = new FixedSizeInstantiation(new XY(0, yAnchor), new XY(0,0), new XY(0, spacingY), 2);
    }
    void UpdateInstantiation(InputType type)
    {
        if(type == previous)
        {
            return;
        }
        else if(previous == InputType.None)
        {
            InstantiateButtons(type);
        }
        else
        {
            ClearChildren();
            InstantiateButtons(type);
        }
    }
    void InstantiateButtons(InputType type)
    {
        SetMode(type);
        for(int i = 0; i < paymentMethods.Length; i++)
        {
            CreateButton(i);
        }
    }
    void CreateButton(int buttonNum)
    {
        GameObject instButton = Instantiate(paymentButton);
        instButton.transform.localScale = new Vector3(1, 1, 1);
        instButton.transform.SetParent(this.transform, false);
        instButton.name = $"PaymentMethod{buttonNum}";
        childList.Add(instButton);

        //Assign data to button.
        PaymentMethodButton buttonScript = instButton.GetComponent<PaymentMethodButton>();
        buttonScript.SetData(paymentMethods[buttonNum]);

        RectTransform trans = instButton.GetComponent<RectTransform>();
        trans.anchoredPosition = instantiator.GetButtonPosition(buttonNum);
        Debug.Log("instantiated button " + buttonNum + " at " + trans.anchoredPosition);
    }
    void ClearChildren()
    {
        foreach(var child in childList)
        {
            Destroy(child);
        }
        childList.Clear();
    }
    void SetMode(InputType type)
    {
        previous = type;
        if(type == InputType.Partial)
        {
            paymentMethods = partialMethods;
        }
        else if(type == InputType.Divided)
        {
            paymentMethods = dividedMethods;
        }
    }
}
