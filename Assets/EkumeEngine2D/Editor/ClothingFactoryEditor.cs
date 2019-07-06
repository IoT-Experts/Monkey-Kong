using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ClothingFactoryEditor : EditorWindow
{
    ClothingFactory clothingFactory;

    Vector2 scrollPos;
    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Clothing items", "Clothing categories" };

    [MenuItem("Tools/Window/Clothing factory")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(ClothingFactoryEditor), false, "Clothing factory");
    }

    void OnEnable()
    {
        clothingFactory = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/ClothingFactory.asset", typeof(ClothingFactory)) as ClothingFactory;
    }

    void OnInspectorUpdate()
    {
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
                ClothingItems();
                break;
            case 1:
                Categories(); 
                break;
        }

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(clothingFactory);
        Undo.RecordObject(clothingFactory, "Undo clothingFactory");
    }

    void Categories ()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("Clothing categories","window", GUILayout.Width(300)))
        { 
            using (new GUILayout.VerticalScope("box", GUILayout.Width(300)))
            {
                EditorGUIUtility.labelWidth = 110;
                for (int i = 0; i < clothingFactory.itemCategories.Count; i++)
                {
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(300)))
                    {
                        clothingFactory.itemCategories[i] = EditorGUILayout.TextField("Category name: ", clothingFactory.itemCategories[i]);
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            int items = clothingFactory.items.Count;
                            for (int j = items-1; j >= 0; j--)
                            {
                                if (clothingFactory.items[j].category == i)
                                {
                                    clothingFactory.items.RemoveAt(j);
                                }
                            }
                            clothingFactory.itemCategories.RemoveAt(i);
                        }
                    }
                }
            }

            if (GUILayout.Button("Add new category"))
            {
                clothingFactory.itemCategories.Add("");
                clothingFactory.items.Add(new ClothingItem(clothingFactory.itemCategories.Count-1, "Default (Type: " + (clothingFactory.itemCategories.Count - 1).ToString() + ")", null));
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void ClothingItems ()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(300)))
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Clothing items", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            for (int i = 0; i < clothingFactory.itemCategories.Count; i++)
            {
                using (new GUILayout.VerticalScope(clothingFactory.itemCategories[i],"window", GUILayout.Width(460)))
                {
                    for (int j = 0; j < clothingFactory.items.Count; j++)
                    {
                        if (clothingFactory.items[j].category == i)
                        {
                            using (new GUILayout.HorizontalScope("helpbox"))
                            {
                                EditorGUIUtility.labelWidth = 50;
                                clothingFactory.items[j].name = EditorGUILayout.TextField("Name: ", clothingFactory.items[j].name, GUILayout.Width(240));
                                clothingFactory.items[j].category = i;
                                EditorGUILayout.Space();
                                EditorGUIUtility.labelWidth = 45;
                                clothingFactory.items[j].gameObjectOfItem = EditorGUILayout.ObjectField("Item: ", clothingFactory.items[j].gameObjectOfItem, typeof(GameObject), false, GUILayout.Width(230)) as GameObject;
                                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                                {
                                    clothingFactory.items.RemoveAt(j);
                                }
                            }
                        }
                    }

                    if (GUILayout.Button("ADD NEW ITEM", EditorStyles.miniButton))
                    {
                        clothingFactory.items.Add(new ClothingItem(i, "", null));
                    }
                }
                EditorGUILayout.Space();
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(clothingFactory);
        Undo.RecordObject(clothingFactory, "Undo clothingFactory");
    }
}