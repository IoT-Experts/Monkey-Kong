using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfLevels))]
class ListOfLevelsEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfLevels");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}