using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerActivator))]

public class PlayerActivatorEditor : Editor
{
    PlayerActivator playerActivator;

    void OnEnable()
    {
        playerActivator = (PlayerActivator)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            playerActivator.whatActivate = (PlayerActivator.WhatActivate)EditorGUILayout.EnumPopup("What activate?: ", playerActivator.whatActivate);

            if (playerActivator.whatActivate == PlayerActivator.WhatActivate.SpecificPlayer)
                playerActivator.playerToActivate = EditorGUILayout.Popup("Player to activate: ",playerActivator.playerToActivate, ConvertListStringToArray(PlayersManager.instance.playerNames));
        }

        EditorUtility.SetDirty(playerActivator);
        Undo.RecordObject(playerActivator, "Undo playerActivator");
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