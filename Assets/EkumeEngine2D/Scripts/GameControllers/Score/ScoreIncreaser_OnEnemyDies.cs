using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using EkumeSavedData.Scores;
using EkumeSavedData;
using System.Collections.Generic;

[RequireComponent(typeof(Enemy))]
public class ScoreIncreaser_OnEnemyDies : MonoBehaviour
{
    public int indexOfScoreselected;
    public float valueToIncrease;
    Enemy enemyStates;

    int multiplyBy = 1;
    bool enemyDied = false;
    bool isDuplicable;

    void Awake ()
    {
        enemyStates = this.GetComponent<Enemy>();
    }

    void Start()
    {
        isDuplicable = Player.playerPowers.ScoresToDuplicate.Contains(indexOfScoreselected);
    }

    void Update()
    {
        if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDie))
        {
            RefreshPuctuation();
            enemyDied = true;
        }
    }

    void RefreshPuctuation ()
    {
        if (!enemyDied)
        {
            if (PlayerPowers.ScoreDuplicatorActivated && isDuplicable)
                multiplyBy = 2;
            else
                multiplyBy = 1;

            Score.SetScoreOfLevel(indexOfScoreselected, Score.GetScoreOfLevel(indexOfScoreselected) + (valueToIncrease * multiplyBy));

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreselected].accumulative)
            {
                if(!ScoreTypesManager.instance.ScoresData[indexOfScoreselected].accumulateOnlyIfWin)
                    Accumulated.SetAccumulated(indexOfScoreselected, Accumulated.GetAccumulated(indexOfScoreselected) + (valueToIncrease * multiplyBy));
            }

            if (ScoreTypesManager.instance.ScoresData[indexOfScoreselected].saveBestByLevel
                && !ScoreTypesManager.instance.ScoresData[indexOfScoreselected].saveBestOnlyIfWin)
            {
                if (Score.GetScoreOfLevel(indexOfScoreselected) > Score.GetBestScoreOfLevel(indexOfScoreselected, Levels.GetLevelIdentificationOfCurrentScene()))
                    Score.SetBestScoreOfLevel(indexOfScoreselected, Levels.GetLevelIdentificationOfCurrentScene(), Score.GetScoreOfLevel(indexOfScoreselected));
            }

            ShowScore.refreshScores = true;
        }
    }
}
