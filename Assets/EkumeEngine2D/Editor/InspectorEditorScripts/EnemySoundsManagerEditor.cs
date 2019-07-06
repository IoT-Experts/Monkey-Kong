using EkumeEnumerations;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(EnemySoundsManager))]

public class EnemySoundsManagerEditor : Editor
{
    EnemySoundsManager enemySoundsManager;

    void OnEnable()
    {
        enemySoundsManager = (EnemySoundsManager)target;
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < enemySoundsManager.audioSource.Count; i++)
        {
            using (var verticalScope0 = new GUILayout.VerticalScope("box"))
            {
                using (var verticalScope1 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("Audio to play", EditorStyles.boldLabel);
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        enemySoundsManager.audioSource[i] = EditorGUILayout.ObjectField("Audio source: ", enemySoundsManager.audioSource[i], typeof(AudioSource), true) as AudioSource;
                        enemySoundsManager.audioClip[i] = EditorGUILayout.ObjectField("Clip to play: ", enemySoundsManager.audioClip[i], typeof(AudioClip), true) as AudioClip;
                        enemySoundsManager.playLikeLoop[i] = EditorGUILayout.Toggle("Play like loop: ", enemySoundsManager.playLikeLoop[i]);
                    }
                }

                using (var verticalScope1 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("Conditions", EditorStyles.boldLabel);
                    using (var verticalScope = new GUILayout.VerticalScope("box"))
                    {
                        for (int j = 0; j < enemySoundsManager.conditionsOfStates[i].enemyStates.Count; j++)
                        {
                            using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUIUtility.labelWidth = 40;
                                enemySoundsManager.conditionsOfStates[i].enemyStates[j] = (EnemyStatesEnum)EditorGUILayout.EnumPopup("State: ", enemySoundsManager.conditionsOfStates[i].enemyStates[j]);


                                    EditorGUIUtility.labelWidth = 45;
                                    enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] = (BooleanType)EditorGUILayout.EnumPopup("When: ", enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum[j], GUILayout.Width(103));


                                if (enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] == BooleanType.True)
                                {
                                    enemySoundsManager.conditionsOfStates[i].playWhenIs[j] = true;
                                }
                                else if (enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] == BooleanType.False)
                                {
                                    enemySoundsManager.conditionsOfStates[i].playWhenIs[j] = false;
                                }

                                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(25)))
                                {
                                    enemySoundsManager.conditionsOfStates[i].enemyStates.RemoveAt(j);
                                    enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum.RemoveAt(j);
                                }
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add state condition", EditorStyles.miniButton))
                    {
                        enemySoundsManager.conditionsOfStates[i].enemyStates.Add(EnemyStatesEnum.EnemyIsMoving);
                        enemySoundsManager.conditionsOfStates[i].playWhenIs_Enum.Add(BooleanType.True);
                        enemySoundsManager.conditionsOfStates[i].playWhenIs.Add(true);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Delete"))
                {
                    enemySoundsManager.audioSource.RemoveAt(i);
                    enemySoundsManager.audioClip.RemoveAt(i);
                    enemySoundsManager.playLikeLoop.RemoveAt(i);
                    enemySoundsManager.conditionsOfStates.RemoveAt(i);
                    enemySoundsManager.attackCategory.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add new"))
        {
            enemySoundsManager.audioSource.Add(null);
            enemySoundsManager.audioClip.Add(null);
            enemySoundsManager.playLikeLoop.Add(false);
            enemySoundsManager.conditionsOfStates.Add(new ConditionsForSoundsOfEnemy());
            enemySoundsManager.attackCategory.Add(0);
        }

        EditorUtility.SetDirty(enemySoundsManager);
        Undo.RecordObject(enemySoundsManager, "Undo playerSoundsManager");
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
