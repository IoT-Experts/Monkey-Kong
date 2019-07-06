using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using EkumeSavedData.Scores;
using EkumeSavedData;

public class ShowScore : MonoBehaviour
{
    public bool dontShowText;
    public int whatToShowSelected; // 0 Accumulated, 1 Current Score, 2 Best score of this level
    public int indexOfScoreselected;
    public string format = "0";
    public bool showScoreOfThisLevel = true;
    public int levelToShowScore;
    public static bool refreshScores;
    public bool activateObjects;
    public List<int> valuesToActivate = new List<int>();
    public List<GameObject> objectsToActivate = new List<GameObject>();
    Text thisText;

    void Awake()
    {
        if (!dontShowText)
            thisText = GetComponent<Text>();
    }

    void OnEnable()
    {
         RefreshScores();
    }

    void Update()
    {
        if (refreshScores)
        {
            RefreshScores();
        }
    }

    void LateUpdate()
    {
        if (refreshScores)
        {
            refreshScores = false;
            WinWithScore.verificateIfWin = true;
        }
    }

    void RefreshScores()
    {
        switch (whatToShowSelected)
        {
            case 0: //Accumulated

                if (!dontShowText)
                    thisText.text = Accumulated.GetAccumulated(indexOfScoreselected).ToString(format);

                if (activateObjects)
                {
                    for (int i = 0; i < objectsToActivate.Count; i++)
                    {
                        if (!objectsToActivate[i].activeSelf && Accumulated.GetAccumulated(indexOfScoreselected) >= valuesToActivate[i])
                        {
                            objectsToActivate[i].SetActive(true);
                        }
                    }
                }

                break;
            case 1: // Current Score

                if (!dontShowText)
                    thisText.text = Score.GetScoreOfLevel(indexOfScoreselected).ToString(format);

                if (activateObjects)
                {
                    for (int i = 0; i < objectsToActivate.Count; i++)
                    {
                        if (objectsToActivate[i] != null)
                        {
                            if (!objectsToActivate[i].activeSelf && Score.GetScoreOfLevel(indexOfScoreselected) >= valuesToActivate[i])
                            {
                                objectsToActivate[i].SetActive(true);
                            }
                        }
                        else
                        {
                            Debug.Log("Please add the corresponding GameObjects in the component 'ShowScore' in the GameObject: " + this.gameObject.name);
                        }
                    }
                }

                break;
            case 2: // Best score of this level

                if (!dontShowText)
                       thisText.text = Score.GetBestScoreOfLevel(indexOfScoreselected,
                       (showScoreOfThisLevel) ? Levels.GetLevelIdentificationOfCurrentScene()
                       : Levels.GetLevelIdentificationOfNumberOfList(levelToShowScore)
                       ).ToString(format);

                if (activateObjects)
                {
                    for (int i = 0; i < objectsToActivate.Count; i++)
                    {
                        if (!objectsToActivate[i].activeSelf && Score.GetBestScoreOfLevel(indexOfScoreselected,
                          (showScoreOfThisLevel) ? Levels.GetLevelIdentificationOfCurrentScene()
                          : Levels.GetLevelIdentificationOfNumberOfList(levelToShowScore)) >= valuesToActivate[i])
                        {
                            objectsToActivate[i].SetActive(true);
                        }
                    }
                }

                break;
        }

        ShowTotalScoreOfWorld.refreshScores = true;
    }
}
