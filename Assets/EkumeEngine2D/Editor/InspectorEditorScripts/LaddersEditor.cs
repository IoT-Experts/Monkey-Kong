using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Ladders))]

public class LaddersEditor : Editor
{

    Ladders ladders;

    void OnEnable()
    {
        ladders = (Ladders)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 170;
                ladders.centerCharacterInLadder = EditorGUILayout.Toggle("Center character in ladder: ", ladders.centerCharacterInLadder);
                ladders.enableControlsForSides = EditorGUILayout.Toggle("Enable controls for sides: ", ladders.enableControlsForSides);
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 135;
                ladders.velocityToGoUp = EditorGUILayout.FloatField("Velocity to go up: ", ladders.velocityToGoUp);
                ladders.velocityToGoDown = EditorGUILayout.FloatField("Velocity to go down: ", ladders.velocityToGoDown);

                if (ladders.enableControlsForSides)
                    ladders.velocityForSides = EditorGUILayout.FloatField("Velocity for sides: ", ladders.velocityForSides);
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 120;
                ladders.controlToGoUp = EditorGUILayout.Popup("Input to go up: ", ladders.controlToGoUp, ConvertListStringToArray(InputControlsManager.instance.inputNames));
                ladders.controlToGoDown = EditorGUILayout.Popup("Input to go down: ", ladders.controlToGoDown, ConvertListStringToArray(InputControlsManager.instance.inputNames));

                if (ladders.enableControlsForSides)
                {
                    ladders.controlToGoRight = EditorGUILayout.Popup("Input to go right: ", ladders.controlToGoRight, ConvertListStringToArray(InputControlsManager.instance.inputNames));
                    ladders.controlToGoLeft = EditorGUILayout.Popup("Input to go left: ", ladders.controlToGoLeft, ConvertListStringToArray(InputControlsManager.instance.inputNames));
                }
            }

            if(!ladders.GetComponent<Collider2D>().isTrigger)
            {
                EditorGUILayout.HelpBox("The component Collider2D is not of type trigger.", MessageType.Warning);
            }
        }

        EditorUtility.SetDirty(ladders);
        Undo.RecordObject(ladders, "Undo ladders");
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


