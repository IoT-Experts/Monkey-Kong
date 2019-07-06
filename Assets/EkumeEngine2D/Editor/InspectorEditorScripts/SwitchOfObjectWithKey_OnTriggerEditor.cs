using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SwitchOfObjectWithKey_OnTrigger))]

public class SwitchOfObjectWithKey_OnTriggerEditor : Editor
{

    SwitchOfObjectWithKey_OnTrigger switchOfObjectWithKey;

    void OnEnable()
    {
        switchOfObjectWithKey = (SwitchOfObjectWithKey_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Enable or disable an object when press a key.", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("The player should be in the trigger to activate the action.", MessageType.None);
            using (var verticalScope0 = new GUILayout.VerticalScope("box"))
            {
                switchOfObjectWithKey.inputControl = EditorGUILayout.Popup("Input control: ", switchOfObjectWithKey.inputControl, convertListStringToArray(switchOfObjectWithKey.inputControlsManager.inputNames));
                switchOfObjectWithKey.objectToSwith = EditorGUILayout.ObjectField("Object to switch: ", switchOfObjectWithKey.objectToSwith, typeof(GameObject), true) as GameObject;

                if (!switchOfObjectWithKey.canReSwitch)
                {
                    switchOfObjectWithKey.turnOn = EditorGUILayout.Toggle("Enable: ", switchOfObjectWithKey.turnOn);
                    if (switchOfObjectWithKey.turnOn)
                    {
                        switchOfObjectWithKey.turnOff = false;
                        switchOfObjectWithKey.canReSwitch = false;
                    }
                    switchOfObjectWithKey.turnOff = EditorGUILayout.Toggle("Disable: ", switchOfObjectWithKey.turnOff);
                    if (switchOfObjectWithKey.turnOff)
                    {
                        switchOfObjectWithKey.turnOn = false;
                        switchOfObjectWithKey.canReSwitch = false;
                    }
                }
                switchOfObjectWithKey.canReSwitch = EditorGUILayout.Toggle("Enable/Disable: ", switchOfObjectWithKey.canReSwitch);
                if (switchOfObjectWithKey.canReSwitch)
                {
                    switchOfObjectWithKey.turnOff = false;
                    switchOfObjectWithKey.turnOn = false;
                }
            }
        }

        EditorUtility.SetDirty(switchOfObjectWithKey);
        Undo.RecordObject(switchOfObjectWithKey, "Undo switchOfObjectWithKey");
    }

    string[] convertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }

}
