using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using EkumeSavedData.Scores;
using EkumeSavedData;

[RequireComponent(typeof(Text))]
public class ShowTotalScoreOfWorld : MonoBehaviour
{
    public int indexOfScoreselected;
    public string format = "0";
    public int worldToShowTotalScore;
    public static bool refreshScores;
    public bool activateObjects;
    public List<int> valuesToActivate = new List<int>();
    public List<GameObject> objectsToActivate = new List<GameObject>();
    Text thisText;

    void Awake ()
    {
        thisText = GetComponent<Text>();
    }

    void OnEnable ()
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

    void LateUpdate ()
    {
        if (refreshScores)
        {
            refreshScores = false;
            WinWithScore.verificateIfWin = true;
        }
    }

    void RefreshScores()
    {
        float totalScore = Score.GetTotalBestScoreOfWorld(worldToShowTotalScore, indexOfScoreselected);
        thisText.text = totalScore.ToString(format);

        if (activateObjects)
        {
            for (int i = 0; i < objectsToActivate.Count; i++)
            {
                if (!objectsToActivate[i].activeSelf && totalScore >= valuesToActivate[i])
                {
                    objectsToActivate[i].SetActive(true);
                }
            }
        }
    }
}

