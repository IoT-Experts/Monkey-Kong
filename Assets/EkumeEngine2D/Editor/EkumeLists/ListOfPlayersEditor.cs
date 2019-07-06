using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfPlayers))]
class ListOfPlayersEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfPlayers");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}