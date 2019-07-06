using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using EkumeSavedData.Scores;
using System.Collections.Generic;
using EkumeSavedData;

public class ScoreIncreaser_OnTrigger : MonoBehaviour
{
    public int indexOfScoreSelected;
    public float valueToIncrease;
    public bool canEnterOnlyOneTime;
    public bool destroyWhenItIsObtained;
    public float delayToDestroy;

    int multiplyBy = 1;
    bool isDuplicable;

    void Start ()
    {
        isDuplicable = Player.playerPowers.ScoresToDuplicate.Contains(indexOfScoreSelected);   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (PlayerPowers.ScoreDuplicatorActivated && isDuplicable)
                multiplyBy = 2;
            else
                multiplyBy = 1;

            Score.SetScoreOfLevel(indexOfScoreSelected, Score.GetScoreOfLevel(indexOfScoreSelected) + (valueToIncrease * multiplyBy));

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreSelected].accumulative)
            {
                if (!ScoreTypesManager.instance.ScoresData[indexOfScoreSelected].accumulateOnlyIfWin)
                    Accumulated.SetAccumulated(indexOfScoreSelected, Accumulated.GetAccumulated(indexOfScoreSelected) + (valueToIncrease * multiplyBy));
            }

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreSelected].saveBestByLevel
                && !ScoreTypesManager.instance.ScoresData[indexOfScoreSelected].saveBestOnlyIfWin)
            {
                if (Score.GetScoreOfLevel(indexOfScoreSelected) > Score.GetBestScoreOfLevel(indexOfScoreSelected, Levels.GetLevelIdentificationOfCurrentScene()))
                    Score.SetBestScoreOfLevel(indexOfScoreSelected, Levels.GetLevelIdentificationOfCurrentScene(), Score.GetScoreOfLevel(indexOfScoreSelected));
            }

            ShowScore.refreshScores = true;

            if (destroyWhenItIsObtained)
                Destroy(gameObject, delayToDestroy);

            if (canEnterOnlyOneTime)
                Destroy(this);
        }
    }
}

