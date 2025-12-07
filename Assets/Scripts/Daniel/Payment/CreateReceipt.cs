using UnityEngine;
using TMPro;

public class CreateReceipt : MonoBehaviour
{
    TMP_Text textObj;
    string receipt;
    void Awake()
    {
        Actions.CreateReceipt += WriteReceipt;
        Actions.PaymentAnimationFinished += AnimationFinished;
        textObj = GetComponent<TMP_Text>(); 
    }
    void OnDisable()
    {
        Actions.CreateReceipt -= WriteReceipt;
        Actions.PaymentAnimationFinished -= AnimationFinished;
    }
    void WriteReceipt(string message)
    {
        receipt = message;
    }
    void AnimationFinished()
    {
        textObj.text = receipt;
    }
}
