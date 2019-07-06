using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(GameConfigs))]
public class GameConfigsEditor : Editor
{
    GameConfigs targetGameConfigs;

    void OnEnable()
    {
        targetGameConfigs = (GameConfigs)target;
    }

    public override void OnInspectorGUI()
    {
        if (targetGameConfigs.GetComponents<Component>().Length > 2)
        {
            EditorGUILayout.HelpBox("Is recommendable put this component in a GameObject without more components.", MessageType.Info);
        }

        if (targetGameConfigs.transform.childCount > 0)
        {
            EditorGUILayout.HelpBox("Is recommendable not add child to this GameObject while you are using this component.", MessageType.Info);
        }

        GameConfigs[] audioConfigs = FindObjectsOfType<GameConfigs>();
        if (audioConfigs.Length > 1)
        {
            EditorGUILayout.HelpBox("You have multiple GameConfigs in your scene. You can, and you should to use only one for all the configurations.", MessageType.Error);
        }

        GUI.contentColor = Color.cyan;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            targetGameConfigs.keepObjectInOtherScenes = EditorGUILayout.Toggle("Keep object in other scenes: ", targetGameConfigs.keepObjectInOtherScenes);
            GUI.contentColor = Color.white;
            if (targetGameConfigs.keepObjectInOtherScenes)
            {
                EditorGUILayout.HelpBox("This object will be deleted in the scenes who have the component 'GameConfigs' to load its corresponding configurations.", MessageType.Info);
            }
        }
       
