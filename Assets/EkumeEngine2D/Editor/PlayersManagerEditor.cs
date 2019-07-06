using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayersManagerEditor : EditorWindow
{
    PlayersManager playersManager;

    Vector2 scrollPos;

    [MenuItem("Tools/Window/Players manager")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(PlayersManagerEditor), false, "Players manager");
    }

    void OnEnable()
    {
        playersManager = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/PlayersManager.asset", typeof(PlayersManager)) as PlayersManager;
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
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("Players manager", "window", GUILayout.Height(100)))
        {
            using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(300)))
            {
                EditorGUIUtility.labelWidth = 115;
                playersManager.defaultPlayer = EditorGUILayout.Popup("Default character: ", playersManager.defaultPlayer, PlayersList());
            }

            using (new GUILayout.VerticalScope("box", GUILayout.Width(300)))
            {
                for (int i = 0; i < playersManager.playerNames.Count; i++)
                {
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(300)))
                    {
                        EditorGUIUtility.labelWidth = 85;
                        playersManager.playerNames[i] = EditorGUILayout.TextField("Character " + i + ":", playersManager.playerNames[i]);

                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                        {
                            playersManager.playerNames.RemoveAt(i);
                        }
                    }
                }
            }

            if (GUILayout.Button("Add new player"))
            {
                playersManager.playerNames.Add("");
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(playersManager);
        Undo.RecordObject(playersManager, "Undo playersManager");
    }


    string[] PlayersList()
    {
        string[] playerNames = new string[playersManager.playerNames.Count];

        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNames[i] = playersManager.playerNames[i];
        }

        return playerNames;
    }

}