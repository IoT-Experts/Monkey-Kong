using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfScores))]
class ListOfScoresEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfScores");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}