using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfInputControls))]
class ListOfInputControlsEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfInputControls");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}