using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsBerryManager : MonoBehaviour
{
    public Slider berrySlider;
    public TMP_Text berryText;

    void Start()
    {
        int savedCount = PlayerPrefs.GetInt("BerryCountSlider", 10);
        berrySlider.value = savedCount;
        berryText.text = savedCount.ToString();

        berrySlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
    }

    public void OnSliderChanged()
    {
        int count = Mathf.RoundToInt(berrySlider.value);
        berryText.text = count.ToString();

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("BerryCountSlider", count);
        PlayerPrefs.Save();
    }
}

