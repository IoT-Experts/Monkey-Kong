using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeSavedData;
using UnityEngine.UI;

[CustomEditor(typeof(ShowScore))]
public class ShowScoreEditor : Editor
{
    ShowScore showScore;

    string[] whatToShow = new string[3] { "Accumulated", "Current Score", "Best score of level" };

    void OnEnable ()
    {
        showScore = (ShowScore)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 180;
            showScore.whatToShowSelected = EditorGUILayout.Popup("What to show", showScore.whatToShowSelected, whatToShow);
            showScore.indexOfScoreselected = EditorGUILayout.Popup("Score to show: ", showScore.indexOfScoreselected, ScoresList());
            showScore.dontShowText = EditorGUILayout.Toggle("It does not shows text: ", showScore.dontShowText);
            if(!showScore.dontShowText)
                showScore.format = EditorGUILayout.TextField("Format: ", showScore.format);

            if (showScore.whatToShowSelected != 0) //if it is not Accumulated
            {
                showScore.showScoreOfThisLevel = EditorGUILayout.Toggle("Show Score of this level: ", showScore.showScoreOfThisLevel);
                if (!showScore.showScoreOfThisLevel)
                    showScore.levelToShowScore = EditorGUILayout.Popup("Level: ", showScore.levelToShowScore, Levels.GetListOfLevelIdentifications());
            }

            showScore.activateObjects = EditorGUILayout.Toggle("Activate objects with score: ", showScore.activateObjects);

            if (showScore.activateObjects)
            {
                using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                {
                    GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 80;
                    EditorGUILayout.LabelField("Minimum to activate");
                    EditorGUILayout.LabelField("GameObject to enable");
                    EditorGUILayout.LabelField(" ", GUILayout.Width(38));
                    GUILayout.EndHorizontal();

                    for (int i = 0; i < showScore.objectsToActivate.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        showScore.valuesToActivate[i] = EditorGUILayout.IntField(showScore.valuesToActivate[i]);
                        showScore.objectsToActivate[i] = EditorGUILayout.ObjectField(showScore.objectsToActivate[i], typeof(GameObject), true) as GameObject;

                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            showScore.objectsToActivate.RemoveAt(i);
                            showScore.valuesToActivate.RemoveAt(i);
                        }
                        GUILayout.EndHorizontal();
                    }
                    

                    if (GUILayout.Button("New"))
                    {
                        showScore.objectsToActivate.Add(null);
                        showScore.valuesToActivate.Add(0);
                    }

                    EditorGUILayout.HelpBox("This values will be compared with the value of 'What to show'", MessageType.None);
                }
            }

            if (!showScore.format.Contains("0"))
                EditorGUILayout.HelpBox("The format should contains at less a 0", MessageType.Error);

            if (showScore.whatToShowSelected == 0 && !ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].accumulative)
            {
                EditorGUILayout.HelpBox(ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].scoreName + " is not accumulative", MessageType.Error);
            }
            else if (showScore.whatToShowSelected == 2 && !ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].saveBestByLevel)
            {
                EditorGUILayout.HelpBox(ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].scoreName + " don't have the option \"Save Best\" selected", MessageType.Error);
            }

            if (showScore.GetComponent<Text>() == null && !showScore.dontShowText)
            {
                EditorGUILayout.HelpBox("Please add to this GameObject the component Text if you want to show some Score, or enable the option \"It does not shows text\" if you just want to enable GameObjects with Scores.", MessageType.Error);
            }
        }

        EditorUtility.SetDirty(showScore);
        Undo.RecordObject(showScore, "Undo showScore");
    }


    string[] ScoresList()
    {
        string[] ScoreList = new string[ScoreTypesManager.instance.ScoresData.Count];

        for (int i = 0; i < ScoreList.Length; i++)
        {
            ScoreList[i] = ScoreTypesManager.instance.ScoresData[i].scoreName;
        }

        return ScoreList;
    }
}
