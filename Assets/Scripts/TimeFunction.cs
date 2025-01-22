using System;
using UnityEngine;

public class TimeFunction : MonoBehaviour
{
    public float timePassed = 0.0f;
    static String FormatterHelper(int lowerTime, int higherTime, int div)
    {
        lowerTime %= div;
        if (higherTime > 0)
        {
            return lowerTime >= 10 ? lowerTime.ToString() : "0" + lowerTime.ToString();

        }
        else
        {
            return lowerTime.ToString();
        }
    }
    public static String StopwatchFormatter(float inputSeconds)
    {
        int seconds = (int)Math.Floor(inputSeconds);
        int minutes = seconds / 60;
        int hours = minutes / 60;
        int days = hours / 24;
        String result = "";
        if (days > 0)
        {
            result += days.ToString() + ":";
        }
        if (hours > 0)
        {
            result += FormatterHelper(hours, days, 24) + ":";
        }
        if (minutes > 0)
        {
            result += FormatterHelper(minutes, hours, 60) + ":";
            // Debug.Log(seconds + "meow" + minutes);
        }
        result += TimeFunction.FormatterHelper(seconds, minutes, 60);
        return result;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
    }
}
