using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(DismountOfPlayer))]

public class DismountOfPlayerEditor : Editor
{
    DismountOfPlayer dismountOfPlayer;

    void OnEnable()
    {
        dismountOfPlayer = (DismountOfPlayer)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 120;
                dismountOfPlayer.inputToDismount = EditorGUILayout.Popup("Input to dismount: ", dismountOfPlayer.inputToDismount, ConvertListStringToArray(InputControlsManager.instance.inputNames));
            }
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 160;
                dismountOfPlayer.timeToReactivateMount = EditorGUILayout.FloatField("Time to reactivate mount: ", dismountOfPlayer.timeToReactivateMount);
            }
            if(dismountOfPlayer.GetComponent<MountLifeManager>() != null)
                EditorGUILayout.HelpBox("The player will be automatically dismounted in case that the mount dies.", MessageType.Info);
        }

        EditorUtility.SetDirty(dismountOfPlayer);
        Undo.RecordObject(dismountOfPlayer, "Undo dismountOfPlayer");
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
