using UnityEditor;
using EkumeLists;

[CustomEditor(typeof(ListOfClothingCategories))]
class ListOfClothingCategoriesEditor : Editor
{
	private SerializedProperty property;
	void OnEnable()
	{
		property = serializedObject.FindProperty("ListOfClothingCategories");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.PropertyField(property);
	}
}