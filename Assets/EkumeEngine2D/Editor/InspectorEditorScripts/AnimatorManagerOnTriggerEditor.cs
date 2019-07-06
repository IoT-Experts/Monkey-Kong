using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEditor.Animations;

[CustomEditor(typeof(AnimatorManagerOnTrigger))]
public class AnimatorManagerOnTriggerEditor : Editor
{
    AnimatorManagerOnTrigger animationManagerOnTrigger;
    AnimatorController animatorController;

    void OnEnable()
    {
        animationManagerOnTrigger = (AnimatorManagerOnTrigger)target;
        if (animationManagerOnTrigger != null)
        {
            if (animationManagerOnTrigger.currentAnimator == null)
            {
                animationManagerOnTrigger.currentAnimator = animationManagerOnTrigger.gameObject.GetComponent<Animator>();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        if (animationManagerOnTrigger != null)
        {
            EditorGUILayout.HelpBox("When this object enters in the trigger of any object with specific tag, it will send to some parameter the value of true while is in the trigger.", MessageType.None);

            if (animationManagerOnTrigger.currentAnimator.runtimeAnimatorController != null)
            {
                animatorController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(animationManagerOnTrigger.currentAnimator.runtimeAnimatorController), typeof(AnimatorController)) as AnimatorController;

                using (new GUILayout.VerticalScope("box"))
                {
                    for (int i = 0; i < animationManagerOnTrigger.parameterNames.Count; i++)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 70;
                            animationManagerOnTrigger.parameterNames[i] = EditorGUILayout.TextField("Parameter: ", animationManagerOnTrigger.parameterNames[i]);
                            EditorGUIUtility.labelWidth = 42;
                            animationManagerOnTrigger.tags[i] = EditorGUILayout.TagField("Tag: ", animationManagerOnTrigger.tags[i]);
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                animationManagerOnTrigger.parameterNames.RemoveAt(i);
                                animationManagerOnTrigger.tags.RemoveAt(i);
                                break;
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (animationManagerOnTrigger.parameterNames[i] != "")
                        {
                            if (!IsStringInArray(ParametersOfAnimator(animatorController), animationManagerOnTrigger.parameterNames[i]))
                            {
                                EditorGUILayout.HelpBox("The parameter does not exist in the animator controller.", MessageType.Warning);
                            }
                            else if (!IsBooleanType(ParametersOfAnimator(animatorController), animationManagerOnTrigger.parameterNames[i]))
                            {
                                EditorGUILayout.HelpBox("The parameter should be of type bool.", MessageType.Warning);
                            }
                        }

                    }
                    if (GUILayout.Button("Add new state control"))
                    {
                        animationManagerOnTrigger.parameterNames.Add("");
                        animationManagerOnTrigger.tags.Add("");
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

            if (animationManagerOnTrigger.GetComponent<Rigidbody2D>() != null && animationManagerOnTrigger.GetComponent<Rigidbody2D>().collisionDetectionMode != CollisionDetectionMode2D.Continuous)
            {
                EditorGUILayout.HelpBox("If you have some problem with the collision (trigger) detection, please select the \'Continuous\' mode in the Collision Detection of your Rigidbody2D.", MessageType.Info);
            }

            EditorUtility.SetDirty(animationManagerOnTrigger);
            Undo.RecordObject(animationManagerOnTrigger, "Undo animationManagerOnCollision");
        }
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
