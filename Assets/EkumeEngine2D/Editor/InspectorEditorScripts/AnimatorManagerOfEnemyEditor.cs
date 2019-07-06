using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEditor.Animations;

[CustomEditor(typeof(AnimatorManagerOfEnemy))]
public class AnimatorManagerOfEnemyEditor : Editor
{
    AnimatorManagerOfEnemy animationManagerOfEnemy;
    AnimatorController animatorController;

    void OnEnable()
    {
        animationManagerOfEnemy = (AnimatorManagerOfEnemy)target;

        if (animationManagerOfEnemy.currentAnimator == null)
        {
            animationManagerOfEnemy.currentAnimator = animationManagerOfEnemy.gameObject.GetComponent<Animator>();
        }
    }

    public override void OnInspectorGUI()
    {
        if (animationManagerOfEnemy.currentAnimator.runtimeAnimatorController != null)
        {
            animatorController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(animationManagerOfEnemy.currentAnimator.runtimeAnimatorController), typeof(AnimatorController)) as AnimatorController;

            using (new GUILayout.VerticalScope("box"))
            {
                for (int i = 0; i < animationManagerOfEnemy.parameterNames.Count; i++)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUIUtility.labelWidth = 70;
                        animationManagerOfEnemy.parameterNames[i] = EditorGUILayout.TextField("Parameter: ", animationManagerOfEnemy.parameterNames[i]);
                        EditorGUIUtility.labelWidth = 42;
                        animationManagerOfEnemy.enemyStates[i] = (EnemyStatesEnum)EditorGUILayout.EnumPopup("State: ", animationManagerOfEnemy.enemyStates[i]);
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            animationManagerOfEnemy.parameterNames.RemoveAt(i);
                            animationManagerOfEnemy.enemyStates.RemoveAt(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (animationManagerOfEnemy.parameterNames[i] != "")
                    {
                        if (!IsStringInArray(ParametersOfAnimator(animatorController), animationManagerOfEnemy.parameterNames[i]))
                        {
                            EditorGUILayout.LabelField("*The parameter does not exist in the animator controller.");
                        }
                        else if (!IsBooleanType(ParametersOfAnimator(animatorController), animationManagerOfEnemy.parameterNames[i]))
                        {
                            EditorGUILayout.LabelField("*The parameter should be of type bool.", EditorStyles.boldLabel);
                        }
                    }

                }
                if (GUILayout.Button("Add new state control"))
                {
                    animationManagerOfEnemy.parameterNames.Add("");
                    animationManagerOfEnemy.enemyStates.Add(EnemyStatesEnum.EnemyIsMoving);
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

        if (animationManagerOfEnemy.enemyStates.Contains(EnemyStatesEnum.EnemyLoseHealthPoint)
            || animationManagerOfEnemy.enemyStates.Contains(EnemyStatesEnum.EnemyAttack))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.HelpBox("For the transitions with the conditions that depends of the states: EnemyLoseHealthPoint or EnemyWeaponAttack, Set the value of \"Has Exit Time\" in true and the value of \"Exit time\" in 1 (For the arrows that leaves the state)", MessageType.Info);
            }
        }

        EditorUtility.SetDirty(animationManagerOfEnemy);
        Undo.RecordObject(animationManagerOfEnemy, "Undo animationManagerOfEnemy");

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
