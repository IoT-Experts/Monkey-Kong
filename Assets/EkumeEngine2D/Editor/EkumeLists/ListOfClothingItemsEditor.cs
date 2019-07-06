using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfClothingItems))]
class ListOfClothingItemsEditor : Editor
{
	private SerializedProperty property;
	
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfClothingItems");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}