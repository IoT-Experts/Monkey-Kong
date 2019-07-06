using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EkumeEnumerations;
using EkumeLists;
using EkumeSavedData.Scores;
using EkumeSavedData;

public class GameTimeUpScore : MonoBehaviour
{
    [SerializeField] PlayerStatesEnum eventToStartTimer;
    [SerializeField] enum TimeFormat { showMinutesAndSeconds, showOnlySeconds, ShowMinutesSecondsAndMilliseconds}
    [SerializeField] TimeFormat timeFormat;
    [SerializeField] PlayerStatesEnum[] whenStop;
    [SerializeField] Text timeCounterText;

    [Header("If dies but continue in save point")]
    [HideWhenTrue("continueWithLastSavedTime")]
    public bool restartTime;
    [HideWhenTrue("restartTime")]
    public bool continueWithLastSavedTime; //Time that was obtained when got the save point.

    [Header("Score options (To store the values)")]
    [SerializeField] ListOfScores ScoreType;
    [SerializeField] bool saveWhenDie;
    [SerializeField] bool saveWhenWin;
    [HideWhenTrue("saveTheShortestTime")]
    [SerializeField] bool saveTheBiggestTime;
    [HideWhenTrue("saveTheBiggestTime")]
    [SerializeField] bool saveTheShortestTime;

    [HideInInspector]
    public float timeOfCounter;

    bool bestScoreSaved;
    [HideInInspector]
    public bool finishTime;

    bool eventToStartIsActivated = false;

    void Start ()
    {
        RefreshTimer();
    }

    void Update ()
    {
        if (!eventToStartIsActivated)
            if (PlayerStates.GetPlayerStateValue(eventToStartTimer))
                eventToStartIsActivated = true;

        if (!finishTime)
        {
            if (eventToStartIsActivated)
            {
                timeOfCounter += Time.deltaTime;
                Score.SetScoreOfLevel(ScoreType.Index, timeOfCounter);
                RefreshTimer();
            }
        }

        if (!bestScoreSaved)
        {
            if (saveWhenDie && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel))
            {
                SaveBestScore();
            }
            else if (saveWhenWin && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerWinLevel))
            {
                SaveBestScore();
            }
        }
    }


    void SaveBestScore()
    {
        bool canSave = false;

        if (saveTheBiggestTime)
            canSave = Score.GetScoreOfLevel(ScoreType.Index) > Score.GetBestScoreOfLevel(ScoreType.Index, Levels.GetLevelIdentificationOfCurrentScene());
        else if (saveTheShortestTime)
            canSave = Score.GetScoreOfLevel(ScoreType.Index) < Score.GetBestScoreOfLevel(ScoreType.Index, Levels.GetLevelIdentificationOfCurrentScene());

        if (canSave)
        {
            Score.SetBestScoreOfLevel(ScoreType.Index, Levels.GetLevelIdentificationOfCurrentScene(), Score.GetScoreOfLevel(ScoreType.Index));
        }

        if (ScoreTypesManager.instance.ScoresData[ScoreType.Index].accumulative)
        {
            Accumulated.SetAccumulated(ScoreType.Index, Accumulated.GetAccumulated(ScoreType.Index) + Score.GetScoreOfLevel(ScoreType.Index));
        }

        bestScoreSaved = true;
    }

    void RefreshTimer ()
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

        if (StopTimerIfEventTrue())
        {
            finishTime = true;
        }
    }

    bool StopTimerIfEventTrue ()
    {
        bool someEventIsTrue = false;

        foreach (PlayerStatesEnum eventToStop in whenStop)
        {
            if (PlayerStates.GetPlayerStateValue(eventToStop))
            {
                someEventIsTrue = true;
            }
        }

        return someEventIsTrue;
    }

}

