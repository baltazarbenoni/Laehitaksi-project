using UnityEngine;
using TMPro;
//C 2025 Daniel Snapir alias Baltazar Benoni
public class SpeedMeter : MonoBehaviour
{
    TMP_Text textBox;
    void Awake()
    {
        textBox = GetComponent<TMP_Text>();
        Actions.SpeedChange += UpdateText;
        textBox.text = "0 km/h";
    }
    void OnDestroy()
    {
        Actions.SpeedChange -= UpdateText;
    }
    void Start()
    {
        
    }
    void UpdateText(float speed)
    {
        textBox.text = speed.ToString() + " km/h";
    }

    void Update()
    {
        
    }
}
