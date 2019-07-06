using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerLifeReductor))]

public class PlayerLifeReductorEditor : Editor
{
    PlayerLifeReductor playerLifeReductor;

    void OnEnable()
    {
        playerLifeReductor = (PlayerLifeReductor)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            playerLifeReductor.actionActivator = (PlayerLifeReductor.ActivatorOfAction)EditorGUILayout.EnumPopup("How lose health: ", playerLifeReductor.actionActivator);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("How many health reduce", EditorStyles.boldLabel);

            if (playerLifeReductor.actionActivator == PlayerLifeReductor.ActivatorOfAction.OnTriggerStay2D || playerLifeReductor.actionActivator == PlayerLifeReductor.ActivatorOfAction.OnCollisionStay2D)
            {
                playerLifeReductor.reduceAllHealth = false;
            }
            else
            {
                playerLifeReductor.reduceAllHealth = EditorGUILayout.Toggle("Reduce all health: ", playerLifeReductor.reduceAllHealth);
            }

            EditorGUIUtility.labelWidth = 175;

            if (!playerLifeReductor.reduceAllHealth)
            {
                playerLifeReductor.healthToReduce = EditorGUILayout.FloatField(
                    ((playerLifeReductor.actionActivator == PlayerLifeReductor.ActivatorOfAction.OnTriggerStay2D
                    || playerLifeReductor.actionActivator == PlayerLifeReductor.ActivatorOfAction.OnCollisionStay2D) ?
                    "Health to reduce per second:" : "Health to reduce: ")
                    , playerLifeReductor.healthToReduce);
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            playerLifeReductor.ignoreShields = EditorGUILayout.Toggle("Ignore shields (Powers): ", playerLifeReductor.ignoreShields);
            playerLifeReductor.ignoreImmunityTime = EditorGUILayout.Toggle("Ignore immunity time: ", playerLifeReductor.ignoreImmunityTime);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("If the player is in a mount", EditorStyles.boldLabel);
            playerLifeReductor.sendDamagesToThePlayer = EditorGUILayout.Toggle("Send damages to the player: ", playerLifeReductor.sendDamagesToThePlayer);
        }

        EditorUtility.SetDirty(playerLifeReductor);
        Undo.RecordObject(playerLifeReductor, "Undo playerLifeReductor");
    }
}
