using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LevelEditor_Editor : EditorWindow
{
    LevelEditor levelEditor;
    Vector2 scrollPos;
    Grid grid;

    GameObject goSelected;
    float zPosToInstantiate;

    bool eraserSelected;
    bool instatiatorSelected;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Level Editor", "Content" };
    float height = 0;
    float width = 0;

    [MenuItem("Tools/Window/Level Editor")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(LevelEditor_Editor), false, "Level Editor");
    }

    void OnEnable()
    {
        levelEditor = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/LevelEditor.asset", typeof(LevelEditor)) as LevelEditor;

        if (FindObjectOfType<Grid>() != null)
        {
            grid = FindObjectOfType<Grid>();
        }
        else
        {
            GameObject newGO = new GameObject();
            newGO.AddComponent<Grid>();
            newGO.transform.position = new Vector3(0, 0, 0);
            grid = newGO.GetComponent<Grid>();
            newGO.name = "GRID";
        }

        SceneView.onSceneGUIDelegate += GridUpdate;
    }

    public void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= GridUpdate;
    }

    void GridUpdate(SceneView sceneview)
    {
        Event _event= Event.current;

        Ray r = Camera.current.ScreenPointToRay(new Vector3(_event.mousePosition.x, -_event.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = r.origin;

        if ((EditorPrefs.GetBool("InstantiateWithClick", true) && _event.type == EventType.MouseDown && _event.button == 0) && instatiatorSelected)
        {
            GameObject obj;

            if (LevelEditor.currentTool != LevelEditor.GetCurrentTool())
            {
                goSelected = null;
            }

            if (goSelected != null)
            {
                LevelEditor.ChangeTool();

                obj = (GameObject)PrefabUtility.InstantiatePrefab(goSelected);

                Vector3 aligned;
                if (EditorPrefs.GetBool("FitWithGrid", false))
                {
                    aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                                  Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, (!levelEditor.zPositionOfPrefab) ? zPosToInstantiate : goSelected.transform.position.z);

                }
                else
                {
                    aligned = new Vector3(mousePos.x, mousePos.y, (!levelEditor.zPositionOfPrefab) ? zPosToInstantiate : goSelected.transform.position.z);
                }

                obj.transform.position = aligned;

                Selection.activeGameObject = obj;
                Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            }
        }
    }

    void OnGUI()
    {
	    if(levelEditor.categories.Count == 0)
        {
            levelEditor.categories.Add(new Category());
            Debug.LogWarning("You should have at least one category. Was added one category automatically.");
        }
		
        if (EditorPrefs.GetBool("FitWithGrid", false) && grid.width > 0 && grid.height > 0)
        {
            if (Selection.activeTransform != null)
            {
                GameObject[] selection = Selection.gameObjects;

                for(int i = 0; i < selection.Length; i++)
                {
                    selection[i].transform.position = new Vector3(Mathf.Floor(selection[i].transform.position.x / grid.width) * grid.width + grid.width / 2.0f,
                                                      Mathf.Floor(selection[i].transform.position.y / grid.height) * grid.height + grid.height / 2.0f, selection[i].transform.position.z);

                    Undo.RecordObject(selection[i].transform, "Moved " + selection[i].name);
                }
            }
        }

        if (FindObjectOfType<Grid>() != null)
        {
            grid = FindObjectOfType<Grid>();
        }
        else
        {
            GameObject newGO = new GameObject();
            newGO.AddComponent<Grid>();
            newGO.transform.position = new Vector3(0, 0, 0);
            grid = newGO.GetComponent<Grid>();
            newGO.name = "GRID";
        }

        //-------------------------------------------------------------------------------------- /
        //Variables for scroll
        width = this.position.width;
        height = this.position.height;

        selectedMenu = GUILayout.Toolbar(selectedMenu, toolbarOptions, EditorStyles.toolbarButton);

        switch (selectedMenu)
        {
            case 0:
                ListOfGameObjects();
                break;
            case 1:
                Content();
                break;
        }
        
        EditorUtility.SetDirty(levelEditor);
        Undo.RecordObject(levelEditor, "Undo fasterLevelMaker");

        if (grid != null)
        {
            EditorUtility.SetDirty(grid);
            Undo.RecordObject(grid, "Undo grid");
        }
    }


    void OnInspectorUpdate()
    {
        Repaint();
        SceneView.RepaintAll();
    }

    void ListOfGameObjects()
    {
        ToolsLevelEditor(); //Show the tools of the level editor

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(width), GUILayout.Height(height - 82));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();
        if (levelEditor.categories.Count > 0)
        {
            if(levelEditor.categories.Count <= EditorPrefs.GetInt("CategorySelected", 0))
            {
                EditorPrefs.SetInt("CategorySelected", 0);
            }

            if (levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].subCategory.Count < EditorPrefs.GetInt("SubcategorySelected"))
            {
                EditorPrefs.SetInt("SubcategorySelected", 0);
            }

            if (levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].divideInSubcategory)
            {
                if (levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].subCategory.Count > 0)
                {
                    ShowObjectsList(levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].subCategory[EditorPrefs.GetInt("SubcategorySelected")].objects);
                }
                else
                {
                    Debug.LogWarning("You cannot select the category " + levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].name + " because you do not have any object added here.");
                    EditorPrefs.SetInt("CategorySelected", 0);
                }
            }
            else
            {
                ShowObjectsList(levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].objects);
            }
        }
        else
        {
            EditorGUIUtility.labelWidth = 300;
            EditorGUILayout.LabelField("Plese create any category from the content page.", EditorStyles.boldLabel);
        }
        EditorGUILayout.EndVertical();


        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void ToolsLevelEditor ()
    {
        CategorySelection();
        GridOptions();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 20;

        GUI.backgroundColor = Color.white;
        //Options of category, subcategory, fit selection with grid
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            if (instatiatorSelected)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.white;

            if (GUILayout.Button("INSTANTIATOR", EditorStyles.miniButtonLeft, GUILayout.Height(25), GUILayout.Width(100)))
            {
                LevelEditor.ChangeTool();

                if (instatiatorSelected)
                {
                    instatiatorSelected = false;
                    goSelected = null;
                }
                else
                {
                    instatiatorSelected = true;
                }

                eraserSelected = false;
            }

            if (goSelected == null)
                GUI.backgroundColor = Color.cyan;
            else
                GUI.backgroundColor = Color.white;

            if (GUILayout.Button("UNSELECT", EditorStyles.miniButtonMid, GUILayout.Width(75), GUILayout.Height(25)))
            {
                goSelected = null;
            }            

            if (eraserSelected)
                GUI.backgroundColor = Color.red;
            else
                GUI.backgroundColor = Color.white;

            if (GUILayout.Button("ERASER", EditorStyles.miniButtonRight, GUILayout.Width(75), GUILayout.Height(25)))
            {
                Selection.activeGameObject = null;

                instatiatorSelected = false;

                if (eraserSelected)
                    eraserSelected = false;
                else
                    eraserSelected = true;

                goSelected = null;
            }

            GUI.backgroundColor = Color.white;

            if (eraserSelected)
            {
                foreach (GameObject go in Selection.gameObjects)
                {
                    if (go.GetComponent<Grid>() == null)
                        Undo.DestroyObjectImmediate(go);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();


        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void ShowObjectsList(List<ObjectOfCategory> objects)
    {
        using (new GUILayout.VerticalScope("helpbox"))
        {
            int numberOfObject = 1;
            while (numberOfObject < objects.Count)
            {
                EditorGUILayout.BeginHorizontal();

                for (int i = numberOfObject; i < objects.Count; i++)
                {
                    if (goSelected == objects[i].obj)
                        GUI.backgroundColor = Color.cyan;
                    else
                        GUI.backgroundColor = Color.white;

                  //  using (new GUILayout.VerticalScope("box", GUILayout.Width(65), GUILayout.Height(65)))
                   // {
                        if (objects[i].obj != null)
                        {
                            Texture2D goPreview = AssetPreview.GetAssetPreview(objects[i].obj);
                            if (GUILayout.Button(goPreview, EditorStyles.miniButtonMid, GUILayout.Height(70), GUILayout.Width(70)))
                            {
                                goSelected = objects[i].obj;
                                zPosToInstantiate = objects[i].zPos;
                                EditorGUIUtility.PingObject(goSelected);
                                LevelEditor.currentTool = LevelEditor.GetCurrentTool();
                            }
                            EditorGUIUtility.labelWidth = 20;
                         //   EditorGUILayout.HelpBox(objects[i].obj.name, MessageType.None);
                     // }
                    }

                    numberOfObject++;

                    if (i % EditorPrefs.GetInt("ColumnsToShowObjects") == 0)
                    {
                        break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            GUI.backgroundColor = Color.white;
        }
    }

    void Content ()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(400)))
        {
            EditorGUIUtility.labelWidth = 250;
            levelEditor.zPositionOfPrefab = EditorGUILayout.Toggle("Instantiate in the Z position of the prefab: ", levelEditor.zPositionOfPrefab);

        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(width), GUILayout.Height(height - 53));
        EditorGUIUtility.labelWidth = 0;

        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < levelEditor.categories.Count; i++)
        {
            using (new GUILayout.VerticalScope(levelEditor.categories[i].name, "window", GUILayout.Width(400), GUILayout.Height(100)))
            {
                using (new GUILayout.HorizontalScope("box"))
                {
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                    {
                        levelEditor.categories.RemoveAt(i);
                        break;
                    }
                    EditorGUIUtility.labelWidth = 100;
                    levelEditor.categories[i].name = EditorGUILayout.TextField("Category name: ", levelEditor.categories[i].name);
                    EditorGUIUtility.labelWidth = 150;
                    levelEditor.categories[i].divideInSubcategory = EditorGUILayout.Toggle("Divide in subcategories: ", levelEditor.categories[i].divideInSubcategory);
                }

                if (levelEditor.categories[i].divideInSubcategory)
                {
                    EditorGUILayout.Space();
                    for (int j = 0; j < levelEditor.categories[i].subCategory.Count; j++)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                            {
                                levelEditor.categories[i].subCategory.RemoveAt(j);
                                break;
                            }

                            EditorGUIUtility.labelWidth = 100;
                            levelEditor.categories[i].subCategory[j].name = EditorGUILayout.TextField("Subcategory: ", levelEditor.categories[i].subCategory[j].name);

                            EditorGUILayout.EndHorizontal();
                            using (new GUILayout.VerticalScope("box"))
                            {
                                for (int k = 1; k < levelEditor.categories[i].subCategory[j].objects.Count; k++)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    if (k != 1)
                                    {
                                        if (GUILayout.Button("▲", EditorStyles.miniButton, GUILayout.Width(20)))
                                        {
                                            ObjectOfCategory aboveObject = levelEditor.categories[i].subCategory[j].objects[k - 1];
                                            levelEditor.categories[i].subCategory[j].objects[k - 1] = levelEditor.categories[i].subCategory[j].objects[k];
                                            levelEditor.categories[i].subCategory[j].objects[k] = aboveObject;
                                        }
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));
                                    }

                                    if (k != levelEditor.categories[i].subCategory[j].objects.Count - 1)
                                    {
                                        if (GUILayout.Button("▼", EditorStyles.miniButton, GUILayout.Width(20)))
                                        {
                                            ObjectOfCategory belowObject = levelEditor.categories[i].subCategory[j].objects[k + 1];
                                            levelEditor.categories[i].subCategory[j].objects[k + 1] = levelEditor.categories[i].subCategory[j].objects[k];
                                            levelEditor.categories[i].subCategory[j].objects[k] = belowObject;
                                        }
                                    }
                                    else
                                    {
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));
                                    }

                                    EditorGUIUtility.labelWidth = 70;
                                    levelEditor.categories[i].subCategory[j].objects[k].obj = EditorGUILayout.ObjectField("Object " + k + ": ", levelEditor.categories[i].subCategory[j].objects[k].obj, typeof(GameObject), false, GUILayout.Width(225)) as GameObject;

                                    if (!levelEditor.zPositionOfPrefab)
                                    {
                                        EditorGUIUtility.labelWidth = 42;
                                        levelEditor.categories[i].subCategory[j].objects[k].zPos = EditorGUILayout.FloatField("Z pos: ", levelEditor.categories[i].subCategory[j].objects[k].zPos, GUILayout.Width(75));
                                    }

                                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                                    {
                                        levelEditor.categories[i].subCategory[j].objects.RemoveAt(k);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }

                                if (levelEditor.categories[i].subCategory[j].objects.Count < 50)
                                {
                                    if (GUILayout.Button("Add object"))
                                    {
                                        levelEditor.categories[i].subCategory[j].objects.Add(new ObjectOfCategory(null, 0));
                                    }
                                }
                                else
                                {
                                    EditorGUILayout.HelpBox("Limit of objects. Please create another category or subcategory to add more objects.", MessageType.Info);
                                }
                            }
                        }
                        EditorGUILayout.Space();
                    }

                    if (GUILayout.Button("Add subcategory of " + levelEditor.categories[i].name))
                    {
                        levelEditor.categories[i].subCategory.Add(new Subcategory());
                    }
                }
                else
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        for (int j = 1; j < levelEditor.categories[i].objects.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            if (j != 1)
                            {
                                if (GUILayout.Button("▲", GUILayout.Width(20)))
                                {
                                    ObjectOfCategory aboveObject = levelEditor.categories[i].objects[j - 1];
                                    levelEditor.categories[i].objects[j - 1] = levelEditor.categories[i].objects[j];
                                    levelEditor.categories[i].objects[j] = aboveObject;
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField("", GUILayout.Width(20));
                            }

                            if (j != levelEditor.categories[i].objects.Count - 1)
                            {
                                if (GUILayout.Button("▼", GUILayout.Width(20)))
                                {
                                    ObjectOfCategory belowObject = levelEditor.categories[i].objects[j + 1];
                                    levelEditor.categories[i].objects[j + 1] = levelEditor.categories[i].objects[j];
                                    levelEditor.categories[i].objects[j] = belowObject;
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField("", GUILayout.Width(20));
                            }

                            EditorGUIUtility.labelWidth = 70;
                            levelEditor.categories[i].objects[j].obj = EditorGUILayout.ObjectField("Object " + j + ": ", levelEditor.categories[i].objects[j].obj, typeof(GameObject), false, GUILayout.Width(225)) as GameObject;

                            if (!levelEditor.zPositionOfPrefab)
                            {
                                EditorGUIUtility.labelWidth = 42;
                                levelEditor.categories[i].objects[j].zPos = EditorGUILayout.FloatField("Z pos: ", levelEditor.categories[i].objects[j].zPos, GUILayout.Width(75));
                            }

                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                            {
                                levelEditor.categories[i].objects.RemoveAt(j);
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add object"))
                        {
                            levelEditor.categories[i].objects.Add(new ObjectOfCategory(null, 0));
                        }
                    }
                }
            }

            EditorGUILayout.Space();
        }
        if (GUILayout.Button("Add new category"))
        {
            levelEditor.categories.Add(new Category());
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    void CategorySelection ()
    {
        GUI.backgroundColor = Color.cyan;
        using (new GUILayout.HorizontalScope("toolbar"))
        {
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.BeginHorizontal();
            EditorPrefs.SetInt("CategorySelected", EditorGUILayout.Popup("Category: ", EditorPrefs.GetInt("CategorySelected", 0), ListOfCategories(levelEditor.categories), EditorStyles.toolbarPopup, GUILayout.Width(210)));
            if (levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)].divideInSubcategory)
            {
                EditorPrefs.SetInt("SubcategorySelected", EditorGUILayout.Popup(EditorPrefs.GetInt("SubcategorySelected", 0), ListOfSubCategories(levelEditor.categories[EditorPrefs.GetInt("CategorySelected", 0)]), EditorStyles.toolbarPopup, GUILayout.Width(150)));
            }


            EditorGUIUtility.labelWidth = 40;
            EditorPrefs.SetInt("ColumnsToShowObjects",
                EditorGUILayout.IntField("Cols: ", EditorPrefs.GetInt("ColumnsToShowObjects", 100), EditorStyles.toolbarTextField));

            if (EditorPrefs.GetInt("ColumnsToShowObjects") == 0)
            {
                EditorPrefs.SetInt("ColumnsToShowObjects", 1);
            }

            EditorGUILayout.EndHorizontal();
        }
        GUI.backgroundColor = Color.white;
    }

    void GridOptions()
    {
        using (new GUILayout.VerticalScope("toolbar"))
        {
            EditorGUIUtility.labelWidth = 70;
            EditorGUILayout.BeginHorizontal();
            grid.showGrid = EditorGUILayout.Toggle("Show Grid: ", grid.showGrid, GUILayout.Width(90));

            if (grid.showGrid)
            {
                EditorGUIUtility.labelWidth = 45;
                grid.height = EditorGUILayout.FloatField("Height", grid.height, EditorStyles.toolbarTextField, GUILayout.Width(80));
                grid.width = EditorGUILayout.FloatField("Width", grid.width, EditorStyles.toolbarTextField, GUILayout.Width(80));
                grid.color = EditorGUILayout.ColorField("Color", grid.color, GUILayout.Width(100));
            }
            else
            {
                EditorPrefs.SetBool("FitWithGrid", false);
            }

            if (grid.showGrid)
            {
                EditorGUIUtility.labelWidth = 130;
                EditorPrefs.SetBool("FitWithGrid", EditorGUILayout.Toggle("Fit selection with grid", EditorPrefs.GetBool("FitWithGrid", true), GUILayout.Width(150)));
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    string[] ListOfCategories (List<Category> categoryList)
    {
        string[] arrayToReturn = new string[categoryList.Count];

        for(int i = 0; i < categoryList.Count; i++)
        {
            arrayToReturn[i] = categoryList[i].name;
        }

        return arrayToReturn;
    }

    string[] ListOfSubCategories(Category category)
    {
        string[] arrayToReturn = new string[category.subCategory.Count];

        for (int i = 0; i < category.subCategory.Count; i++)
        {
            arrayToReturn[i] = category.subCategory[i].name;
        }

        return arrayToReturn;
    }
}