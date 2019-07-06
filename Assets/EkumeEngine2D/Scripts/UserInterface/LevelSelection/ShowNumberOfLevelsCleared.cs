using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using EkumeSavedData;
using EkumeLists;

[RequireComponent(typeof(Text))]
public class ShowNumberOfLevelsCleared : MonoBehaviour
{
    [SerializeField] bool showTotalOfLevelsCleared;
    [SerializeField] ListOfWorlds worldNumber;
    [SerializeField] string textFormat = "0";

    Text thisText;

    void OnEnable()
    {
        thisText = GetComponent<Text>();
        RefreshText();
    }

    void RefreshText()
    {
        int numberOfLevelsCleared = 0;
        if (!showTotalOfLevelsCleared)
            numberOfLevelsCleared = Levels.GetNumberOfLevelsClearedOfWorld(worldNumber.Index);
        else
            numberOfLevelsCleared = Levels.GetTotalOfLevelsCleared();

        thisText.text = numberOfLevelsCleared.ToString(textFormat);
    }
}

