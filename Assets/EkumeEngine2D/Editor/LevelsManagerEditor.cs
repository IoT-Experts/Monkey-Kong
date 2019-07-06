using UnityEngine;
using UnityEditor;
using System.Collections;
using EkumeSavedData;
public class LevelsManagerEditor : EditorWindow
{
    LevelsManager levelsManager;

    Vector2 scrollPos;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Levels manager", "Testing tools" };

    [MenuItem("Tools/Window/Levels manager")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(LevelsManagerEditor), false, "Levels manager");
    }

    void OnEnable()
    {
        levelsManager = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/LevelsManager.asset", typeof(LevelsManager)) as LevelsManager;
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
                LevelsManager();
                break;
            case 1:
                TestingTools();
                break;
        }

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(levelsManager);
        Undo.RecordObject(levelsManager, "Undo levelsManager");
    }

    void LevelsManager ()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Levels manager", "window", GUILayout.Width(300)))
        {
            for (int i = 1; i < levelsManager.world.Count; i++)
            {
                using (new GUILayout.VerticalScope("helpbox"))
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        levelsManager.world.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.LabelField("Scenes of world " + i, EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    using (new GUILayout.VerticalScope("helpbox"))
                    {
                        for (int j = 1; j < levelsManager.world[i].level.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 65;
                            levelsManager.world[i].level[j].levelScene = EditorGUILayout.ObjectField("Level " + j, levelsManager.world[i].level[j].levelScene, typeof(SceneAsset), false, GUILayout.Width(215)) as SceneAsset;
                            EditorGUIUtility.labelWidth = 85;
                            levelsManager.world[i].level[j].startUnlocked = EditorGUILayout.Toggle("Start as won: ", levelsManager.world[i].level[j].startUnlocked);

                            if (levelsManager.world[i].level[j]._levelScene != null)
                            {
                                string name = levelsManager.world[i].level[j]._levelScene.ToString();
                                if (levelsManager.world[i].level[j].sceneName != name.Substring(0, name.IndexOf(" (UnityEngine.SceneAsset)")))
                                {
                                    Debug.Log("The scene " + levelsManager.world[i].level[j].sceneName + " was changed to "
                                      + name.Substring(0, name.IndexOf(" (UnityEngine.SceneAsset)")) + " in the Levels Manager.");

                                    levelsManager.world[i].level[j].sceneName = name.Substring(0, name.IndexOf(" (UnityEngine.SceneAsset)"));
                                }
                            }

                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                levelsManager.world[i].level.RemoveAt(j);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    if (GUILayout.Button("Add new level"))
                    {
                        levelsManager.world[i].level.Add(new Level(false, null));
                    }
                }
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add new world"))
            {
                levelsManager.world.Add(new World());
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void TestingTools ()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Testing tools", "window", GUILayout.Height(30)))
        {
            GUILayout.Label("The values changed here only will works on your PC to test your game.");

            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("You should quit the play mode and play again to see the changes.", MessageType.Warning);
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorPrefs.SetInt("UnlockTheLevel", EditorGUILayout.Popup("Select a level: ", EditorPrefs.GetInt("UnlockTheLevel", 0), Levels.GetListOfLevelIdentifications()));

                if(GUILayout.Button("Win the selected level"))
                {
                    Levels.SetLevelCleared(Levels.GetLevelIdentificationOfNumberOfList(EditorPrefs.GetInt("UnlockTheLevel", 0)), true);
                }

                if (GUILayout.Button("Put like not-won the selected level"))
                {
                    Levels.SetLevelCleared(Levels.GetLevelIdentificationOfNumberOfList(EditorPrefs.GetInt("UnlockTheLevel", 0)), false);
                }
            }

            EditorGUILayout.Space();
            using (new GUILayout.VerticalScope("helpbox"))
            {
                if (GUILayout.Button("Win current level ("+ Levels.GetLevelIdentificationOfNextLevel() + ")"))
                {
                    Levels.SetLevelCleared(Levels.GetLevelIdentificationOfNextLevel(), true);
                }
            }

            EditorGUILayout.Space();
            using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(300)))
            {
                EditorGUILayout.LabelField("Restart all saved data including:", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Levels cleared, accumulative Scores, best scores, things bought, etc...", EditorStyles.miniLabel, GUILayout.Width(400));

                if (GUILayout.Button("Restart all (Saved data)"))
                {
                    EkumeSecureData.DeleteAll();
                    PlayerPrefs.DeleteAll();
                }
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }
}