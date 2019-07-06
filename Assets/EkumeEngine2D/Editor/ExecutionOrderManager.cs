#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[InitializeOnLoad]
public class ExecutionOrderManager : Editor
{
    static ExecutionOrderManager()
    {
        //THINGS TO DO WHEN THE PROJECT IS OPENED
        SetupInputManager();
        SetupExecutionOrder();
        SetupResourcesFolder();

        //Add the new necessaries layers and tags for Ekume Engine 2D if it does not exist
        string[] layers = new string[1] { "Ground" };
        string[] tags = new string[5] { "Mount", "Enemy", "FlyingEnemy", "Coin", "ParkourWall" };

        CheckLayers(layers);
        CheckTags(tags);

        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void SetupExecutionOrder()
    {
        // Get the name of the script we want to change it's execution order
        string playerActivator = typeof(PlayerActivator).Name;
        string player = typeof(Player).Name;
        string inputControls = typeof(InputControls).Name;
        string mobileInputButton = typeof(MobileInputButton).Name;
        string playerSoundsManager = typeof(PlayerSoundsManager).Name;
        string virtualJoystick = typeof(VirtualJoystick).Name;
        string enemy = typeof(VirtualJoystick).Name;
        string aIEnemyMovement = typeof(AIEnemyMovement).Name;
        string aIFlyinEnemy = typeof(AIFlyingEnemy).Name;

        // Iterate through all scripts
        MonoScript[] monoScripts = MonoImporter.GetAllRuntimeMonoScripts();
        foreach (MonoScript monoScript in monoScripts)
        {
            if (monoScript.name == playerActivator)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -750)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -750);
                }
            }
            else if (monoScript.name == player)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -725)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -725);
                }
            }
            else if (monoScript.name == inputControls)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -700)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -700);
                }
            }
            else if (monoScript.name == virtualJoystick)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -680)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -680);
                }
            }
            else if (monoScript.name == mobileInputButton)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -650)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -650);
                }
            }
            else if (monoScript.name == playerSoundsManager)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -600)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -600);
                }
            }
            else if (monoScript.name == enemy)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -550)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -550);
                }
            }
            else if (monoScript.name == aIEnemyMovement)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -500)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -500);
                }
            }
            else if (monoScript.name == aIFlyinEnemy)
            {
                if (MonoImporter.GetExecutionOrder(monoScript) != -450)
                {
                    MonoImporter.SetExecutionOrder(monoScript, -450);
                }
            }
            else
            {
                if (AssetDatabase.GetAssetPath(monoScript).Contains("Assets"))
                {
                    if (MonoImporter.GetExecutionOrder(monoScript) == 0)
                    {
                        MonoImporter.SetExecutionOrder(monoScript, 100);
                    }
                }
            }
        }
    }

    static void SetupInputManager()
    {
        if (!AxisDefined("X axis")) { AddAxis(new InputAxis("X axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 1, 0)); }
        if (!AxisDefined("Y axis")) { AddAxis(new InputAxis("Y axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 2, 0)); }
        if (!AxisDefined("3rd axis")) { AddAxis(new InputAxis("3rd axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 3, 0)); }
        if (!AxisDefined("4th axis")) { AddAxis(new InputAxis("4th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 4, 0)); }
        if (!AxisDefined("5th axis")) { AddAxis(new InputAxis("5th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 5, 0)); }
        if (!AxisDefined("6th axis")) { AddAxis(new InputAxis("6th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 6, 0)); }
        if (!AxisDefined("7th axis")) { AddAxis(new InputAxis("7th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 7, 0)); }
        if (!AxisDefined("8th axis")) { AddAxis(new InputAxis("8th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 8, 0)); }
        if (!AxisDefined("9th axis")) { AddAxis(new InputAxis("9th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 9, 0)); }
        if (!AxisDefined("10th axis")) { AddAxis(new InputAxis("10th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 10, 0)); }
        if (!AxisDefined("11th axis")) { AddAxis(new InputAxis("11th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 11, 0)); }
        if (!AxisDefined("12th axis")) { AddAxis(new InputAxis("12th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 12, 0)); }
        if (!AxisDefined("13th axis")) { AddAxis(new InputAxis("13th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 13, 0)); }
        if (!AxisDefined("14th axis")) { AddAxis(new InputAxis("14th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 14, 0)); }
        if (!AxisDefined("15th axis")) { AddAxis(new InputAxis("15th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 15, 0)); }
        if (!AxisDefined("16th axis")) { AddAxis(new InputAxis("16th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 16, 0)); }
        if (!AxisDefined("17th axis")) { AddAxis(new InputAxis("17th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 17, 0)); }
        if (!AxisDefined("18th axis")) { AddAxis(new InputAxis("18th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 18, 0)); }
        if (!AxisDefined("19th axis")) { AddAxis(new InputAxis("19th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 19, 0)); }
        if (!AxisDefined("20th axis")) { AddAxis(new InputAxis("20th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 20, 0)); }
        if (!AxisDefined("21st axis")) { AddAxis(new InputAxis("21st axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 21, 0)); }
        if (!AxisDefined("22nd axis")) { AddAxis(new InputAxis("22nd axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 22, 0)); }
        if (!AxisDefined("23th axis")) { AddAxis(new InputAxis("23rd axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 23, 0)); }
        if (!AxisDefined("24th axis")) { AddAxis(new InputAxis("24th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 24, 0)); }
        if (!AxisDefined("25th axis")) { AddAxis(new InputAxis("25th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 25, 0)); }
        if (!AxisDefined("26th axis")) { AddAxis(new InputAxis("26th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 26, 0)); }
        if (!AxisDefined("27th axis")) { AddAxis(new InputAxis("27th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 27, 0)); }
        if (!AxisDefined("28th axis")) { AddAxis(new InputAxis("28th axis", "", "", "", "", "", "", 0, 0.19f, 1, false, false, AxisType.JoystickAxis, 28, 0)); }
    }

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    private static bool AxisDefined(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    public enum AxisType
    {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };

    public class InputAxis
    {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap = false;
        public bool invert = false;

        public AxisType type;

        public int axis;
        public int joyNum;

        public InputAxis(string _name, string _descriptiveName, string _descriptiveNegativeName, string _negativeButton, string _positiveButton,
            string _altNegativeButton, string _altPositiveButton, float _gravity, float _dead, float _sensitivity, bool _snap, bool _invert,
            AxisType _type, int _axis, int _joyNum)
        {
            name = _name;
            descriptiveName = _descriptiveName;
            descriptiveNegativeName = _descriptiveNegativeName;
            negativeButton = _negativeButton;
            positiveButton = _positiveButton;
            altNegativeButton = _altNegativeButton;
            altPositiveButton = _altPositiveButton;
            gravity = _gravity;
            dead = _dead;
            sensitivity = _sensitivity;
            snap = _snap;
            invert = _invert;
            type = _type;
            axis = _axis;
            joyNum = _joyNum;
        }
    }

    private static void AddAxis(InputAxis axis)
    {
        if (AxisDefined(axis.name)) return;

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }

    static void SetupResourcesFolder()
    {
        //If the directory "Resources" is in the folder "EkumeEngine2D" will change the path to the main folder.
        DirectoryInfo dir = new DirectoryInfo("Assets/EkumeEngine2D/Resources/Data");
        FileInfo[] files = dir.GetFiles();

        if (!Directory.Exists("Assets/Resources"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory("Assets/Resources");
            AssetDatabase.Refresh();
            SetupResourcesFolder();
        }
        else
        {
            if (!Directory.Exists("Assets/Resources/Data"))
            {
                //if it doesn't, create it
                Directory.CreateDirectory("Assets/Resources/Data");
                AssetDatabase.Refresh();
                SetupResourcesFolder();
            }
        }

        if (Directory.Exists("Assets/Resources/Data") && Directory.Exists("Assets/EkumeEngine2D/Resources/Data"))
        {
            foreach (FileInfo file in files)
            {
                AssetDatabase.MoveAsset("Assets/EkumeEngine2D/Resources/Data/" + file.Name,
                                    "Assets/Resources/Data/" + file.Name);
            }
        }
    }

    public static void OnSceneGUI(SceneView sceneView)
    {
        SceneView.RepaintAll();
        Handles.BeginGUI();

        if (EditorPrefs.GetBool("ShowInformationOfScene", true))
        {
            GUILayout.Window(2, new Rect(Screen.width - 565, Screen.height - 270, 100, 80), (id) =>
            {
                using (new GUILayout.HorizontalScope("helpbox"))
                {
                    using (new GUILayout.VerticalScope("Tools", "Window", GUILayout.Width(227)))
                    {

                        if (GUILayout.Button("Input Controls Manager"))
                        {
                            EditorWindow.GetWindow(typeof(InputControlsManagerEditor), false, "Input controls");
                        }

                        if (GUILayout.Button("Level Editor"))
                        {
                            EditorWindow.GetWindow(typeof(LevelEditor_Editor), false, "Level Editor");
                        }

                        if (GUILayout.Button("Components Manager"))
                        {
                            EditorWindow.GetWindow(typeof(ComponentsManagerEditor), false, "Components Manager");
                        }

                        if (GUILayout.Button("Levels Manager"))
                        {
                            EditorWindow.GetWindow(typeof(LevelsManagerEditor), false, "Levels Manager");
                        }

                        if (GUILayout.Button("Score Types"))
                        {
                            EditorWindow.GetWindow(typeof(ScoreTypesManagerEditor), false, "Score Types");
                        }

                        if (GUILayout.Button("Players Manager"))
                        {
                            EditorWindow.GetWindow(typeof(PlayersManagerEditor), false, "Players Manager");
                        }

                        if (GUILayout.Button("Weapon Factory"))
                        {
                            EditorWindow.GetWindow(typeof(WeaponFactoryEditor), false, "Weapon Factory");
                        }

                        if (GUILayout.Button("Clothing Factory"))
                        {
                            EditorWindow.GetWindow(typeof(ClothingFactoryEditor), false, "Clothing Factory");
                        }

                        if (GUILayout.Button("Options of Powers"))
                        {
                            EditorWindow.GetWindow(typeof(OptionsOfPowersEditor), false, "Options of Powers");
                        }
                    }

                    EditorGUILayout.BeginVertical();
                    using (new GUILayout.VerticalScope("helpbox"))
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Minimum objects for an scene", EditorStyles.boldLabel, GUILayout.Width(220));

                        if (GUILayout.Button("Hide (-)"))
                        {
                            EditorPrefs.SetBool("ShowInformationOfScene", false);
                        }
                        EditorGUILayout.EndHorizontal();

                        if (FindObjectOfType<InputControls>() == null)
                            GUI.backgroundColor = Color.yellow;
                        else
                            GUI.backgroundColor = Color.white;

                        EditorGUILayout.Space();
                        using (new GUILayout.VerticalScope("Essential", "Window"))
                        {
                            if (FindObjectOfType<InputControls>() == null)
                            {
                                if (GUILayout.Button("Create \"" + typeof(InputControls).Name + "\" to read the inputs."))
                                {
                                    GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/EssentialSceneObjects/InputControls.prefab", typeof(GameObject)), new Vector3(0.5f, 10.5f, 0), Quaternion.identity) as GameObject;
                                    go.name = typeof(InputControls).Name;
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField(typeof(InputControls).Name + " added ✔");
                            }
                        }

                        EditorGUILayout.Space();
                        GUI.backgroundColor = Color.white;
                        using (new GUILayout.VerticalScope("For game scene", "window"))
                        {
                            if (FindObjectOfType<GameOverManager>() == null)
                            {
                                if (GUILayout.Button("Create \"" + typeof(GameOverManager).Name + "\""))
                                {
                                    GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/EssentialSceneObjects/GameOverManager.prefab", typeof(GameObject)), new Vector3(0.5f, 12.5f, 0), Quaternion.identity) as GameObject;
                                    go.name = typeof(GameOverManager).Name;
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField(typeof(GameOverManager).Name + " added ✔");
                            }
                        }
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    using (new GUILayout.HorizontalScope("helpbox"))
                    {
                        if (GUILayout.Button("SUPPORT & BUG REPORT", EditorStyles.miniButtonLeft))
                        {
                            Application.OpenURL("http://www.ekume.com/web/soporte/ask");
                        }
                        if (GUILayout.Button("WEB & DOCUMENTATION", EditorStyles.miniButtonRight))
                        {
                            Application.OpenURL("http://www.ekume.com/web/home/");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }, "Tools");
        }
        else
        {
            GUILayout.Window(2, new Rect(Screen.width - 45, Screen.height - 70, 40, 30), (id) =>
            {
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    EditorPrefs.SetBool("ShowInformationOfScene", true);
                }
            }, "Tools");
        }

        Handles.EndGUI();
    }

    public static void CheckLayers(string[] layerNames)
    {
        SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

#if !UNITY_4
        SerializedProperty layersProp = manager.FindProperty("layers");
#endif

        foreach (string name in layerNames)
        {
            // check if layer is present
            bool found = false;
            for (int i = 0; i <= 31; i++)
            {
#if UNITY_4
                    string nm = "User Layer " + i;
                    SerializedProperty sp = manager.FindProperty(nm);
#else
                SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
#endif
                if (sp != null && name.Equals(sp.stringValue))
                {
                    found = true;
                    break;
                }
            }

            // not found, add into 1st open slot
            if (!found)
            {
                SerializedProperty slot = null;
                for (int i = 8; i <= 31; i++)
                {
#if UNITY_4
                        string nm = "User Layer " + i;
                        SerializedProperty sp = manager.FindProperty(nm);
#else
                    SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
#endif
                    if (sp != null && string.IsNullOrEmpty(sp.stringValue))
                    {
                        slot = sp;
                        break;
                    }
                }

                if (slot != null)
                {
                    slot.stringValue = name;
                }
                else
                {
                    Debug.LogError("Could not find an open Layer Slot for: " + name);
                }
            }
        }

        // save
        manager.ApplyModifiedProperties();
    }

    public static void CheckTags(string[] tagNames)
    {
        SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = manager.FindProperty("tags");

        List<string> DefaultTags = new List<string>() { "Untagged", "Respawn", "Finish", "EditorOnly", "MainCamera", "Player", "GameController" };

        foreach (string name in tagNames)
        {
            if (DefaultTags.Contains(name)) continue;

            // check if tag is present
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(name)) { found = true; break; }
            }

            // if not found, add it
            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = name;
            }
        }

        // save
        manager.ApplyModifiedProperties();
    }
}

#endif