using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEditor.Animations;

[CustomEditor(typeof(AnimatorManagerOfPlayer))]
public class AnimatorManagerOfPlayerEditor : Editor
{
    AnimatorManagerOfPlayer animatorManagerOfPlayer;
    AnimatorController animatorController;
    Rigidbody2D rb2DOfPlayer;

    void OnEnable()
    {
        animatorManagerOfPlayer = (AnimatorManagerOfPlayer)target;

        if (animatorManagerOfPlayer.currentAnimator == null)
        {
            animatorManagerOfPlayer.currentAnimator = animatorManagerOfPlayer.gameObject.GetComponent<Animator>();
        }

        if (animatorManagerOfPlayer.GetComponent<Rigidbody2D>() != null)
            rb2DOfPlayer = animatorManagerOfPlayer.GetComponent<Rigidbody2D>();
    }

    public override void OnInspectorGUI()
    {
        if (animatorManagerOfPlayer.currentAnimator.runtimeAnimatorController != null)
        {
            animatorController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(animatorManagerOfPlayer.currentAnimator.runtimeAnimatorController), typeof(AnimatorController)) as AnimatorController;

            using (new GUILayout.VerticalScope("box"))
            {
                for (int i = 0; i < animatorManagerOfPlayer.parameterNames.Count; i++)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUIUtility.labelWidth = 70;
                        animatorManagerOfPlayer.parameterNames[i] = EditorGUILayout.TextField("Parameter: ", animatorManagerOfPlayer.parameterNames[i]);
                        EditorGUIUtility.labelWidth = 42;
                        animatorManagerOfPlayer.playerStates[i] = (PlayerStatesEnum)EditorGUILayout.EnumPopup("State: ", animatorManagerOfPlayer.playerStates[i]);
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            animatorManagerOfPlayer.parameterNames.RemoveAt(i);
                            animatorManagerOfPlayer.playerStates.RemoveAt(i);
                            animatorManagerOfPlayer.selectedCategoryOfAttack.RemoveAt(i);
                            animatorManagerOfPlayer.categoryOfWeaponSelected.RemoveAt(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                        if(animatorManagerOfPlayer.playerStates[i] == PlayerStatesEnum.PlayerAttackWithWeapon)
                        {
                            EditorGUIUtility.labelWidth = 105;
                            animatorManagerOfPlayer.selectedCategoryOfAttack[i] = EditorGUILayout.Popup("Attack category: ", animatorManagerOfPlayer.selectedCategoryOfAttack[i], ConvertListStringToArray(WeaponFactory.instance.weaponCategories), GUILayout.Width(200));
                        }
                        else if (animatorManagerOfPlayer.playerStates[i] == PlayerStatesEnum.PlayerIsUsingSpecificWeaponCategory)
                        {
                            EditorGUIUtility.labelWidth = 115;
                            animatorManagerOfPlayer.categoryOfWeaponSelected[i] = EditorGUILayout.Popup("Weapon Category: ", animatorManagerOfPlayer.categoryOfWeaponSelected[i], ConvertListStringToArray(WeaponFactory.instance.weaponCategories), GUILayout.Width(200));
                        }
                        else if(animatorManagerOfPlayer.playerStates[i] == PlayerStatesEnum.PlayerIsInParkourWall)
                        {
                            if (rb2DOfPlayer != null)
                            {
                                if (rb2DOfPlayer.collisionDetectionMode != CollisionDetectionMode2D.Continuous)
                                {
                                    using (new GUILayout.VerticalScope("helpbox"))
                                    {
                                        EditorGUILayout.HelpBox("This state might not works well if you do not have selected the option 'Continuous' in the Collision Detection of the RigidBody2D.", MessageType.Warning);

                                        if (GUILayout.Button("Set as 'Continuous' the Collision Detection of the RigidBody2D", EditorStyles.miniButtonMid))
                                        {
                                            rb2DOfPlayer.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (animatorManagerOfPlayer.parameterNames[i] != "")
                    {
                        if (!IsStringInArray(ParametersOfAnimator(animatorController), animatorManagerOfPlayer.parameterNames[i]))
                        {
                            EditorGUILayout.HelpBox("The parameter does not exist in the animator controller.", MessageType.Warning);
                        }
                        else if (!IsBooleanType(ParametersOfAnimator(animatorController), animatorManagerOfPlayer.parameterNames[i]))
                        {
                            EditorGUILayout.HelpBox("The parameter should be of type bool.", MessageType.Warning);
                        }
                    }

                }
                if (GUILayout.Button("Add new state control"))
                {
                    animatorManagerOfPlayer.parameterNames.Add("");
                    animatorManagerOfPlayer.playerStates.Add(PlayerStatesEnum.IsTheSecondJump);
                    animatorManagerOfPlayer.selectedCategoryOfAttack.Add(0);
                    animatorManagerOfPlayer.categoryOfWeaponSelected.Add(0);
                }
            }
        }
        else
        {
            using (new GUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.LabelField("*Add an animator controller to the component Animator.");
            }
        }

        if (animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerLoseOneLive)
            || animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerLoseHealthPoint)
            || animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerLoseAllLives)
            || animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerAttackWithWeapon)
            || animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerReappearInSavePoint))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.HelpBox("For the arrows with the conditions that depends of the parameters: PlayerAttack, PlayerLoseOneLive, PlayerLoseHealthPoint, PlayerLoseAllLives, or PlayerContinueLevel, Set the value of \"Has Exit Time\" in true and the value of \"Exit time\" in 1 (For the arrows that leaves the state). And for the arrows that enter in the states (with the mentioned parameters) set the value of \"Has Exit Time\" in false. For the all arrows (transitions) is recommended put a value in \'Transition Duration\' between 0 and 0.1 in the transitions that depends on the states mentioned before.", MessageType.Info);
            }
        }

        if(animatorManagerOfPlayer.tag == "Mount" && !animatorManagerOfPlayer.playerStates.Contains(PlayerStatesEnum.PlayerIsRidingAMount))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.HelpBox("To manages the animations of the mount, you should have the state PlayerIsMountingAMount in some parameter.", MessageType.Warning);
            }
        }

        EditorUtility.SetDirty(animatorManagerOfPlayer);
        Undo.RecordObject(animatorManagerOfPlayer, "Undo animationManagerOfPlayer");

    }

    RuntimeAnimatorController GetEffectiveController(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }

        return controller;
    }

    string[] ParametersOfAnimator (AnimatorController animatorController)
    {
        string[] parameters = new string[animatorController.parameters.Length];

        for(int i = 0; i < parameters.Length; i++)
        {
            parameters[i] = animatorController.parameters[i].name;
        }

        return parameters;
    }

    bool IsStringInArray(string[] array, string valueToSearch)
    {
        bool valueToReturn = false;

        foreach(string value in array)
        {
            if(value == valueToSearch)
            {
                valueToReturn = true;
                break;
            }
        }

        return valueToReturn;
    }

    bool IsBooleanType (string[] parameters, string parameterName)
    {
        bool valueToReturn = false;
        int counter = 0;
        foreach (string value in parameters)
        {
            if (value == parameterName)
            {
                if(animatorController.parameters[counter].type == AnimatorControllerParameterType.Bool)
                    valueToReturn = true;

                break;
            }
            counter++;
        }

        return valueToReturn;
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
