using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeSavedData.Scores;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WinLevel : MonoBehaviour
{

    public enum ActivatorOfAction { OnTriggerEnter2D, OnCollisionEnter2D }
    public ActivatorOfAction actionActivator;
    GameOverManager finishLevelController;
    public bool needScoreToWin;
    public GameObject objToActivateIfDontHaveScores;
    public float timeToKeepEnabled = 1.7f;

    // Variables if need Score to win

    public List<int> dataTypeSeleced = new List<int>(); // 0 Accumulated, 1 Current Score, 2 Best score of this level
    public List<int> indexOfScoreselected = new List<int>();
    public List<int> minimumScore = new List<int>();

    void Awake ()
    {
        finishLevelController = FindObjectOfType<GameOverManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                WinTheLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            if (actionActivator == ActivatorOfAction.OnCollisionEnter2D)
                WinTheLevel();
        }
    }

    void WinTheLevel()
    {
        int numberOfTrueConditions = 0;
        if (needScoreToWin)
        {
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

                        if (Score.GetBestScoreOfLevel(indexOfScoreselected[i], SceneManager.GetActiveScene().name) >= minimumScore[i])
                            numberOfTrueConditions++;

                        break;
                }
            }
        }

        if(needScoreToWin && numberOfTrueConditions != dataTypeSeleced.Count)
        {
            if (objToActivateIfDontHaveScores != null)
            {
                objToActivateIfDontHaveScores.SetActive(true);
                StartCoroutine(DisableGameObject());
            }
        }

        if (!needScoreToWin || (needScoreToWin && numberOfTrueConditions == dataTypeSeleced.Count))
        {
            finishLevelController.FinishTheGame("win", false);
        }
    }

    IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(timeToKeepEnabled);
        objToActivateIfDontHaveScores.SetActive(false);
    }
}


