using UnityEngine;
using UnityEditor;
using EkumeSavedData.Scores;
using EkumeSavedData;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class ScoreTypesManagerEditor : EditorWindow
{
    ScoreTypesManager scoreTypesManager;

    Vector2 scrollPos;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Score types", "Testing tools" };

    [MenuItem("Tools/Window/Score types")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(ScoreTypesManagerEditor), false, "Score types");
    }

    void OnEnable()
    {
        scoreTypesManager = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/ScoreTypesManager.asset", typeof(ScoreTypesManager)) as ScoreTypesManager;
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

        //-------------------------------------------------------------------------------------- /

        selectedMenu = GUILayout.Toolbar(selectedMenu, toolbarOptions, EditorStyles.toolbarButton);

        //Start scroll view
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(width), GUILayout.Height(height - 17));

        EditorGUILayout.Space();
        switch (selectedMenu)
        {
            case 0:
                ScoreTypes();
                break;
            case 1:
                TestingTools();
                break;
        }    

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(scoreTypesManager);
        Undo.RecordObject(scoreTypesManager, "Undo creatorOfScoreType");
    }

    void ScoreTypes ()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Score types", "window", GUILayout.Width(300), GUILayout.Height(50)))
        {
            for (int i = 0; i < scoreTypesManager.ScoresData.Count; i++)
            {
                using (new GUILayout.HorizontalScope("box"))
                {
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        EditorPrefs.SetBool("showWarningToDeleteScore" + i, true);
                        return;
                    }

                    EditorGUIUtility.labelWidth = 70;
                    EditorGUILayout.BeginVertical();
                    scoreTypesManager.ScoresData[i].scoreName = EditorGUILayout.TextField("["+ i + "]" + " Name: ", scoreTypesManager.ScoresData[i].scoreName, GUILayout.Width(150));
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    EditorGUIUtility.labelWidth = 140;
                    scoreTypesManager.ScoresData[i].accumulative = EditorGUILayout.Toggle("Is accumulative: ", scoreTypesManager.ScoresData[i].accumulative, GUILayout.Width(155));
                    
                    if (scoreTypesManager.ScoresData[i].accumulative)
                    {
                        scoreTypesManager.ScoresData[i].accumulateOnlyIfWin = EditorGUILayout.Toggle("Accumulate only if win: ", scoreTypesManager.ScoresData[i].accumulateOnlyIfWin, GUILayout.Width(155));
                        EditorGUIUtility.labelWidth = 90;
                        scoreTypesManager.ScoresData[i].defaultValue = EditorGUILayout.FloatField("Default value: ", scoreTypesManager.ScoresData[i].defaultValue, GUILayout.Width(155));
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical();
                    EditorGUIUtility.labelWidth = 140;
                    scoreTypesManager.ScoresData[i].saveBestByLevel = EditorGUILayout.Toggle("| Save best: ", scoreTypesManager.ScoresData[i].saveBestByLevel, GUILayout.Width(160));
                    if (scoreTypesManager.ScoresData[i].saveBestByLevel)
                    {
                        scoreTypesManager.ScoresData[i].saveBestOnlyIfWin = EditorGUILayout.Toggle("| Save best only if win: ", scoreTypesManager.ScoresData[i].saveBestOnlyIfWin, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndVertical();
                }

                if (EditorPrefs.GetBool("showWarningToDeleteScore" + i, false))
                {
                    EditorGUILayout.HelpBox("WARNING!\nIf you remove the score " + scoreTypesManager.ScoresData[i].scoreName + " all the components that requires of this information, like the score increasers or the buttons to buy things will change the variable of 'score' for another or will be clear without a value and you will need to change it by yourself.\nAre you sure you want to remove this score?", MessageType.Warning);
                    EditorGUILayout.BeginHorizontal();
                    if(GUILayout.Button("YES", EditorStyles.miniButton))
                    {
                        scoreTypesManager.ScoresData.RemoveAt(i);
                        EditorPrefs.SetBool("showWarningToDeleteScore" + i, false);
                    }

                    if (GUILayout.Button("NOT", EditorStyles.miniButton))
                    {
                        EditorPrefs.SetBool("showWarningToDeleteScore" + i, false);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("New Score type", GUILayout.Width(300)))
            {
                scoreTypesManager.ScoresData.Add(new ScoresData("", false, false, false, false, 0));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void TestingTools ()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Testing tools", "window", GUILayout.Height(100), GUILayout.Width(300)))
        {
            EditorGUILayout.HelpBox("The values changed here only will works on your PC to test your game.", MessageType.Info);

            using (new GUILayout.VerticalScope("Accumulative Scores", "window", GUILayout.Width(420), GUILayout.Height(30)))
            {
                for (int i = 0; i < scoreTypesManager.ScoresData.Count; i++)
                {
                    if (scoreTypesManager.ScoresData[i].accumulative)
                    {
                        using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(400)))
                        {
                            EditorGUIUtility.labelWidth = 100;
                            EditorGUILayout.LabelField(scoreTypesManager.ScoresData[i].scoreName, EditorStyles.boldLabel, GUILayout.Width(100));
                            EditorPrefs.SetInt("TestingTools-QuantityToAddToScore-Accumulated" + i, EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Accumulated" + i, 0)));

                            if (GUILayout.Button("Add to accumulated"))
                            {
                                Accumulated.SetAccumulated(i, Accumulated.GetAccumulated(i) + EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Accumulated" + i, 0));

                                if (Application.isPlaying)
                                    ShowScore.refreshScores = true;
                            }
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope("Best scores", "window", GUILayout.Width(420), GUILayout.Height(30)))
            {
                EditorGUIUtility.labelWidth = 50;
                EditorPrefs.SetInt("levelIdentificationSelectedForScoreTypesTesting", EditorGUILayout.Popup("Level: ", EditorPrefs.GetInt("levelIdentificationSelectedForScoreTypesTesting", 0), Levels.GetListOfLevelIdentifications(), GUILayout.Width(180)));
                for (int i = 0; i < scoreTypesManager.ScoresData.Count; i++)
                {
                    if (scoreTypesManager.ScoresData[i].saveBestByLevel)
                    {
                        using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(300)))
                        {
                            EditorGUIUtility.labelWidth = 100;
                            EditorGUILayout.LabelField(scoreTypesManager.ScoresData[i].scoreName, EditorStyles.boldLabel, GUILayout.Width(100));
                            EditorPrefs.SetInt("TestingTools-QuantityToAddToScore-Best" + i, EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Best" + i, 0)));
                            if (GUILayout.Button("Set as best score"))
                            {
                                Score.SetBestScoreOfLevel(i, Levels.GetLevelIdentificationOfNumberOfList(EditorPrefs.GetInt("levelIdentificationSelectedForScoreTypesTesting")), EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Best" + i, 0));
                                if (Application.isPlaying)
                                    ShowScore.refreshScores = true;
                            }
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope("Current Scores in game", "window", GUILayout.Height(50), GUILayout.Width(420)))
            {
                if (Application.isPlaying)
                {
                    for (int i = 0; i < scoreTypesManager.ScoresData.Count; i++)
                    {
                        if (!scoreTypesManager.ScoresData[i].accumulative)
                        {
                            using (new GUILayout.HorizontalScope("box", GUILayout.Width(300)))
                            {
                                EditorGUIUtility.labelWidth = 100;
                                EditorGUILayout.LabelField(scoreTypesManager.ScoresData[i].scoreName, EditorStyles.boldLabel, GUILayout.Width(100));
                                EditorPrefs.SetInt("TestingTools-QuantityToAddToScore-Score" + i, EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Score" + i, 0)));

                                if (GUILayout.Button("Add to current score"))
                                {
                                    Score.SetScoreOfLevel(i, Score.GetScoreOfLevel(i) + EditorPrefs.GetInt("TestingTools-QuantityToAddToScore-Score" + i, 0));
                                    ShowScore.refreshScores = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("To use this option, you should to be in play mode.", MessageType.Warning);
                }
            }

            EditorGUILayout.Space();
            using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(420)))
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
