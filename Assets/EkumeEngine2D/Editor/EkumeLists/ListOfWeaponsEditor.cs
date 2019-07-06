using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfWeapons))]
class ListOfWeaponsEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfWeapons");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}