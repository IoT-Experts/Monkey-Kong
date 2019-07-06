using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreIncreaser_OnTrigger))]
[CanEditMultipleObjects]
public class ScoreIncreaser_OnTriggerEditor : Editor
{
    ScoreIncreaser_OnTrigger ScoreIncreaser;
    SerializedProperty indexOfScoreSelectedProp;
    SerializedProperty valueToIncreaseProp;
    SerializedProperty canEnterOnlyOneTimeProp;
    SerializedProperty destroyWhenItIsObtainedProp;
    SerializedProperty delayToDestroyProp;

    bool enableMultiEditing = false;

    void OnEnable()
    {
        ScoreIncreaser = (ScoreIncreaser_OnTrigger)target;
        indexOfScoreSelectedProp = serializedObject.FindProperty("indexOfScoreSelected");
        valueToIncreaseProp = serializedObject.FindProperty("valueToIncrease");
        canEnterOnlyOneTimeProp = serializedObject.FindProperty("canEnterOnlyOneTime");
        destroyWhenItIsObtainedProp = serializedObject.FindProperty("destroyWhenItIsObtained");
        delayToDestroyProp = serializedObject.FindProperty("delayToDestroy");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            if ((!indexOfScoreSelectedProp.hasMultipleDifferentValues && !valueToIncreaseProp.hasMultipleDifferentValues &&
                  !canEnterOnlyOneTimeProp.hasMultipleDifferentValues && !destroyWhenItIsObtainedProp.hasMultipleDifferentValues &&
                  !delayToDestroyProp.hasMultipleDifferentValues) || enableMultiEditing)
            {
                EditorGUIUtility.labelWidth = 160;

                indexOfScoreSelectedProp.intValue = EditorGUILayout.Popup("Score to change: ", indexOfScoreSelectedProp.intValue, ScoresList());

                valueToIncreaseProp.floatValue = EditorGUILayout.FloatField("Value to increase: ", valueToIncreaseProp.floatValue);
                destroyWhenItIsObtainedProp.boolValue = EditorGUILayout.Toggle("Destroy when it's obtained: ", destroyWhenItIsObtainedProp.boolValue);

                if (destroyWhenItIsObtainedProp.boolValue)
                {
                    delayToDestroyProp.floatValue = EditorGUILayout.FloatField("Delay to destroy: ", delayToDestroyProp.floatValue);
                }

                if (!destroyWhenItIsObtainedProp.boolValue || (destroyWhenItIsObtainedProp.boolValue && delayToDestroyProp.floatValue > 0))
                {
                    canEnterOnlyOneTimeProp.boolValue = EditorGUILayout.Toggle("Can enter only once: ", canEnterOnlyOneTimeProp.boolValue);

                    if (canEnterOnlyOneTimeProp.boolValue)
                        EditorGUILayout.HelpBox("This Score will be not increased when the player enter by second time.", MessageType.Info);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The multi-object editing is not available when it has multiple different values.", MessageType.Warning);

                if(GUILayout.Button("Enable multi-object editing and put same values in all objects", EditorStyles.miniButton))
                {
                    enableMultiEditing = true;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(ScoreIncreaser);
        Undo.RecordObject(ScoreIncreaser, "Undo Score increaser");
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
