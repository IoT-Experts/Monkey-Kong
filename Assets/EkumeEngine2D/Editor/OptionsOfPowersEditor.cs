using UnityEngine;
using UnityEditor;
using System.Collections;
using EkumeSavedData.Player;
using EkumeEnumerations;

public class OptionsOfPowersEditor : EditorWindow
{
    OptionsOfPowers optionsOfPowers;

    Vector2 scrollPos;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Options of powers", "Testing tools" };

    [MenuItem("Tools/Window/Options of powers")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(OptionsOfPowersEditor), false, "Options of powers");
    }

    void OnEnable()
    {
        optionsOfPowers = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/OptionsOfPowers.asset", typeof(OptionsOfPowers)) as OptionsOfPowers;
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
                OptionsOfPowers();
                break;
            case 1:
                TestingTools();
                break;
        }

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(optionsOfPowers);
        Undo.RecordObject(optionsOfPowers, "Undo optionsOfPowers");
    }

    void OptionsOfPowers ()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.HorizontalScope("Options of powers","window", GUILayout.Height(100)))
        {
            using (new GUILayout.VerticalScope("Default quantity for powers","window", GUILayout.Width(300)))
            {
                optionsOfPowers.defaultQuantityForPower.coinMagnet = EditorGUILayout.IntField("Coin magnet: ", optionsOfPowers.defaultQuantityForPower.coinMagnet);
                optionsOfPowers.defaultQuantityForPower.coinsDuplicator = EditorGUILayout.IntField("Coins duplicator: ", optionsOfPowers.defaultQuantityForPower.coinsDuplicator);
                optionsOfPowers.defaultQuantityForPower.powerToFly = EditorGUILayout.IntField("Power to fly: ", optionsOfPowers.defaultQuantityForPower.powerToFly);
                optionsOfPowers.defaultQuantityForPower.jetpack = EditorGUILayout.IntField("Jetpack: ", optionsOfPowers.defaultQuantityForPower.jetpack);
                optionsOfPowers.defaultQuantityForPower.killerShield = EditorGUILayout.IntField("Killer shield: ", optionsOfPowers.defaultQuantityForPower.killerShield);
                optionsOfPowers.defaultQuantityForPower.protectorShield = EditorGUILayout.IntField("Protector shield: ", optionsOfPowers.defaultQuantityForPower.protectorShield);
                optionsOfPowers.defaultQuantityForPower.trapsConverter = EditorGUILayout.IntField("Traps converter: ", optionsOfPowers.defaultQuantityForPower.trapsConverter);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope("Default time for powers", "window", GUILayout.Width(300)))
            {
                optionsOfPowers.defaultTimeForPower.coinMagnet = EditorGUILayout.FloatField("Coin magnet: ", optionsOfPowers.defaultTimeForPower.coinMagnet);
                optionsOfPowers.defaultTimeForPower.coinsDuplicator = EditorGUILayout.FloatField("Coins duplicator: ", optionsOfPowers.defaultTimeForPower.coinsDuplicator);
                optionsOfPowers.defaultTimeForPower.powerToFly = EditorGUILayout.FloatField("Power to fly: ", optionsOfPowers.defaultTimeForPower.powerToFly);
                optionsOfPowers.defaultTimeForPower.jetpack = EditorGUILayout.FloatField("Jetpack: ", optionsOfPowers.defaultTimeForPower.jetpack);
                optionsOfPowers.defaultTimeForPower.killerShield = EditorGUILayout.FloatField("Killer shield: ", optionsOfPowers.defaultTimeForPower.killerShield);
                optionsOfPowers.defaultTimeForPower.protectorShield = EditorGUILayout.FloatField("Protector shield: ", optionsOfPowers.defaultTimeForPower.protectorShield);
                optionsOfPowers.defaultTimeForPower.trapsConverter = EditorGUILayout.FloatField("Traps converter: ", optionsOfPowers.defaultTimeForPower.trapsConverter);
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

        using (new GUILayout.VerticalScope("Testing tools", "window"))
        {
            EditorGUILayout.HelpBox("The values changed here only will works on your PC to test your game.", MessageType.None);
            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Coin magnet", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("CoinMagnet-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("CoinMagnet-ValueToIncrease-Quantity", 0)));

                if(GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ObjectMagnet, EditorPrefs.GetInt("CoinMagnet-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("CoinMagnet-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("CoinMagnet-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ObjectMagnet, EditorPrefs.GetInt("CoinMagnet-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Coins duplicator", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("CoinsDuplicator-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("CoinsDuplicator-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ScoreDuplicator, EditorPrefs.GetInt("CoinsDuplicator-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("CoinsDuplicator-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("CoinsDuplicator-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ScoreDuplicator, EditorPrefs.GetInt("CoinsDuplicator-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }


            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Power to fly", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("Fly-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("Fly-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.FlyingPower, EditorPrefs.GetInt("Fly-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("Fly-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("Fly-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.FlyingPower, EditorPrefs.GetInt("Fly-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Jetpack", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("Jetpack-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("Jetpack-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.Jetpack, EditorPrefs.GetInt("Jetpack-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("Jetpack-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("Jetpack-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.Jetpack, EditorPrefs.GetInt("Jetpack-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Power Killer Shield", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("KillerShield-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("KillerShield-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.KillerShield, EditorPrefs.GetInt("KillerShield-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("KillerShield-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("KillerShield-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.KillerShield, EditorPrefs.GetInt("KillerShield-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Power Protector Shield", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("ProtectorShield-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("ProtectorShield-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ProtectorShield, EditorPrefs.GetInt("ProtectorShield-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("ProtectorShield-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("ProtectorShield-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.ProtectorShield, EditorPrefs.GetInt("ProtectorShield-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("helpbox"))
            {
                EditorGUILayout.LabelField("Power Traps Converter", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 100;
                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("TrapsConverter-ValueToIncrease-Quantity", EditorGUILayout.IntField("Quantity to add: ", EditorPrefs.GetInt("TrapsConverter-ValueToIncrease-Quantity", 0)));

                if (GUILayout.Button("Add quantity",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.TrapsConverter, EditorPrefs.GetInt("TrapsConverter-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorPrefs.SetInt("TrapsConverter-ValueToIncrease-Time", EditorGUILayout.IntField("Time to add: ", EditorPrefs.GetInt("TrapsConverter-ValueToIncrease-Time", 0)));

                if (GUILayout.Button("Add time",  EditorStyles.miniButton))
                {
                    PowerStats.AddQuantityOfPower(PowersEnum.TrapsConverter, EditorPrefs.GetInt("TrapsConverter-ValueToIncrease-Quantity", 0));
                }

                EditorGUILayout.EndHorizontal();
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