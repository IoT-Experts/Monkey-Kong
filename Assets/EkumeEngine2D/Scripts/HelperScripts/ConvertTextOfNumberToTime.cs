using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConvertTextOfNumberToTime : MonoBehaviour
{

    enum TimeFormat { showMinutesAndSeconds, showOnlySeconds, ShowMinutesSecondsAndMilliseconds }
    [SerializeField] TimeFormat timeFormat;
    [Header("The text should be a number. Will be refreshed when start.")]
    [SerializeField] Text timeCounterText;

    float timeOfCounter;

    // Use this for initialization
    void Start ()
    {
        if (!float.TryParse(timeCounterText.text, out timeOfCounter))
        {
            Debug.LogError("String cannot be parsed. The text can have only numbers.");
        }

        StartCoroutine(WaitToRefresh());
    }

    IEnumerator WaitToRefresh()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.1f);
            RefreshText();
        }
    }

    void RefreshText()
    {
        int minutes = (int)timeOfCounter / 60; //Divide the secondsToLose by sixty to get the minutes.
        int seconds = (int)timeOfCounter % 60; //Use the euclidean division for the seconds.

        if (timeFormat == TimeFormat.ShowMinutesSecondsAndMilliseconds)
        {
            int milliseconds = (int)(timeOfCounter * 100) % 100;
            timeCounterText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        else if (timeFormat == TimeFormat.showMinutesAndSeconds)
            timeCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        else if (timeFormat == TimeFormat.showOnlySeconds)
            timeCounterText.text = string.Format("{00}", seconds + (minutes * 60));
    }

}
