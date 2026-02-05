using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Button speed1xButton;
    public Button speed2xButton;
    public Button speed3xButton;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void Speed1x()
    {
        Time.timeScale = 1f;
    }

    public void Speed2x()
    {
        Time.timeScale = 2f;
    }

    public void Speed3x()
    {
        Time.timeScale = 3f;
    }
}
