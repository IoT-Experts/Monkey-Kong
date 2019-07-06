using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(InstantiateWithKey_OnTrigger))]

public class InstantiateWithKey_OnTriggerEditor : Editor
{

    InstantiateWithKey_OnTrigger instantiateWithKey;

    void OnEnable()
    {
        instantiateWithKey = (InstantiateWithKey_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 165;
            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                instantiateWithKey.tagThatActivates = EditorGUILayout.TagField("Tag to activate: ", instantiateWithKey.tagThatActivates);
                EditorGUILayout.LabelField("*When it is triggered with this tag, the player can press the input.", EditorStyles.miniLabel);
            }
                using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                instantiateWithKey.inputControl = EditorGUILayout.Popup("Input control: ", instantiateWithKey.inputControl, ConvertListStringToArray(instantiateWithKey.inputControlsManager.inputNames));
                instantiateWithKey.positionToInstantiate = EditorGUILayout.ObjectField("Position to instantiate: ", instantiateWithKey.positionToInstantiate, typeof(Transform), true) as Transform;
                instantiateWithKey.gameObjectToInstantiate = EditorGUILayout.ObjectField("GameObject to instantiate: ", instantiateWithKey.gameObjectToInstantiate, typeof(GameObject), true) as GameObject;
            }
            EditorGUIUtility.labelWidth = 190;

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                instantiateWithKey.destroyObjectWhenKeyIsRaised = EditorGUILayout.Toggle("Destroy object when key is up: ", instantiateWithKey.destroyObjectWhenKeyIsRaised);

                if (instantiateWithKey.instantiateWhilePress && instantiateWithKey.destroyObjectWhenKeyIsRaised)
                {
                    EditorGUILayout.LabelField("*It will be destroyed only the last instantiated object.");
                }
            }

            using (var verticalScope2 = new GUILayout.VerticalScope("box"))
            {
                instantiateWithKey.instantiateWhilePress = EditorGUILayout.Toggle("Instantiate while key is pressed: ", instantiateWithKey.instantiateWhilePress);

                if (instantiateWithKey.instantiateWhilePress)
                {
                    instantiateWithKey.timeToReinstantiate = EditorGUILayout.FloatField("Time to reinstantiate: ", instantiateWithKey.timeToReinstantiate);
                }
            }

        }

        EditorUtility.SetDirty(instantiateWithKey);
        Undo.RecordObject(instantiateWithKey, "Undo instantiateWithKey");
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
