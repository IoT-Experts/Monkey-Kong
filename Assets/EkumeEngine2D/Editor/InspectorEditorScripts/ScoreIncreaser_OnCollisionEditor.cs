using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ScoreIncreaser_OnCollision))]

public class ScoreIncreaser_OnCollisionEditor : Editor
{
    ScoreIncreaser_OnCollision ScoreIncreaser;

    void OnEnable()
    {
        ScoreIncreaser = (ScoreIncreaser_OnCollision)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 160;
            ScoreIncreaser.indexOfScoreselected = EditorGUILayout.Popup("Score to change: ", ScoreIncreaser.indexOfScoreselected, ScoresList());

            ScoreIncreaser.valueToIncrease = EditorGUILayout.FloatField("Value to increase: ", ScoreIncreaser.valueToIncrease);
            ScoreIncreaser.destroyWhenItIsObtained = EditorGUILayout.Toggle("Destroy when it's obtained: ", ScoreIncreaser.destroyWhenItIsObtained);

            if (ScoreIncreaser.destroyWhenItIsObtained)
            {
                ScoreIncreaser.delayToDestroy = EditorGUILayout.FloatField("Delay to destroy: ", ScoreIncreaser.delayToDestroy);
            }

            if (!ScoreIncreaser.destroyWhenItIsObtained || (ScoreIncreaser.destroyWhenItIsObtained && ScoreIncreaser.delayToDestroy == 0))
            {
                ScoreIncreaser.canEnterOnlyOneTime = EditorGUILayout.Toggle("Can enter only once: ", ScoreIncreaser.canEnterOnlyOneTime);

                if (ScoreIncreaser.canEnterOnlyOneTime)
                    EditorGUILayout.HelpBox("This Score will be not increased when the player enter by second time.", MessageType.Info);
            }

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
