using EkumeEnumerations;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerSoundsManager))]

public class PlayerSoundsManagerEditor : Editor
{
    PlayerSoundsManager playerSoundsManager;

    void OnEnable()
    {
        playerSoundsManager = (PlayerSoundsManager)target;
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < playerSoundsManager.audioSource.Count; i++)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("Audio to play", EditorStyles.boldLabel);
                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 100;
                        playerSoundsManager.audioSource[i] = EditorGUILayout.ObjectField("Audio source: ", playerSoundsManager.audioSource[i], typeof(AudioSource), true) as AudioSource;
                        playerSoundsManager.audioClip[i] = EditorGUILayout.ObjectField("Clip to play: ", playerSoundsManager.audioClip[i], typeof(AudioClip), true) as AudioClip;
                        playerSoundsManager.playLikeLoop[i] = EditorGUILayout.Toggle("Play like loop: ", playerSoundsManager.playLikeLoop[i]);
                    }
                }

                using (var verticalScope1 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("Conditions", EditorStyles.boldLabel);
                    using (new GUILayout.VerticalScope("box"))
                    {
                        for (int j = 0; j < playerSoundsManager.conditionsOfStates[i].playerStates.Count; j++)
                        {
                            using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUIUtility.labelWidth = 40;
                                playerSoundsManager.conditionsOfStates[i].playerStates[j] = (PlayerStatesEnum)EditorGUILayout.EnumPopup("State: ", playerSoundsManager.conditionsOfStates[i].playerStates[j]);

                                if (playerSoundsManager.conditionsOfStates[i].playerStates[j] != PlayerStatesEnum.PlayerAttackWithWeapon)
                                {
                                    EditorGUIUtility.labelWidth = 55;
                                    playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] = (BooleanType)EditorGUILayout.EnumPopup("When is: ", playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum[j], GUILayout.Width(103));
                                }
                                else
                                {
                                    EditorGUIUtility.labelWidth = 62;
                                    playerSoundsManager.attackCategory[i] = EditorGUILayout.Popup("Category: ", playerSoundsManager.attackCategory[i], ConvertListStringToArray(WeaponFactory.instance.weaponCategories));
                                }

                                if (playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] == BooleanType.True)
                                {
                                    playerSoundsManager.conditionsOfStates[i].playWhenIs[j] = true;
                                }
                                else if (playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum[j] == BooleanType.False)
                                {
                                    playerSoundsManager.conditionsOfStates[i].playWhenIs[j] = false;
                                }

                                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(25)))
                                {
                                    playerSoundsManager.conditionsOfStates[i].playerStates.RemoveAt(j);
                                    playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum.RemoveAt(j);
                                    break;
                                }
                            }

                            EditorGUILayout.EndHorizontal();

                            if (playerSoundsManager.conditionsOfStates[i].playerStates[j] == PlayerStatesEnum.PlayerIsUsingSpecificWeaponCategory)
                            {
                                EditorGUILayout.HelpBox("To play some sound depending if the player attack with some weapon, please select the state PlayerAttackWithWeapon.", MessageType.Error);
                            }
                        }
                        
                        if (playerSoundsManager.useInputControl[i])
                        {
                            using (new GUILayout.VerticalScope("box"))
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUIUtility.labelWidth = 85;
                                playerSoundsManager.inputControl[i] = EditorGUILayout.Popup("Input control: ", playerSoundsManager.inputControl[i], ConvertListStringToArray(InputControlsManager.instance.inputNames));

                                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(25)))
                                {
                                    playerSoundsManager.useInputControl[i] = false;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add state condition", EditorStyles.miniButton))
                    {
                        playerSoundsManager.conditionsOfStates[i].playerStates.Add(PlayerStatesEnum.IsTheSecondJump);
                        playerSoundsManager.conditionsOfStates[i].playWhenIs_Enum.Add(BooleanType.True);
                        playerSoundsManager.conditionsOfStates[i].playWhenIs.Add(true);
                    }

                    if (!playerSoundsManager.useInputControl[i])
                    {
                        if (GUILayout.Button("Add input control condition", EditorStyles.miniButton))
                        {
                            playerSoundsManager.useInputControl[i] = true;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Delete"))
                {
                    playerSoundsManager.audioSource.RemoveAt(i);
                    playerSoundsManager.audioClip.RemoveAt(i);
                    playerSoundsManager.playLikeLoop.RemoveAt(i);
                    playerSoundsManager.conditionsOfStates.RemoveAt(i);
                    playerSoundsManager.useInputControl.RemoveAt(i);
                    playerSoundsManager.inputControl.RemoveAt(i);
                    playerSoundsManager.attackCategory.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add new"))
        {
            playerSoundsManager.audioSource.Add(null);
            playerSoundsManager.audioClip.Add(null);
            playerSoundsManager.playLikeLoop.Add(false);
            playerSoundsManager.conditionsOfStates.Add(new ConditionsForSoundsOfPlayer());
            playerSoundsManager.useInputControl.Add(false);
            playerSoundsManager.inputControl.Add(0);
            playerSoundsManager.attackCategory.Add(0);
        }

        EditorUtility.SetDirty(playerSoundsManager);
        Undo.RecordObject(playerSoundsManager, "Undo playerSoundsManager");
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