        using (new GUILayout.VerticalScope("box"))
        {
            foreach (GameConfigCategory gameConf in targetGameConfigs.configCategories)
            {
                EditorGUILayout.BeginHorizontal();
                using (new GUILayout.VerticalScope(gameConf.categoryName, "window", GUILayout.Height(100)))
                {
                    EditorGUIUtility.labelWidth = 135;
                    gameConf.categoryName = EditorGUILayout.TextField("Category name: ", gameConf.categoryName);
                    gameConf.defaultValueOfThisCategory = (GameConfigCategory.DefaultValue)EditorGUILayout.EnumPopup("Default value: ", gameConf.defaultValueOfThisCategory);
                    gameConf.toggle = EditorGUILayout.ObjectField("Toggle UI: ", gameConf.toggle, typeof(Toggle), true) as Toggle;
                    EditorGUIUtility.labelWidth = 170;

                    using (new GUILayout.VerticalScope("helpbox"))
                    {
                        if (targetGameConfigs.keepObjectInOtherScenes)
                            EditorGUILayout.HelpBox("Here you should to add all the objects or components you want to manage, even those who are not in this scene, in this way, the configurations will be applied when the scene changes.", MessageType.None);

                        foreach (ObjectData obj in gameConf.objects)
                        {
                            EditorGUILayout.BeginHorizontal();
                            using (new GUILayout.VerticalScope("helpbox"))
                            {
                                obj.type = (ObjectData.Type)EditorGUILayout.EnumPopup("Type: ", obj.type);
                                if (obj.type == ObjectData.Type.GameObject)
                                {
                                    obj.name = EditorGUILayout.TextField("GameObject name: ", obj.name);

                                    GameObject objFound = GameObject.Find(obj.name);
                                    if (objFound == null && targetGameConfigs.keepObjectInOtherScenes)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.name + " does not exist in this scene, but it will be searched in the next scenes.", MessageType.Warning);
                                    }
                                    else if (objFound == null)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.name + " does not exist in this scene, if you want to search it when this scene is changed, enable the option 'Keep object in other scenes'.", MessageType.Error);
                                    }
                                }
                                else if(obj.type == ObjectData.Type.Component)
                                {
                                    obj.name = EditorGUILayout.TextField("Component name: ", obj.name);
                                    obj.gameObjectWithTheComponent = EditorGUILayout.TextField("Object with the component: ", obj.gameObjectWithTheComponent);

                                    GameObject objFound = GameObject.Find(obj.gameObjectWithTheComponent);
                                    if (objFound != null && objFound.GetComponent(obj.name) == null)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " exist, but the component " + obj.name + " is not part of the object." + ((targetGameConfigs.keepObjectInOtherScenes) ? " Anyway, it will be searched in the next scenes." : ""), MessageType.Warning);
                                    }
                                    else if (objFound == null && targetGameConfigs.keepObjectInOtherScenes)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " does not exist in this scene, but it will be searched in the next scenes.", MessageType.Warning);
                                    }
                                    else if (objFound == null)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " does not exist in this scene, if you want to search it when this scene is changed, enable the option 'Keep object in other scenes'.", MessageType.Error);
                                    }
                                }
                                else if(obj.type == ObjectData.Type.Script)
                                {
                                    obj.name = EditorGUILayout.TextField("Script name: ", obj.name);
                                    obj.gameObjectWithTheComponent = EditorGUILayout.TextField("Object with the script: ", obj.gameObjectWithTheComponent);

                                    GameObject objFound = GameObject.Find(obj.gameObjectWithTheComponent);

                                    if(objFound != null && objFound.GetComponent(obj.name) == null)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " exist, but the component " + obj.name + " is not part of the object." + ((targetGameConfigs.keepObjectInOtherScenes) ? " Anyway, it will be searched in the next scenes." : ""), MessageType.Warning);
                                    }
                                    else if (objFound != null && objFound.GetComponent(obj.name) != null)
                                    {
                                        Behaviour script = objFound.GetComponent(obj.name) as Behaviour;
                                        if (script == null)
                                        {
                                            EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " and the component " + obj.name + " exist, but the component is not of type Script. You can select the Type 'Component' but you can just destroy it.", MessageType.Error);
                                        }
                                    }
                                    else if(objFound == null && targetGameConfigs.keepObjectInOtherScenes)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " does not exist in this scene, but it will be searched in the next scenes.", MessageType.Warning);
                                    }
                                    else if(objFound == null)
                                    {
                                        EditorGUILayout.HelpBox("The object " + obj.gameObjectWithTheComponent + " does not exist in this scene, if you want to search it when this scene is changed, enable the option 'Keep object in other scenes'.", MessageType.Error);
                                    }
                                }

                                if(obj.type == ObjectData.Type.GameObject || obj.type == ObjectData.Type.Script)
                                {
                                    obj.action1OnEnable = (ObjectData.Actions1)EditorGUILayout.EnumPopup("When toggle is true: ", obj.action1OnEnable);
                                    obj.action1OnDisable = (ObjectData.Actions1)EditorGUILayout.EnumPopup("When toggle is false: ", obj.action1OnDisable);
                                }
                                else if(obj.type == ObjectData.Type.Component)
                                {
                                    obj.action2OnEnable = (ObjectData.Actions2)EditorGUILayout.EnumPopup("When toggle is true: ", obj.action2OnEnable);
                                    obj.action2OnDisable = (ObjectData.Actions2)EditorGUILayout.EnumPopup("When toggle is false: ", obj.action2OnDisable);
                                    EditorGUILayout.HelpBox("Components can be only destroyed. Enable or disable actions will not works.", MessageType.None);
                                }
                            }

                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(23)))
                            {
                                gameConf.objects.Remove(obj);
                                return;
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }
                        if (GUILayout.Button("Add new object", EditorStyles.miniButton))
                        {
                            gameConf.objects.Add(new ObjectData());
                            return;
                        }
                    }
                }
                if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(23)))
                {
                    targetGameConfigs.configCategories.Remove(gameConf);
                    return;
                }
                EditorGUILayout.EndHorizontal();

                using (new GUILayout.VerticalScope("helpbox")) { }
            }

            if(GUILayout.Button("Add new category", EditorStyles.miniButton))
            {
                targetGameConfigs.configCategories.Add(new GameConfigCategory());
            }
        }

        EditorUtility.SetDirty(targetGameConfigs);
        Undo.RecordObject(targetGameConfigs, "Undo gameConfigs");
    }
}
