using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ComponentsManagerEditor : EditorWindow
{
    ComponentsManager componentsManager;

    Vector2 scrollPos;
    string textToSearch = "";
    int categoryToFilter;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Components", "Edit information", "Categories" };

    Texture2D scriptIcon;

    [MenuItem("Tools/Window/Components Manager")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(ComponentsManagerEditor), false, "Components Manager");
    }

    void OnEnable()
    {
        componentsManager = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/ComponentsManager.asset", typeof(ComponentsManager)) as ComponentsManager;
    }

    void OnInspectorUpdate()
    {
        autoRepaintOnSceneChange = true;
        Repaint();
    }

    void OnGUI()
    {
        //-------------------------------------------------------------------------------------- /
        //Variables for scroll
        float width = this.position.width;
        float height = this.position.height;

        //Start scroll view
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(width), GUILayout.Height(height));
        //-------------------------------------------------------------------------------------- /

        selectedMenu = GUILayout.Toolbar(selectedMenu, toolbarOptions, EditorStyles.toolbarButton);

        switch (selectedMenu)
        {
            case 0:
                Components();
                break;
            case 1:
                EditInformation();
                break;
            case 2:
                Categories();
                break;
        }

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(componentsManager);
        Undo.RecordObject(componentsManager, "Undo componentsManager");
    }

    void Components ()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("Components", "window", GUILayout.Width(300)))
        {
            EditorGUIUtility.labelWidth = 64;
            EditorGUILayout.BeginHorizontal();

            textToSearch = EditorGUILayout.TextField("Search: ", textToSearch, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(280));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared
                textToSearch = "";
                GUI.FocusControl(null);
            }

            categoryToFilter = EditorGUILayout.Popup("Category: ", categoryToFilter, ConvertListStringToArray(componentsManager.categories), GUILayout.Width(240));
            EditorGUILayout.EndHorizontal();

            if (Selection.activeGameObject == null)
                EditorGUILayout.LabelField("*Select a GameObject to add some component of the list.", EditorStyles.boldLabel);

            for (int i = 0; i < componentsManager.components.Count; i++)
            {
                if (componentsManager.components[i].script != null)
                {
                    string nameInLowercase = componentsManager.components[i].script.name.ToLower();
                    string descriptionInLowercase = componentsManager.components[i].description.ToLower();
                    if (((nameInLowercase.Contains(textToSearch) || componentsManager.components[i].script.name.Contains(textToSearch)
                        && textToSearch != ""
                        || componentsManager.components[i].description.Contains(textToSearch) || descriptionInLowercase.Contains(textToSearch))
                        && (componentsManager.components[i].category == categoryToFilter || categoryToFilter == 0)))
                    {
                        using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(600)))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 250;
                            scriptIcon = AssetPreview.GetMiniThumbnail(componentsManager.components[i].script);
                            GUILayout.Label((scriptIcon), GUILayout.Height(25), GUILayout.Width(25));
                           
                            if (GUILayout.Button(componentsManager.components[i].script.name, EditorStyles.boldLabel))
                            {
                                EditorGUIUtility.PingObject(componentsManager.components[i].script);
                            }

                            if (Selection.activeGameObject != null)
                            {
                                if (GUILayout.Button("Add component", GUILayout.Width(130)))
                                {
                                    Selection.activeGameObject.AddComponent(componentsManager.components[i].script.GetClass());
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 510;

                            if (componentsManager.components[i].description != null && componentsManager.components[i].description != "")
                            {
                                EditorGUILayout.HelpBox(componentsManager.components[i].description, MessageType.None);
                            }

                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginHorizontal();

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void EditInformation()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Edit Information", "window", GUILayout.Width(300)))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 80;
            textToSearch = EditorGUILayout.TextField("Search: ", textToSearch, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(280));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared
                textToSearch = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 1; i < componentsManager.categories.Count; i++)
            {
                using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(460)))
                {
                    EditorGUILayout.LabelField(componentsManager.categories[i], EditorStyles.boldLabel);
                    for (int j = 0; j < componentsManager.components.Count; j++)
                    {
                        if (componentsManager.components[j].category == i)
                        {
                            string nameInLowercase = "";
                            if (componentsManager.components[j].script != null)
                                nameInLowercase = componentsManager.components[j].script.name.ToLower();

                            string normalScriptName = "";
                            if (componentsManager.components[j].script != null)
                                normalScriptName = componentsManager.components[j].script.name;

                            string descriptionInLowercase = componentsManager.components[j].description.ToLower();
                            if ((nameInLowercase.Contains(textToSearch) || normalScriptName.Contains(textToSearch)
                                && textToSearch != ""
                                || componentsManager.components[j].description.Contains(textToSearch) || descriptionInLowercase.Contains(textToSearch))
                                && (componentsManager.components[j].category == categoryToFilter || categoryToFilter == 0))
                            {
                                using (new GUILayout.VerticalScope("helpbox"))
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField(j.ToString(), GUILayout.Width(25));
                                    if (j != 0)
                                    {
                                        if (GUILayout.Button("▲", EditorStyles.miniButton, GUILayout.Width(20)))
                                        {
                                            ComponentInfo aboveObject = componentsManager.components[j - 1];
                                            componentsManager.components[j - 1] = componentsManager.components[j];
                                            componentsManager.components[j] = aboveObject;
                                        }
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));
                                    }


                                    if (j != componentsManager.components.Count - 1)
                                    {
                                        if (GUILayout.Button("▼", EditorStyles.miniButton, GUILayout.Width(20)))
                                        {
                                            ComponentInfo belowObject = componentsManager.components[j + 1];
                                            componentsManager.components[j + 1] = componentsManager.components[j];
                                            componentsManager.components[j] = belowObject;
                                        }
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));
                                    }

                                    EditorGUIUtility.labelWidth = 55;
                                    componentsManager.components[j].script = EditorGUILayout.ObjectField("Scirpt: ", componentsManager.components[j].script, typeof(MonoScript), false, GUILayout.Width(210)) as MonoScript;
                                    EditorGUIUtility.labelWidth = 105;
                                    componentsManager.components[j].description = EditorGUILayout.TextField("Description: ", componentsManager.components[j].description, GUILayout.Width(350));

                                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                                    {
                                        componentsManager.components.RemoveAt(j);
                                        break;
                                    }

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUIUtility.labelWidth = 70;
                                    EditorGUILayout.LabelField("", GUILayout.Width(44));
                                    componentsManager.components[j].category = EditorGUILayout.Popup("Category: ", componentsManager.components[j].category, ConvertListStringToArray(componentsManager.categories), GUILayout.Width(215));

                                    if (componentsManager.components[j].category == 0)
                                    {
                                        componentsManager.components[j].category = i;
                                        Debug.LogWarning("You can't select this category. Please select other.");
                                    }

                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }
                    }


                    if (GUILayout.Button("Add new component information"))
                    {
                        componentsManager.components.Add(new ComponentInfo(null, "", i));
                    }
                }
                EditorGUILayout.Space();
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void Categories()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Categories","window", GUILayout.Width(300), GUILayout.Height(30)))
        {
            for (int i = 0; i < componentsManager.categories.Count; i++)
            {
                using (new GUILayout.HorizontalScope("box", GUILayout.Width(300)))
                {
                    EditorGUIUtility.labelWidth = 80;
                    componentsManager.categories[i] = EditorGUILayout.TextField("Category " + i + ":", componentsManager.categories[i]);

                    if (i != 0)
                    {
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            int components = componentsManager.components.Count;
                            for (int j = components - 1; j >= 0; j--)
                            {
                                if (componentsManager.components[j].category == i)
                                {
                                    componentsManager.components.RemoveAt(j);
                                }
                            }

                            for(int k = 0; k < componentsManager.components.Count; k++)
                            {
                                if(componentsManager.components[k].category >= i)
                                {
                                    componentsManager.components[k].category--;
                                }
                            }

                            componentsManager.categories.RemoveAt(i);
                        }
                    }
                }
            }

            if (GUILayout.Button("Add category"))
            {
                componentsManager.categories.Add("");
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    string[] ConvertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }
}