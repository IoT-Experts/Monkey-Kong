using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WinWithScore))]

public class WinWithScoresEditor : Editor
{

    WinWithScore winWithScore;

    string[] whatToShow = new string[3] { "Accumulated", "Current Score", "Best score of this level" };

    void OnEnable()
    {
        winWithScore = (WinWithScore)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            using (var verticalScope3 = new GUILayout.VerticalScope("box"))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Data type");
                GUILayout.Label("Score name");
                GUILayout.Label("Minimum score");
                GUILayout.EndHorizontal();

                for (int i = 0; i < winWithScore.dataTypeSeleced.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    winWithScore.dataTypeSeleced[i] = EditorGUILayout.Popup(winWithScore.dataTypeSeleced[i], whatToShow);
                    winWithScore.indexOfScoreselected[i] = EditorGUILayout.Popup(winWithScore.indexOfScoreselected[i], ScoresList());
                    winWithScore.minimumScore[i] = EditorGUILayout.IntField(winWithScore.minimumScore[i]);
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        winWithScore.dataTypeSeleced.RemoveAt(i);
                        winWithScore.indexOfScoreselected.RemoveAt(i);
                        winWithScore.minimumScore.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add condition of score to win"))
                {
                    winWithScore.dataTypeSeleced.Add(0);
                    winWithScore.indexOfScoreselected.Add(0);
                    winWithScore.minimumScore.Add(0);
                }

                GUILayout.Label("*If the score reach the minimum, the player will win");
            }
        }

        EditorUtility.SetDirty(winWithScore);
        Undo.RecordObject(winWithScore, "Undo winWithScore");
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
