using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoxSpawner : MonoBehaviour
{
    public Slider foxSlider;
    public TMP_Text foxText;

    void Start()
    {
        int savedCount = PlayerPrefs.GetInt("PredatorCountSlider", 10);
        foxSlider.value = savedCount;
        foxText.text = savedCount.ToString();

        foxSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
    }

    public void OnSliderChanged()
    {
        int count = Mathf.RoundToInt(foxSlider.value);
        foxText.text = count.ToString();

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("PredatorCountSlider", count);
        PlayerPrefs.Save();
    }
}