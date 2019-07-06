using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Enemy))]

public class EnemyEditor : Editor
{

    Enemy enemy;
    Texture2D playerWithGunIcon;
    Texture2D liveIcon;
    Texture2D gearsTexture;
    Texture2D soundIcon;
    Texture2D animationIcon;

    void OnEnable()
    {
        playerWithGunIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/PlayerWithGunIcon.png", typeof(Texture2D)) as Texture2D;
        liveIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LiveIcon.png", typeof(Texture2D)) as Texture2D;
        gearsTexture = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/Gears.png", typeof(Texture2D)) as Texture2D;
        soundIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/SoundIcon.png", typeof(Texture2D)) as Texture2D;
        animationIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/AnimationIcon.png", typeof(Texture2D)) as Texture2D;

        enemy = (Enemy)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = Color.white;

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Add components to the enemy", EditorStyles.boldLabel, GUILayout.Width(210));

        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.HelpBox("Remember change the configurations of every added component.", MessageType.None);
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = Color.white;
        using (new GUILayout.VerticalScope("box", GUILayout.Width(220)))
        {
            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<EnemyLifeManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(liveIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<EnemyLifeManager>() == null)
                    {
                        enemy.gameObject.AddComponent<EnemyLifeManager>();
                        enemy.GetComponent<EnemyLifeManager>().enabled = false;
                    }
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: EnemyLifeManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("This component manages the health of the enemy.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<UseWeapon>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(playerWithGunIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<UseWeapon>() == null)
                        enemy.gameObject.AddComponent<UseWeapon>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: UseWeapon.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("It allows to the enemy uses guns or melee weapons. You can create new weapons in: ", MessageType.None);
                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Weapon Factory", EditorStyles.miniButtonMid))
                {
                    EditorWindow.GetWindow<WeaponFactoryEditor>(false, "Weapon Factory");
                }
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<AIEnemyMovement>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(gearsTexture, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<AIEnemyMovement>() == null)
                        enemy.gameObject.AddComponent<AIEnemyMovement>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: AIEnemyMovement.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Use the basic elements for the AI of an enemy in the platform games.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<AIFlyingEnemy>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(gearsTexture, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<AIFlyingEnemy>() == null)
                        enemy.gameObject.AddComponent<AIFlyingEnemy>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: AIFlyingEnemy.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("This component lets your enemy follow a target ignoring the physics.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<EnemySoundsManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(soundIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<EnemySoundsManager>() == null)
                        enemy.gameObject.AddComponent<EnemySoundsManager>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: EnemySoundsManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Manages the sounds depending of the enemy states.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (enemy.GetComponent<AnimatorManagerOfEnemy>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(animationIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (enemy.GetComponent<AnimatorManagerOfEnemy>() == null)
                        enemy.gameObject.AddComponent<AnimatorManagerOfEnemy>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: AnimatorManagerOfEnemy.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Manages the parameter values of the animator depending of the enemy states.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /


        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

    }

}
