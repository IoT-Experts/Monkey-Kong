using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Player))]

public class PlayerEditor : Editor
{

    Texture2D jumpIcon;
    Texture2D playerWithGunIcon;
    Texture2D liveIcon;
    Texture2D playerHorizontalMovementIcon;
    Texture2D iconPowers;
    Texture2D soundIcon;
    Texture2D animationIcon;
    Player player;

    void OnEnable()
    {
        jumpIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/JumpIcon.png", typeof(Texture2D)) as Texture2D;
        playerWithGunIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/PlayerWithGunIcon.png", typeof(Texture2D)) as Texture2D;
        liveIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LiveIcon.png", typeof(Texture2D)) as Texture2D;
        playerHorizontalMovementIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/PlayerHorizontalMovementIcon.png", typeof(Texture2D)) as Texture2D;
        iconPowers = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/IconPowers.png", typeof(Texture2D)) as Texture2D;
        soundIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/SoundIcon.png", typeof(Texture2D)) as Texture2D;
        animationIcon = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/AnimationIcon.png", typeof(Texture2D)) as Texture2D;
        player = (Player)target;
        player.tag = "Player";
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        GUI.backgroundColor = Color.white;
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                player.playerIdentification = EditorGUILayout.Popup("Player identification: ", player.playerIdentification, PlayersList());
            }

            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Open players manager", EditorStyles.miniButtonMid))
            {
                EditorWindow.GetWindow<PlayersManagerEditor>(false, "Players manager");
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.white;
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Add components to the player", EditorStyles.boldLabel, GUILayout.Width(210));

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
                GUI.backgroundColor = (player.GetComponent<PlayerJump>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(jumpIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<PlayerJump>() == null)
                        player.gameObject.AddComponent<PlayerJump>();
                }
            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerJump.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Allows jump to the player. You will be able to configure the jump as you need it.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (player.GetComponent<UseWeapon>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(playerWithGunIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<UseWeapon>() == null)
                        player.gameObject.AddComponent<UseWeapon>();
                }

            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box", GUILayout.Width(250)))
            {
                EditorGUILayout.LabelField("Name: UseWeapon.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Allows use to the player, guns and melee weapons. You can create new weapons in the Weapon Factory window.", MessageType.None);
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
                GUI.backgroundColor = (player.GetComponent<PlayerLifeManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(liveIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<PlayerLifeManager>() == null)
                        player.gameObject.AddComponent<PlayerLifeManager>();
                }
            }
            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerLifeManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Manages the health of the player, this component should be added if your player dies in some moment.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /


            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (player.GetComponent<PlayerHorizontalMovement>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(playerHorizontalMovementIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<PlayerHorizontalMovement>() == null)
                        player.gameObject.AddComponent<PlayerHorizontalMovement>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerHorizontalMovement.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Allows to the player move horizontally with different options to have the movement type you want.", MessageType.None);
            }

            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (player.GetComponent<PlayerPowers>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(iconPowers, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<PlayerPowers>() == null)
                        player.gameObject.AddComponent<PlayerPowers>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerPowers.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("7 different powers that you can add to the player.", MessageType.None);
            }
            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (player.GetComponent<PlayerSoundsManager>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(soundIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<PlayerSoundsManager>() == null)
                        player.gameObject.AddComponent<PlayerSoundsManager>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: PlayerSoundsManager.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Manages the sounds depending of the player states and input control conditions too.", MessageType.None);
            }
            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /

            //---------------------------------------------------------------------------------------------- /
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.white;

            using (new GUILayout.VerticalScope("box"))
            {
                GUI.backgroundColor = (player.GetComponent<AnimatorManagerOfPlayer>() == null) ? Color.cyan : Color.gray;

                if (GUILayout.Button(animationIcon, GUILayout.Height(55), GUILayout.Width(70)))
                {
                    if (player.GetComponent<AnimatorManagerOfPlayer>() == null)
                        player.gameObject.AddComponent<AnimatorManagerOfPlayer>();
                }
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Name: AnimatorManagerOfPlayer.cs", EditorStyles.miniLabel);
                EditorGUILayout.HelpBox("Manages the parameter values of the animator component depending of the player states.", MessageType.None);
            }
            EditorGUILayout.EndHorizontal();
            //---------------------------------------------------------------------------------------------- /
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(player);
        Undo.RecordObject(player, "Undo player");
    }

    string[] PlayersList()
    {
        string[] playerNames = new string[PlayersManager.instance.playerNames.Count];

        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNames[i] = PlayersManager.instance.playerNames[i];
        }

        return playerNames;
    }

}
