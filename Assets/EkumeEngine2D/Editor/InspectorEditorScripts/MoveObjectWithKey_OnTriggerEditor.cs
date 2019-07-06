using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MoveObjectWithKey_OnTrigger))]

public class MoveObjectWithKey_OnTriggerEditor : Editor
{
    MoveObjectWithKey_OnTrigger moveObjectWithKey;

    void OnEnable()
    {
        moveObjectWithKey = (MoveObjectWithKey_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            moveObjectWithKey.inputControl = EditorGUILayout.Popup("Input control: ", moveObjectWithKey.inputControl, convertListStringToArray(moveObjectWithKey.inputControlsManager.inputNames));
            moveObjectWithKey.velocityToMove = EditorGUILayout.FloatField("Velocity to move: ", moveObjectWithKey.velocityToMove);
            moveObjectWithKey.newPosition = EditorGUILayout.ObjectField("New position: ", moveObjectWithKey.newPosition, typeof(Transform), true) as Transform;
            moveObjectWithKey.objectToMove = EditorGUILayout.ObjectField("Object to move: ", moveObjectWithKey.objectToMove, typeof(Transform), true) as Transform;
            moveObjectWithKey.canReturnToPos1 = EditorGUILayout.Toggle("Can return to pos 1: ", moveObjectWithKey.canReturnToPos1);

            if(moveObjectWithKey.canReturnToPos1)
            {
                EditorGUILayout.LabelField("*It will return to original position if press again the input.", EditorStyles.miniLabel);
            }
        }

        EditorUtility.SetDirty(moveObjectWithKey);
        Undo.RecordObject(moveObjectWithKey, "Undo moveObjectWithKey");
    }

    string[] convertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }
}
