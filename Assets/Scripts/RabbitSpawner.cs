using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RabbitSpawner : MonoBehaviour
{
    public Slider rabbitSlider;
    public TMP_Text rabbitText;

    void Start()
    {
        int savedCount = PlayerPrefs.GetInt("PreyCountSlider", 10);
        rabbitSlider.value = savedCount;
        rabbitText.text = savedCount.ToString();

        rabbitSlider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
    }

    public void OnSliderChanged()
    {
        int count = Mathf.RoundToInt(rabbitSlider.value);
        rabbitText.text = count.ToString();

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("PreyCountSlider", count);
        PlayerPrefs.Save();
    }
}
