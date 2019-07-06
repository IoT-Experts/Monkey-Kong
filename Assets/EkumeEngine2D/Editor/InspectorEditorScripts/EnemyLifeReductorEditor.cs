using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyLifeReductor))]

public class EnemyLifeReductorEditor : Editor
{
    EnemyLifeReductor enemyLifeReductor;

    void OnEnable()
    {
        enemyLifeReductor = (EnemyLifeReductor)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            enemyLifeReductor.actionActivator = (EnemyLifeReductor.ActivatorOfAction)EditorGUILayout.EnumPopup("How lose health: ", enemyLifeReductor.actionActivator);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("How many health reduce", EditorStyles.boldLabel);

            if (enemyLifeReductor.actionActivator == EnemyLifeReductor.ActivatorOfAction.OnTriggerStay2D || enemyLifeReductor.actionActivator == EnemyLifeReductor.ActivatorOfAction.OnCollisionStay2D)
            {
                enemyLifeReductor.reduceAllHealth = false;
            }
            else
            {
                enemyLifeReductor.reduceAllHealth = EditorGUILayout.Toggle("Reduce all health: ", enemyLifeReductor.reduceAllHealth);
            }

            if (!enemyLifeReductor.reduceAllHealth)
            {

                EditorGUIUtility.labelWidth = 175;

                enemyLifeReductor.healthToReduce = EditorGUILayout.FloatField(
                    ((enemyLifeReductor.actionActivator == EnemyLifeReductor.ActivatorOfAction.OnTriggerStay2D
                    || enemyLifeReductor.actionActivator == EnemyLifeReductor.ActivatorOfAction.OnCollisionStay2D) ?
                    "Health to reduce per second:" : "Health to reduce: ")
                    , enemyLifeReductor.healthToReduce);
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 100;
            EditorGUILayout.LabelField("Tags of the enemies that can receive the damage", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope("box"))
            {
                for(int i = 0; i < enemyLifeReductor.tagsThatActivatesTheAction.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    enemyLifeReductor.tagsThatActivatesTheAction[i] = EditorGUILayout.TagField("Tag: ", enemyLifeReductor.tagsThatActivatesTheAction[i]);
                    if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                    {
                        enemyLifeReductor.tagsThatActivatesTheAction.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("Add tag"))
            {
                enemyLifeReductor.tagsThatActivatesTheAction.Add("");
            }
        }

        EditorUtility.SetDirty(enemyLifeReductor);
        Undo.RecordObject(enemyLifeReductor, "Undo playerLifeReductor");
    }
}
