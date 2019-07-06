﻿#if UNITY_EDITOR
//HideWhenFalseDrawer.cs 
//Should be placed inside an Editor folder. 
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(HideWhenTrueAttribute))]
public class HideWhenTrueDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideWhenTrueAttribute hiddenAttribute = attribute as HideWhenTrueAttribute;
        SerializedProperty boolProperty = property.serializedObject.FindProperty(hiddenAttribute.hideBoolean);

        if (!boolProperty.boolValue)
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideWhenTrueAttribute hiddenAttribute = attribute as HideWhenTrueAttribute;
        SerializedProperty boolProperty = property.serializedObject.FindProperty(hiddenAttribute.hideBoolean);

        if (boolProperty.boolValue)
            return 0f;

        return EditorGUI.GetPropertyHeight(property);
    }
}
#endif