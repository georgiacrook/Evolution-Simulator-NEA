using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    Vector3 rotate = Vector3.zero;
    float degrees = 0.6F;

    public TMP_Text timeText;
    public TMP_Text dayText;

    public float realTimeDuration = 600f;
    private float realTimeElapsed = 0f;

    private float startTime = 8f;
    private int dayCount = 1;

    float inGameTime;

    // Update is called once per frame
    void Update()
    {
        rotate.x = degrees * Time.deltaTime;
        sun.transform.Rotate(rotate, Space.World);

        realTimeElapsed += Time.deltaTime; //keeps track of the real time the program has been running for

        if (realTimeElapsed >= realTimeDuration)
        {
            realTimeElapsed -= realTimeDuration; //makes sure 1 day is 10 minutes long
        }

        if (dayCount == 1)
        {
            inGameTime = startTime + (realTimeElapsed / realTimeDuration) * 24f; //makes the start time 8 am
        }
        else
        {
            inGameTime = (realTimeElapsed / realTimeDuration) * 24f; //makes the time go up in time with real time but faster
        }

        if (inGameTime >= 24f) //new day 
        {
            inGameTime = 0f;
            realTimeElapsed = 0f; //resetting timers
            dayCount++;
        }

        DisplayTime(inGameTime);
        DisplayDay();
    }

    void DisplayTime(float inGameTime)
    {
        int hours = Mathf.FloorToInt(inGameTime);
        int minutes = Mathf.FloorToInt((inGameTime - hours) * 60f);

        timeText.text = string.Format("Time: {0:D2}:{1:D2}", hours, minutes);
    }

    void DisplayDay()
    {
        dayText.text = "Day: " + dayCount;
    }
}