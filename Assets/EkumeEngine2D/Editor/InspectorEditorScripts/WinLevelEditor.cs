using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WinLevel))]
public class WinLevelEditor : Editor
{
    WinLevel winLevel;

    string[] whatToShow = new string[3] { "Accumulated", "Current Score", "Best score of this level" };

    void OnEnable()
    {
        winLevel = (WinLevel)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 155;
                winLevel.actionActivator = (WinLevel.ActivatorOfAction)EditorGUILayout.EnumPopup("How is activated: ", winLevel.actionActivator);
                winLevel.needScoreToWin = EditorGUILayout.Toggle("Score condition: ", winLevel.needScoreToWin);
            }

            if (winLevel.needScoreToWin)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Conditions", EditorStyles.boldLabel);

                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Data type");
                        GUILayout.Label("Score name");
                        GUILayout.Label("Minimum score");
                        GUILayout.EndHorizontal();
                        for (int i = 0; i < winLevel.dataTypeSeleced.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            winLevel.dataTypeSeleced[i] = EditorGUILayout.Popup(winLevel.dataTypeSeleced[i], whatToShow);
                            winLevel.indexOfScoreselected[i] = EditorGUILayout.Popup(winLevel.indexOfScoreselected[i], ScoresList());
                            winLevel.minimumScore[i] = EditorGUILayout.IntField(winLevel.minimumScore[i]);
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                winLevel.dataTypeSeleced.RemoveAt(i);
                                winLevel.indexOfScoreselected.RemoveAt(i);
                                winLevel.minimumScore.RemoveAt(i);
                            }
                            GUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add condition of score to win"))
                        {
                            winLevel.dataTypeSeleced.Add(0);
                            winLevel.indexOfScoreselected.Add(0);
                            winLevel.minimumScore.Add(0);
                        }
                    }

                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("Object to enable if it does not reach the conditions");
                        winLevel.objToActivateIfDontHaveScores = EditorGUILayout.ObjectField("GameObject: ", winLevel.objToActivateIfDontHaveScores, typeof(GameObject), true) as GameObject;
                        winLevel.timeToKeepEnabled = EditorGUILayout.FloatField("Time to keep enabled: ", winLevel.timeToKeepEnabled);
                    }
                }
            }
        }

        EditorUtility.SetDirty(winLevel);
        Undo.RecordObject(winLevel, "Undo win level");
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
