using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ScoreIncreaser_OnEnemyDies))]

public class ScoreIncreaser_OnEnemyDiesEditor : Editor
{
    ScoreIncreaser_OnEnemyDies ScoreIncreaser;

    void OnEnable()
    {
        ScoreIncreaser = (ScoreIncreaser_OnEnemyDies)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 160;
            ScoreIncreaser.indexOfScoreselected = EditorGUILayout.Popup("Score to change: ", ScoreIncreaser.indexOfScoreselected, ScoresList());

            ScoreIncreaser.valueToIncrease = EditorGUILayout.FloatField("Value to increase: ", ScoreIncreaser.valueToIncrease);

            EditorGUILayout.HelpBox("This action will be called when this enemy dies.", MessageType.None);

        }

        EditorUtility.SetDirty(ScoreIncreaser);
        Undo.RecordObject(ScoreIncreaser, "Undo ScoreIncreaser");
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
