using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RideTheMount))]

public class RideTheMountEditor : Editor
{
    RideTheMount rideTheMount;

    void OnEnable()
    {
        rideTheMount = (RideTheMount)target;
    }

    public override void OnInspectorGUI()
    {
        if (!rideTheMount.GetComponent<Collider2D>().isTrigger)
            EditorGUILayout.HelpBox("The Collider2D should be of type Trigger.", MessageType.Error);

        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 90;
                rideTheMount.howToRide = (RideTheMount.HowToRide)EditorGUILayout.EnumPopup("How to ride: ", rideTheMount.howToRide);

                if (rideTheMount.howToRide == RideTheMount.HowToRide.PressingInput)
                    rideTheMount.inputControlToRide = EditorGUILayout.Popup("Input to ride: ", rideTheMount.inputControlToRide, ConvertListStringToArray(InputControlsManager.instance.inputNames));

            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 155;
                rideTheMount.disableWeaponOfPlayer = EditorGUILayout.Toggle("Disable weapon of player: ", rideTheMount.disableWeaponOfPlayer);
                EditorGUILayout.LabelField("It avoid that the player shoot with his weapon.", EditorStyles.miniLabel);
            }
        }

        EditorUtility.SetDirty(rideTheMount);
        Undo.RecordObject(rideTheMount, "Undo rideTheMount");

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
