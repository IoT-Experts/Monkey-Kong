using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputControlsManagerEditor : EditorWindow
{

    InputControlsManager inputControlsManager;

    Texture2D textureDesktopPlatforms;
    Texture2D textureJoystick;
    Texture2D textureMobiles;
    Texture2D textureVirtualJoystick;
    Texture2D textureButtonMobile;
    Vector2 scrollPos;
    
    [MenuItem("Tools/Window/Input controls manager")]
    private static void OpenWindow()
    {
        InputControlsManagerEditor window = EditorWindow.GetWindow<InputControlsManagerEditor>();
        // Loads an icon from an image stored at the specified path
        Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/EkumeEngine2D/Sprites/EditorIcons/Joystick.png");
        // Create the instance of GUIContent to assign to the window. Gives the title "RBSettings" and the icon
        GUIContent titleContent = new GUIContent("Input controls", icon);
        window.titleContent = titleContent;
    }


    void OnEnable()
    {
        textureDesktopPlatforms = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/DesktopPlatforms.png", typeof(Texture2D)) as Texture2D;
        textureJoystick = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/Joystick.png", typeof(Texture2D)) as Texture2D;
        textureMobiles = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/Mobiles.png", typeof(Texture2D)) as Texture2D;
        textureVirtualJoystick = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/VirtualJoystick.png", typeof(Texture2D)) as Texture2D;
        textureButtonMobile = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/ButtonMobileIcon.png", typeof(Texture2D)) as Texture2D;

        inputControlsManager = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/InputControlsManager.asset", typeof(InputControlsManager)) as InputControlsManager;
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
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("Input controls", "window", GUILayout.Width(30), GUILayout.Height(30)))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Control names", EditorStyles.boldLabel);

                using (var verticalScope1 = new GUILayout.VerticalScope("helpbox"))
                {
                    EditorGUIUtility.labelWidth = 115;
                    for (int i = 0; i < inputControlsManager.inputNames.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        inputControlsManager.inputNames[i] = EditorGUILayout.TextField("Control name [" + i + "]: ", inputControlsManager.inputNames[i], GUILayout.Width(280));
                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(25)))
                        {
                            inputControlsManager.inputNames.RemoveAt(i);
                            inputControlsManager.keyCodePC.RemoveAt(i);
                            inputControlsManager.keyCodeJoystick.RemoveAt(i);
                            inputControlsManager.useJoystickAxis.RemoveAt(i);
                            inputControlsManager.positiveAxisValue.RemoveAt(i);
                            inputControlsManager.keyCodeCursorEvent.RemoveAt(i);
                            if (inputControlsManager.inputNames.Count > 0)
                                i = 0;
                            else
                                break;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("New input control", GUILayout.Width(280)))
                    {
                        inputControlsManager.inputNames.Add("");
                        inputControlsManager.keyCodePC.Add(0);
                        inputControlsManager.keyCodeJoystick.Add(0);
                        inputControlsManager.useJoystickAxis.Add(false);
                        inputControlsManager.positiveAxisValue.Add(true);
                        inputControlsManager.keyCodeCursorEvent.Add(0);
                    }
                }
            }

            using (var verticalScope = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 150;
                inputControlsManager.useInPC = EditorGUILayout.Toggle("Use desktop inputs", inputControlsManager.useInPC, GUILayout.Width(175));
                GUILayout.Label(textureDesktopPlatforms, GUILayout.Height(20));
                EditorGUILayout.EndHorizontal();

                if (inputControlsManager.useInPC)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 120;
                        EditorGUILayout.LabelField("Input controls for desktop", EditorStyles.boldLabel);

                        for (int i = 0; i < inputControlsManager.inputNames.Count; i++)
                        {
                            inputControlsManager.keyCodePC[i] = EditorGUILayout.Popup(inputControlsManager.inputNames[i], inputControlsManager.keyCodePC[i], InputControlsManager.CustomKeyCodesPC, GUILayout.Width(280));
                        }
                    }
                }
            }

            using (var verticalScope = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 150;
                inputControlsManager.useInJoystick = EditorGUILayout.Toggle("Use Joystick inputs", inputControlsManager.useInJoystick, GUILayout.Width(175));
                GUILayout.Label(textureJoystick, GUILayout.Height(20));
                EditorGUILayout.EndHorizontal();

                if (inputControlsManager.useInJoystick)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 120;
                        EditorGUILayout.LabelField("Input controls for Joystick", EditorStyles.boldLabel);

                        for (int i = 0; i < inputControlsManager.inputNames.Count; i++)
                        {
                            using (var verticalScope3 = new GUILayout.VerticalScope("box"))
                            {
                                EditorGUIUtility.labelWidth = 120;

                                if (!inputControlsManager.useJoystickAxis[i])
                                    inputControlsManager.keyCodeJoystick[i] = EditorGUILayout.Popup(inputControlsManager.inputNames[i], inputControlsManager.keyCodeJoystick[i], InputControlsManager.CustomKeyCodesJoystick, GUILayout.Width(280));
                                else
                                    inputControlsManager.keyCodeJoystick[i] = EditorGUILayout.Popup(inputControlsManager.inputNames[i], inputControlsManager.keyCodeJoystick[i], InputControlsManager.CustomKeyCodesJoystickAxis, GUILayout.Width(280));

                                EditorGUILayout.BeginHorizontal();
                                EditorGUIUtility.labelWidth = 60;
                                inputControlsManager.useJoystickAxis[i] = EditorGUILayout.Toggle("Use axis: ", inputControlsManager.useJoystickAxis[i], GUILayout.Width(90));
                                if (inputControlsManager.useJoystickAxis[i])
                                {
                                    EditorGUIUtility.labelWidth = 80;

                                    inputControlsManager.positiveAxisValue[i] = EditorGUILayout.Toggle("Axis value: ", inputControlsManager.positiveAxisValue[i], GUILayout.Width(100));

                                    if (!inputControlsManager.positiveAxisValue[i])
                                        EditorGUILayout.LabelField("(Negative)");
                                    else
                                        EditorGUILayout.LabelField("(Positive)");

                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                }
            }


            using (var verticalScope = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 110;
                inputControlsManager.useControlsInUI = EditorGUILayout.Toggle("Use controls in UI ", inputControlsManager.useControlsInUI, GUILayout.Width(130));
                GUILayout.Label(textureMobiles, GUILayout.Height(25));
                EditorGUILayout.EndHorizontal();

                if (inputControlsManager.useControlsInUI)
                {
                    using (var verticalScope3 = new GUILayout.VerticalScope("helpbox"))
                    {
                        EditorGUILayout.LabelField("Manager of controls in UI", EditorStyles.boldLabel);

                        using (var verticalScope4 = new GUILayout.VerticalScope("helpbox"))
                        {
                            EditorGUIUtility.labelWidth = 190;
                            inputControlsManager.nameOfParentMobileControls = EditorGUILayout.TextField("Screen controls, parent name: ", inputControlsManager.nameOfParentMobileControls);

                            GameObject parentMobileControls = GameObject.Find(inputControlsManager.nameOfParentMobileControls);

                            if (parentMobileControls != null && parentMobileControls.GetComponent<RectTransform>() != null && parentMobileControls.transform.parent != null && parentMobileControls.transform.parent.GetComponent<Canvas>() != null)
                            {
                                foreach (Transform child in GameObject.Find(inputControlsManager.nameOfParentMobileControls).transform)
                                {
                                    using (var horizontal = new GUILayout.HorizontalScope("box"))
                                    {
                                        EditorGUILayout.BeginVertical(GUILayout.Width(75));
                                        //EditorGUILayout.LabelField(child.name, EditorStyles.whiteLabel,GUILayout.Width(72));
                                        child.name = EditorGUILayout.TextField(child.name, GUILayout.Width(75));
                                        if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(75)))
                                        {
                                            DestroyImmediate(child.gameObject);
                                        }
                                        EditorGUILayout.EndVertical();

                                        if (child != null)
                                        {
                                            if (child.GetComponent<VirtualJoystick>() != null)
                                            {
                                                VirtualJoystick virtualJoystick = child.GetComponent<VirtualJoystick>();
                                                GUILayout.Label(textureVirtualJoystick, GUILayout.Height(25), GUILayout.Width(30));

                                                EditorGUILayout.BeginVertical();
                                                using (var verticalScope0 = new GUILayout.VerticalScope("box"))
                                                {
                                                    EditorGUIUtility.labelWidth = 110;
                                                    virtualJoystick.useAxisXPositive = EditorGUILayout.Toggle("Use axis X+: ", virtualJoystick.useAxisXPositive);
                                                    if (virtualJoystick.useAxisXPositive)
                                                    {
                                                        virtualJoystick.inputControlRight = EditorGUILayout.Popup("Input control X+: ", virtualJoystick.inputControlRight, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
                                                    }

                                                    virtualJoystick.useAxisXNegative = EditorGUILayout.Toggle("Use axis X-: ", virtualJoystick.useAxisXNegative);
                                                    if (virtualJoystick.useAxisXNegative)
                                                    {
                                                        virtualJoystick.inputControlLeft = EditorGUILayout.Popup("Input control X-: ", virtualJoystick.inputControlLeft, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
                                                    }
                                                }

                                                using (var verticalScope0 = new GUILayout.VerticalScope("box"))
                                                {
                                                    virtualJoystick.useAxisYPositive = EditorGUILayout.Toggle("Use axis Y+: ", virtualJoystick.useAxisYPositive);
                                                    if (virtualJoystick.useAxisYPositive)
                                                    {
                                                        virtualJoystick.inputControlUp = EditorGUILayout.Popup("Input control Y+: ", virtualJoystick.inputControlUp, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
                                                    }

                                                    virtualJoystick.useAxisYNegative = EditorGUILayout.Toggle("Use axis Y-: ", virtualJoystick.useAxisYNegative);
                                                    if (virtualJoystick.useAxisYNegative)
                                                    {
                                                        virtualJoystick.inputControlDown = EditorGUILayout.Popup("Input control Y-: ", virtualJoystick.inputControlDown, convertListStringToArray(virtualJoystick.inputControlsManager.inputNames));
                                                    }
                                                }

                                                using (var verticalScope0 = new GUILayout.VerticalScope("box"))
                                                {
                                                    EditorGUIUtility.labelWidth = 125;
                                                    virtualJoystick.joystickDeadZone = EditorGUILayout.FloatField("Joystick dead zone: ", virtualJoystick.joystickDeadZone);
                                                }

                                                EditorUtility.SetDirty(virtualJoystick);
                                                EditorGUILayout.EndVertical();

                                            }
                                            else if (child.GetComponent<Button>() != null)
                                            {
                                                GUILayout.Label(textureButtonMobile, GUILayout.Height(15), GUILayout.Width(30));

                                                if (child.GetComponent<MobileInputButton>() != null)
                                                {
                                                    MobileInputButton mobileInputButton = child.GetComponent<MobileInputButton>();
                                                    EditorGUIUtility.labelWidth = 85;
                                                    mobileInputButton.inputControl.Index = EditorGUILayout.Popup("Input control: ", mobileInputButton.inputControl.Index, convertListStringToArray(InputControlsManager.instance.inputNames), GUILayout.Width(180));

                                                    EditorUtility.SetDirty(mobileInputButton);
                                                }
                                                else
                                                {
                                                    EditorGUILayout.BeginVertical();
                                                    EditorGUILayout.LabelField("It must have <MobileInputControls> script to work.", EditorStyles.miniBoldLabel);

                                                    if (GUILayout.Button("Add MobileInputControls"))
                                                    {
                                                        child.gameObject.AddComponent<MobileInputButton>();
                                                    }
                                                    EditorGUILayout.EndVertical();
                                                }
                                            }
                                            else
                                            {
                                                EditorGUILayout.LabelField("(This object is not a button and is not joystick)", EditorStyles.miniBoldLabel);
                                            }
                                        }
                                    }
                                }

                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("Add new Button"))
                                {
                                    GameObject newObject = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/MobileControls/Button.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                                    newObject.transform.SetParent(parentMobileControls.transform);
                                    newObject.GetComponent<RectTransform>().localScale = Vector3.one * 1.3f;

                                }

                                if (GUILayout.Button("Add new Joystick"))
                                {
                                    GameObject newObject = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/MobileControls/Joystick.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                                    newObject.transform.SetParent(parentMobileControls.transform);
                                    newObject.GetComponent<RectTransform>().localScale = Vector3.one * 1.3f;

                                }

                                EditorGUILayout.EndHorizontal();

                                if (PrefabUtility.GetPrefabParent(parentMobileControls.transform.root.gameObject) != null)
                                {
                                    if (GUILayout.Button("Save canvas prefab"))
                                    {

                                        PrefabUtility.ReplacePrefab(parentMobileControls.transform.root.gameObject, PrefabUtility.GetPrefabParent(parentMobileControls.transform.root.gameObject), ReplacePrefabOptions.ConnectToPrefab);
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("Atention: This object is not a prefab.");
                                    GUILayout.Label("If you want this changes for all scenes, save the object in prefab.", EditorStyles.miniLabel);
                                }
                            }
                            else
                            {
                                EditorGUILayout.LabelField("(Name of the object in the scene that contains the controls)", EditorStyles.miniBoldLabel);
                                EditorGUILayout.LabelField("(The object must have the component <RectTransform>)", EditorStyles.miniBoldLabel);
                                EditorGUILayout.LabelField("(The object must be child of the main CanvasUI)", EditorStyles.miniBoldLabel);
                            }
                        }
                    }
                }
            }

            using (var verticalScope3 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 140;

                EditorGUILayout.BeginHorizontal();
                inputControlsManager.useTouchOrClickEvents = EditorGUILayout.Toggle("Use touch/click events ", inputControlsManager.useTouchOrClickEvents, GUILayout.Width(165));
                GUILayout.Label(textureMobiles, GUILayout.Height(25), GUILayout.Width(25));
                GUILayout.Label(textureDesktopPlatforms, GUILayout.Height(20), GUILayout.Width(75));
                EditorGUILayout.EndHorizontal();

                if (inputControlsManager.useTouchOrClickEvents)
                {
                    using (var verticalScope4 = new GUILayout.VerticalScope("helpbox"))
                    {

                        EditorGUILayout.LabelField("Touch and click events (Works in mobile and PC)", EditorStyles.boldLabel);

                        for (int i = 0; i < inputControlsManager.inputNames.Count; i++)
                        {
                            using (new GUILayout.VerticalScope("box"))
                            {
                                EditorGUIUtility.labelWidth = 115;
                                inputControlsManager.keyCodeCursorEvent[i] = EditorGUILayout.Popup(inputControlsManager.inputNames[i], inputControlsManager.keyCodeCursorEvent[i], InputControlsManager.CursorEvents, GUILayout.Width(280));

                                if (inputControlsManager.keyCodeCursorEvent[i] == 1)
                                {
                                    EditorGUILayout.LabelField("It will not click in UI elements with \"Raycast Target\" true.", EditorStyles.miniLabel);
                                }
                            }
                        }

                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUILayout.LabelField("Time to keep pressed swipe functions", EditorStyles.boldLabel);

                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 85;
                            inputControlsManager.timeToKeepSwipeUp = EditorGUILayout.FloatField("Swipe up: ", inputControlsManager.timeToKeepSwipeUp);
                            EditorGUILayout.Space();
                            inputControlsManager.timeToKeepSwipeDown = EditorGUILayout.FloatField("Swipe down: ", inputControlsManager.timeToKeepSwipeDown);
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 85;
                            inputControlsManager.timeToKeepSwipeLeft = EditorGUILayout.FloatField("Swipe left: ", inputControlsManager.timeToKeepSwipeLeft);
                            EditorGUILayout.Space();
                            inputControlsManager.timeToKeepSwipeRight = EditorGUILayout.FloatField("Swipe right: ", inputControlsManager.timeToKeepSwipeRight);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            EditorGUILayout.HelpBox("It's used when the function need to be called constantly. Example: The function to go up ladders need be called every frame, to detects if the input to go up the ladder is being pressed, in this case this timer is required, to know by how many time the input to go up the ladder will keeps pressed.", MessageType.Info);
                        }
                    }
                }
            }
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /
        EditorUtility.SetDirty(inputControlsManager);
        Undo.RecordObject(inputControlsManager, "Undo inputControlsManager");
    }

    string[] convertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }

}
