using UnityEngine;
using UnityEngine.UI;
//C 2025 Daniel Snapir alias Baltazar Benoni

public class SpeedSlider : MonoBehaviour
{
    Slider slider;
    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate {ValueChange(); });
    }
    void ValueChange()
    {
        Actions.SpeedChange(slider.value);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
