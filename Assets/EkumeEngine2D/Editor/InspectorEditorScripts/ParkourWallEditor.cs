using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ParkourWall))]

public class ParkourWallEditor : Editor
{

    ParkourWall parkourWall;
    List<Player> playersInScene = new List<Player>();

    void OnEnable()
    {
        parkourWall = (ParkourWall)target;

        playersInScene = FindObjectsOfTypeAll<Player>();

    }

    public override void OnInspectorGUI()
    {
        foreach (Player player in playersInScene)
        {
            if (player.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D rbOfPlayer = player.GetComponent<Rigidbody2D>();

                if (rbOfPlayer.collisionDetectionMode != CollisionDetectionMode2D.Continuous)
                {
                    EditorGUILayout.HelpBox("This component might not works well with the player '" + player.gameObject.name + "'. Please select the option 'Continuous' in the Collision Detection of the RigidBody2D of the Player.", MessageType.Warning);

                    if (GUILayout.Button("Set as 'Continuous' the Collision Detection of " + player.gameObject.name, EditorStyles.miniButtonMid))
                    {
                        rbOfPlayer.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    }
                    EditorGUILayout.Space();
                }
            }
        }

        DrawDefaultInspector();

        EditorUtility.SetDirty(parkourWall);
        Undo.RecordObject(parkourWall, "Undo parkourWall");
    }

    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }
}
