﻿using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using EkumeLists;
#endif

namespace EkumeLists
{
    [Serializable]
    public class ListOfPlayers
    {
        public int Index;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListOfPlayers))]
class ListOfPlayersDrawer : PropertyDrawer
{
	//private readonly int[] primes = {0, 1, 2};
	private readonly string[] listToShow = new string[] { };
	
    public ListOfPlayersDrawer()
    {
        listToShow = ConvertListStringToArray(PlayersManager.instance.playerNames);
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

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
        // Need to wrap this is Begin/End Property so prefab override logic works (so says the docs: http://docs.unity3d.com/ScriptReference/PropertyDrawer.html)
        EditorGUI.BeginProperty(position, label, property);
		
		// Draw a nice prefix label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(property.name));

        // Create the rect for our field
        var rect = new Rect(position.x, position.y, position.width, position.height);
		
		// The property we want to edit is the 'Prime' field on the 'MyProperty' type. The
		// property passed into this method is an instance of the 'MyProperty' type on some
		// serialized object (e.g. a GameObject)
		var primeProperty = property.FindPropertyRelative("Index");
		// Draw a popup to select a prime
		//var newValue = EditorGUI.Popup (rect, 0, primes);
		var newValue = EditorGUI.Popup(rect, primeProperty.intValue, listToShow);
		// If the value has changed, apply it. This allows undo to work
		if (newValue != primeProperty.intValue)
		{
			primeProperty.intValue = newValue;
			primeProperty.serializedObject.ApplyModifiedProperties();
		}
		EditorGUI.EndProperty();
	}
}
#endif