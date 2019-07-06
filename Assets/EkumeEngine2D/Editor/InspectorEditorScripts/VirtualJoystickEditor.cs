using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(VirtualJoystick))]

public class VirtualJoystickEditor : Editor
{

    VirtualJoystick virtualJoystick;

    void OnEnable()
    {
        virtualJoystick = (VirtualJoystick)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            virtualJoystick.useAxisXPositive = EditorGUILayout.Toggle("Use axis X+: ", virtualJoystick.useAxisXPositive);
            if (virtualJoystick.useAxisXPositive)
            {
                virtualJoystick.inputControlRight = EditorGUILayout.Popup("Input control X+: ", virtualJoystick.inputControlRight, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
            }

            virtualJoystick.useAxisXNegative = EditorGUILayout.Toggle("Use axis X-: ", virtualJoystick.useAxisXNegative);
            if (virtualJoystick.useAxisXNegative)
            {
                virtualJoystick.inputControlLeft = EditorGUILayout.Popup("Input control X-: ", virtualJoystick.inputControlLeft, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
            }
        }

        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            virtualJoystick.useAxisYPositive = EditorGUILayout.Toggle("Use axis Y+: ", virtualJoystick.useAxisYPositive);
            if (virtualJoystick.useAxisYPositive)
            {
                virtualJoystick.inputControlUp = EditorGUILayout.Popup("Input control Y+: ", virtualJoystick.inputControlUp, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
            }

            virtualJoystick.useAxisYNegative = EditorGUILayout.Toggle("Use axis Y-: ", virtualJoystick.useAxisYNegative);
            if (virtualJoystick.useAxisYNegative)
            {
                virtualJoystick.inputControlDown = EditorGUILayout.Popup("Input control Y-: ", virtualJoystick.inputControlDown, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
            }
        }

        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            virtualJoystick.joystickDeadZone = EditorGUILayout.FloatField("Joystick dead zone: ", virtualJoystick.joystickDeadZone);
        }

        EditorUtility.SetDirty(virtualJoystick);
        Undo.RecordObject(virtualJoystick, "Undo virtualJoystick");
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
