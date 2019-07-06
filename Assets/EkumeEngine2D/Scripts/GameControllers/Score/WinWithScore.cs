using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeSavedData.Scores;
using EkumeSavedData;

public class WinWithScore : MonoBehaviour
{
    public List<int> dataTypeSeleced = new List<int>(); // 0 Accumulated, 1 Current Score, 2 Best score of this level
    public List<int> indexOfScoreselected = new List<int>();
    public List<int> minimumScore = new List<int>();

    public static bool verificateIfWin;

    GameOverManager finishLevelController;

    void Awake ()
    {
        finishLevelController = FindObjectOfType<GameOverManager>();
    }

    void Update ()
    {
        if (verificateIfWin)
        {
            int numberOfTrueConditions = 0;
            for (int i = 0; i < dataTypeSeleced.Count; i++)
            {
                switch (dataTypeSeleced[i])
                {
                    case 0:

                        if (Accumulated.GetAccumulated(indexOfScoreselected[i]) >= minimumScore[i])
                            numberOfTrueConditions++;

                        break;

                    case 1:

                        if (Score.GetScoreOfLevel(indexOfScoreselected[i]) >= minimumScore[i])
                            numberOfTrueConditions++;

                        break;

                    case 2:

                        if (Score.GetBestScoreOfLevel(indexOfScoreselected[i], Levels.GetLevelIdentificationOfCurrentScene()) >= minimumScore[i])
                            numberOfTrueConditions++;

                        break;
                }
            }

            if (numberOfTrueConditions == dataTypeSeleced.Count)
            {
                finishLevelController.FinishTheGame("win", false);
            }

            verificateIfWin = false;
        }
	}
}
