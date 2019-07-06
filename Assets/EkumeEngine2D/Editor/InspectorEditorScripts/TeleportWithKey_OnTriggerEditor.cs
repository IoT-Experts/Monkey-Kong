using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(TeleportWithKey_OnTrigger))]

public class TeleportWithKey_OnTriggerEditor : Editor
{

    TeleportWithKey_OnTrigger teleportWithKey;

    void OnEnable()
    {
        teleportWithKey = (TeleportWithKey_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            teleportWithKey.inputControl = EditorGUILayout.Popup("Input control: ", teleportWithKey.inputControl, ConvertListStringToArray(teleportWithKey.inputControlsManager.inputNames));
            teleportWithKey.positionToTeleport = EditorGUILayout.ObjectField("Position to teleport: ", teleportWithKey.positionToTeleport, typeof(Transform), true) as Transform;
        }

        EditorUtility.SetDirty(teleportWithKey);
        Undo.RecordObject(teleportWithKey, "Undo teleport with key");
    }

    string[] ConvertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }
}
