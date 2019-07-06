using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerJump))]
public class PlayerJumpEditor : Editor
{

    PlayerJump playerJump;

    void OnEnable()
    {
        playerJump = (PlayerJump)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Ground detection", EditorStyles.boldLabel);
            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ground checker");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                playerJump.groundChecker = EditorGUILayout.ObjectField(playerJump.groundChecker, typeof(Transform), true) as Transform;
                EditorGUIUtility.labelWidth = 50;
                playerJump.radiusOfGroundChecker = EditorGUILayout.FloatField("Radius: ", playerJump.radiusOfGroundChecker);
                EditorGUILayout.EndHorizontal();
            }
        }

        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Jump options", EditorStyles.boldLabel);

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Jump force");
                EditorGUILayout.LabelField("Input control");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                playerJump.jumpForce = EditorGUILayout.FloatField(playerJump.jumpForce);
                playerJump.inputControl = EditorGUILayout.Popup(playerJump.inputControl, ConvertListStringToArray(InputControlsManager.instance.inputNames));
                EditorGUILayout.EndHorizontal();
            }
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;

                EditorGUILayout.BeginHorizontal();
                playerJump.activateDoubleJump = EditorGUILayout.Toggle("Double jump: ", playerJump.activateDoubleJump);

                if (playerJump.activateDoubleJump)
                {
                    playerJump.noLimitOfJumps = false;
                }

                playerJump.noLimitOfJumps = EditorGUILayout.Toggle("Jump without limits: ", playerJump.noLimitOfJumps);

                if (playerJump.noLimitOfJumps)
                {
                    playerJump.activateDoubleJump = false;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.HelpBox("Remember add the layer \"Ground\" to all the GameObjects in that the player can jump.", MessageType.Info);
        }

        EditorUtility.SetDirty(playerJump);
        Undo.RecordObject(playerJump, "Undo playerJump");
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
