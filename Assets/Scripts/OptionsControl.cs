using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsControl : MonoBehaviour
{
    public TMP_Text optionCounter;
    public Slider slider;

    void Update()
    {
        optionCounter.text = $"{slider.value}";

        float count = slider.value;
    }
}
