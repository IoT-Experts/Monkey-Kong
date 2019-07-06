using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using EkumeSavedData.Scores;
using System.Collections.Generic;
using EkumeSavedData;
public class ScoreIncreaser_OnCollision : MonoBehaviour
{
    public int indexOfScoreselected;
    public float valueToIncrease;
    public bool canEnterOnlyOneTime;
    public bool destroyWhenItIsObtained;
    public float delayToDestroy;

    int multiplyBy = 1;
    bool isDuplicable;

    void Start()
    {
        isDuplicable = Player.playerPowers.ScoresToDuplicate.Contains(indexOfScoreselected);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || (other.collider.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (PlayerPowers.ScoreDuplicatorActivated && isDuplicable)
                multiplyBy = 2;
            else
                multiplyBy = 1;

            Score.SetScoreOfLevel(indexOfScoreselected, Score.GetScoreOfLevel(indexOfScoreselected) + (valueToIncrease * multiplyBy));

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreselected].accumulative)
            {
                if (!ScoreTypesManager.instance.ScoresData[indexOfScoreselected].accumulateOnlyIfWin)
                    Accumulated.SetAccumulated(indexOfScoreselected, Accumulated.GetAccumulated(indexOfScoreselected) + (valueToIncrease * multiplyBy));
            }

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreselected].saveBestByLevel
                && !ScoreTypesManager.instance.ScoresData[indexOfScoreselected].saveBestOnlyIfWin)
            {
                if (Score.GetScoreOfLevel(indexOfScoreselected) > Score.GetBestScoreOfLevel(indexOfScoreselected, Levels.GetLevelIdentificationOfCurrentScene()))
                    Score.SetBestScoreOfLevel(indexOfScoreselected, Levels.GetLevelIdentificationOfCurrentScene(), Score.GetScoreOfLevel(indexOfScoreselected));
            }

            ShowScore.refreshScores = true;

            if (destroyWhenItIsObtained)
                Destroy(gameObject, delayToDestroy);

            if (canEnterOnlyOneTime)
                Destroy(this);
        }
    }
}
