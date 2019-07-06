using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfWorlds))]
class ListOfWorldsEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfWorlds");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}