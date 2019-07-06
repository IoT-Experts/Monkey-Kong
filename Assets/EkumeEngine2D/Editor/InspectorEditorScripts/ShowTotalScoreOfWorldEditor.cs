using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeSavedData;

[CustomEditor(typeof(ShowTotalScoreOfWorld))]
public class ShowTotalScoreOfWorldEditor : Editor
{
    ShowTotalScoreOfWorld showScore;

    void OnEnable ()
    {
        showScore = (ShowTotalScoreOfWorld)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script shows the sum of the best scores of all levels of a world.", MessageType.Info);
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 180;
            showScore.indexOfScoreselected = EditorGUILayout.Popup("Score to show: ", showScore.indexOfScoreselected, ScoresList());

            if(!ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].saveBestByLevel)
            {
                EditorGUILayout.HelpBox("Error!: " + ScoreTypesManager.instance.ScoresData[showScore.indexOfScoreselected].scoreName + " don't have the option \"Save Best\" selected", MessageType.Error);
            }

            showScore.format = EditorGUILayout.TextField("Format: ", showScore.format);
            showScore.worldToShowTotalScore = EditorGUILayout.Popup("World: ", showScore.worldToShowTotalScore, Levels.GetListOfWorlds());

            showScore.activateObjects = EditorGUILayout.Toggle("Activate objs with score: ", showScore.activateObjects);

            if (showScore.activateObjects)
            {
                using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                {

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Minimum to activate / GameObject to activate");
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
                }
            }

            if (!showScore.format.Contains("0"))
                GUILayout.Label("Error!: The format should contains at less a 0", EditorStyles.boldLabel);
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
