using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEditor.Animations;

[CustomEditor(typeof(AnimatorManagerOnCollision))]
public class AnimatorManagerOnCollisionEditor : Editor
{
    AnimatorManagerOnCollision animationManagerOnCollision;
    AnimatorController animatorController;

    void OnEnable()
    {
        animationManagerOnCollision = (AnimatorManagerOnCollision)target;

        if (animationManagerOnCollision.currentAnimator == null)
        {
            animationManagerOnCollision.currentAnimator = animationManagerOnCollision.gameObject.GetComponent<Animator>();
        }
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("When this object collides with any specific tag, it will send to some parameter the value of true while is colliding.", MessageType.None);

        if (animationManagerOnCollision.currentAnimator.runtimeAnimatorController != null)
        {
            animatorController = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(animationManagerOnCollision.currentAnimator.runtimeAnimatorController), typeof(AnimatorController)) as AnimatorController;

            using (new GUILayout.VerticalScope("box"))
            {
                for (int i = 0; i < animationManagerOnCollision.parameterNames.Count; i++)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUIUtility.labelWidth = 70;
                        animationManagerOnCollision.parameterNames[i] = EditorGUILayout.TextField("Parameter: ", animationManagerOnCollision.parameterNames[i]);
                        EditorGUIUtility.labelWidth = 42;
                        animationManagerOnCollision.tags[i] = EditorGUILayout.TagField("Tag: ", animationManagerOnCollision.tags[i]);
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            animationManagerOnCollision.parameterNames.RemoveAt(i);
                            animationManagerOnCollision.tags.RemoveAt(i);
                            break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (animationManagerOnCollision.parameterNames[i] != "")
                    {
                        if (!IsStringInArray(ParametersOfAnimator(animatorController), animationManagerOnCollision.parameterNames[i]))
                        {
                            EditorGUILayout.HelpBox("The parameter does not exist in the animator controller.", MessageType.Warning);
                        }
                        else if (!IsBooleanType(ParametersOfAnimator(animatorController), animationManagerOnCollision.parameterNames[i]))
                        {
                            EditorGUILayout.HelpBox("The parameter should be of type bool.", MessageType.Warning);
                        }
                    }

                }
                if (GUILayout.Button("Add new state control"))
                {
                    animationManagerOnCollision.parameterNames.Add("");
                    animationManagerOnCollision.tags.Add("");
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

        if(animationManagerOnCollision.GetComponent<Rigidbody2D>().collisionDetectionMode != CollisionDetectionMode2D.Continuous)
        {
            EditorGUILayout.HelpBox("If you have some problem with the collision detection, please select the \'Continuous\' mode in the Collision Detection of your Rigidbody2D.", MessageType.Info);
        }

        EditorUtility.SetDirty(animationManagerOnCollision);
        Undo.RecordObject(animationManagerOnCollision, "Undo animationManagerOnCollision");

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
