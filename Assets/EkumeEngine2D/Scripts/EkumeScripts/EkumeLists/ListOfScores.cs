using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EkumeLists;
#endif

namespace EkumeLists
{
    [Serializable]
    public class ListOfScores
    {
        public int Index;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ListOfScores))]
class ListOfScoresDrawer : PropertyDrawer
{
	//private readonly int[] primes = {0, 1, 2};
	private readonly string[] listToShow;
	
    public ListOfScoresDrawer()
    {
        listToShow = ScoresList();
    }

    string[] ScoresList()
    {
        string[] ScoreList = new string[ScoreTypesManager.instance.ScoresData.Count];

        for (int i = 0; i < ScoreList.Length; i++)
        {
            ScoreList[i] = ScoreTypesManager.instance.ScoresData[i].scoreName;
        }

        return ScoreList;
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