using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Mount))]

public class MountEditor : Editor
{
    Mount mount;

    Texture2D jumpIcon;
    Texture2D playerWithGunIcon;
    Texture2D liveIcon;
    Texture2D playerHorizontalMovementIcon;
    Texture2D arrowDismountIcon;
    Texture2D soundIcon;
    Texture2D animationIcon;

    void OnEnable()
    {
        jumpIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/JumpIcon.png", typeof(Texture2D)) as Texture2D;
        playerWithGunIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/PlayerWithGunIcon.png", typeof(Texture2D)) as Texture2D;
        liveIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LiveIcon.png", typeof(Texture2D)) as Texture2D;
        playerHorizontalMovementIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/PlayerHorizontalMovementIcon.png", typeof(Texture2D)) as Texture2D;
        arrowDismountIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/ArrowDismountIcon.png", typeof(Texture2D)) as Texture2D;
        soundIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/SoundIcon.png", typeof(Texture2D)) as Texture2D;
        animationIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/AnimationIcon.png", typeof(Texture2D)) as Texture2D;

        mount = (Mount)target;
        mount.tag = "Mount";
    }

    public override void OnInspectorGUI()
    {
        if (mount.GetComponentInChildren<RideTheMount>() == null)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.HelpBox("You should have the component \"RideTheMount\" in some child of your mount.", MessageType.Warning);

                if(GUILayout.Button("Create child with component \"RideTheMount\""))
                {
                    GameObject objectCreated = new GameObject();
                    objectCreated.name = "PlayerMounting";
                    objectCreated.transform.SetParent(mount.transform);
                    objectCreated.AddComponent<BoxCollider2D>();
                    objectCreated.GetComponent<BoxCollider2D>().isTrigger = true;
                    objectCreated.AddComponent<RideTheMount>();
                    objectCreated.transform.position = mount.transform.position;
                    Selection.activeGameObject = objectCreated;
                    EditorGUIUtility.PingObject(objectCreated);
                    Debug.Log("The GameObject \"PlayerMounting\" of type RideTheMount was created.", objectCreated);
                }
            }
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = Color.white;

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Add components to the mount", EditorStyles.boldLabel, GUILayout.Width(210));

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
                GUI.backgroundColor = (mount.GetComponent<DismountOfPlayer>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(arrowDismountIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<DismountOfPlayer>() == null)
                    {
                        mount.gameObject.AddComponent<DismountOfPlayer>();
                        mount.GetComponent<DismountOfPlayer>().enabled = false;
                    }
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: DismountOfPlayer.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Allows the player dismount with a control.", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /


            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (mount.GetComponent<PlayerJump>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(jumpIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<PlayerJump>() == null)
                    {
                        mount.gameObject.AddComponent<PlayerJump>();
                        mount.GetComponent<PlayerJump>().enabled = false;
                    }
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerJump.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Allows the mount jump.", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (mount.GetComponent<AnimatorManagerOfPlayer>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(animationIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<AnimatorManagerOfPlayer>() == null)
                    {
                        mount.gameObject.AddComponent<AnimatorManagerOfPlayer>();
                        mount.GetComponent<AnimatorManagerOfPlayer>().enabled = false;
                    }
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: AnimatorManagerOfPlayer.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Manages the parameter values of the ", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("animator depending of the player/mount", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("states.", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (mount.GetComponent<MountLifeManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(liveIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<MountLifeManager>() == null)
                    {
                        mount.gameObject.AddComponent<MountLifeManager>();
                        mount.GetComponent<MountLifeManager>().enabled = false;
                    }
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: MountLifeManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Manages the health of the mount,", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("this component should be added if your", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("mount dies in some moment.", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (mount.GetComponent<UseWeapon>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(playerWithGunIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<UseWeapon>() == null)
                        mount.gameObject.AddComponent<UseWeapon>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: UseWeapon.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("It allows the mount uses guns or melee", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("weapons. You can create new weapons in:", EditorStyles.miniLabel, GUILayout.Width(212));
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
                GUI.backgroundColor = (mount.GetComponent<PlayerHorizontalMovement>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(playerHorizontalMovementIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<PlayerHorizontalMovement>() == null)
                        mount.gameObject.AddComponent<PlayerHorizontalMovement>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerHorizontalMovement.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("It allows the mount to move horizontally.", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (mount.GetComponent<PlayerSoundsManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(soundIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (mount.GetComponent<PlayerSoundsManager>() == null)
                        mount.gameObject.AddComponent<PlayerSoundsManager>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerSoundsManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("Manages the sounds depending of the", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("player/mount states.", EditorStyles.miniLabel);
            }
            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /


        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

    }
}
