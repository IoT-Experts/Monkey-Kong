using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class WeaponFactoryEditor : EditorWindow
{
    WeaponFactory weaponFactory;
    Vector2 scrollPos;
    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Bullet creator", "Weapon creator", "Weapon categories and options" };
    Texture2D weaponPreview;
    Texture2D bulletPreview;

    [MenuItem("Tools/Window/Weapon factory")]
    private static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(WeaponFactoryEditor), false, "Weapon factory");
    }

    void OnEnable()
    {
        weaponFactory = AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/WeaponFactory.asset", typeof(WeaponFactory)) as WeaponFactory;
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
                BulletCreator();
                break;
            case 1:
                WeaponCreator();
                break;
            case 2:
                WeaponOptions();
                break;
        }


        //-------------------------------------------------------------------------------------- /
        //End scroll view
        EditorGUILayout.EndScrollView();
        //-------------------------------------------------------------------------------------- /

        EditorUtility.SetDirty(weaponFactory);
        Undo.RecordObject(weaponFactory, "Undo weaponFactory");
    }

    void BulletCreator()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("Bullet creator", "window", GUILayout.Width(200)))
        {
            for (int i = 0; i < weaponFactory.bulletData.Count; i++)
            {
                using (new GUILayout.VerticalScope("helpbox"))
                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        weaponFactory.bulletData.RemoveAt(i);
                        weaponFactory.bulletNames.RemoveAt(i);
                        if (weaponFactory.bulletData.Count > 0)
                            i = 0;
                        else
                            break;
                    }

                    bulletPreview = AssetPreview.GetAssetPreview(weaponFactory.bulletData[i].bulletGameObject);
                    GUILayout.Label(bulletPreview, GUILayout.Height(45), GUILayout.Width(45));
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(45)))
                    {
                        EditorGUIUtility.labelWidth = 50;
                        weaponFactory.bulletNames[i] = EditorGUILayout.TextField("Name: ", weaponFactory.bulletNames[i], GUILayout.Width(200));
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    using (new GUILayout.VerticalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 80;
                        weaponFactory.bulletData[i].bulletType = (BulletData.Type)EditorGUILayout.EnumPopup("Bullet type: ", weaponFactory.bulletData[i].bulletType, GUILayout.Width(180));

                        if(weaponFactory.bulletData[i].bulletType == BulletData.Type.Grenade)
                        {
                            EditorGUIUtility.labelWidth = 192;
                            weaponFactory.bulletData[i].countdownToActivateGranade = EditorGUILayout.FloatField("Countdown to activate grenade: ", weaponFactory.bulletData[i].countdownToActivateGranade, GUILayout.Width(230));
                            EditorGUIUtility.labelWidth = 100;
                            weaponFactory.bulletData[i].activationMode = (BulletData.ActivationMode)EditorGUILayout.EnumPopup("Activation Mode: ", weaponFactory.bulletData[i].activationMode, GUILayout.Width(235));
                        }
                    }
                    using (new GUILayout.HorizontalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 57;
                        weaponFactory.bulletData[i].bulletVelocity = EditorGUILayout.FloatField("Velocity ", weaponFactory.bulletData[i].bulletVelocity, GUILayout.Width(140));
                    }

                    using (new GUILayout.HorizontalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 83;
                        weaponFactory.bulletData[i].maxDistance = EditorGUILayout.FloatField("Max distance ", weaponFactory.bulletData[i].maxDistance, GUILayout.Width(150));
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    using (new GUILayout.HorizontalScope("helpbox"))
                    {
                        EditorGUIUtility.labelWidth = 60;
                        weaponFactory.bulletData[i].bulletGameObject = EditorGUILayout.ObjectField((weaponFactory.bulletData[i].bulletType == BulletData.Type.NormalBullet) ? "Bullet " : "Grenade ", weaponFactory.bulletData[i].bulletGameObject, typeof(GameObject), false, GUILayout.Width(240)) as GameObject;
                    }
                    if (weaponFactory.bulletData[i].bulletType == BulletData.Type.NormalBullet)
                    {
                        using (new GUILayout.HorizontalScope("helpbox"))
                        {
                            EditorGUIUtility.labelWidth = 100;
                            weaponFactory.bulletData[i].collisionEffectGameObject = EditorGUILayout.ObjectField("Collision Effect ", weaponFactory.bulletData[i].collisionEffectGameObject, typeof(GameObject), false, GUILayout.Width(280)) as GameObject;
                        }
                    }

                    if (weaponFactory.bulletData[i].bulletType == BulletData.Type.Grenade)
                    {
                        using (new GUILayout.VerticalScope("helpbox"))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 115;
                            weaponFactory.bulletData[i].explosionOfGrenadeGameObject = EditorGUILayout.ObjectField("Grenade Explosion", weaponFactory.bulletData[i].explosionOfGrenadeGameObject, typeof(GameObject), false, GUILayout.Width(280)) as GameObject;

                            if (GUILayout.Button("?", EditorStyles.miniButton, GUILayout.Width(20)))
                            {
                                if (!EditorPrefs.GetBool("showInformationAboutGrenade", false))
                                    EditorPrefs.SetBool("showInformationAboutGrenade", true);
                                else
                                    EditorPrefs.SetBool("showInformationAboutGrenade", false);
                            }
                            EditorGUILayout.EndHorizontal();
                            if (EditorPrefs.GetBool("showInformationAboutGrenade", false))
                            {
                                EditorGUILayout.HelpBox("The grenade explosion will be instantiated when the countdown finish. You should to add the corresponding components to the explosion object to cause damage and to destroy itself.", MessageType.Info);
                            }

                            if (weaponFactory.bulletData[i].explosionOfGrenadeGameObject != null && weaponFactory.bulletData[i].explosionOfGrenadeGameObject.GetComponent<SwitchOfComponent>() == null)
                            {
                                EditorGUILayout.HelpBox("Is necessary to add the component SwitchOfComponent to the grenade explosion to destroy itself later of be instantiated.", MessageType.Warning);
                            }
                            else if (weaponFactory.bulletData[i].explosionOfGrenadeGameObject == null)
                            {
                                EditorGUILayout.HelpBox("Please add the prefab (The explosion) to instantiate when the countdown finish.", MessageType.Error);
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    if (weaponFactory.bulletData[i].bulletGameObject != null && weaponFactory.bulletData[i].bulletGameObject.GetComponent<BulletOfWeapon>() == null)
                    {
                        if(weaponFactory.bulletData[i].bulletType == BulletData.Type.NormalBullet)
                            EditorGUILayout.HelpBox("The bullet should have the component \"BulletOfWeapon\".", MessageType.Warning);
                        else
                            EditorGUILayout.HelpBox("The grenade should have the component \"BulletOfWeapon\".", MessageType.Warning);
                    }

                    if (weaponFactory.bulletData[i].collisionEffectGameObject != null && weaponFactory.bulletData[i].collisionEffectGameObject.GetComponent<SwitchOfComponent>() == null)
                    {
                        EditorGUILayout.HelpBox("The collision effect should have the component \"SwitchOfComponent\" to destroy to itself, else, the object will keep in the scene.", MessageType.Info);
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (weaponFactory.bulletData[i].bulletGameObject == null)
                    {
                        if (GUILayout.Button("Create bullet example", EditorStyles.miniButton))
                        {
                            Camera sceneCam = SceneView.lastActiveSceneView.camera;
                            Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                            Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/Weapon/BulletX.prefab", typeof(GameObject)), new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                        }
                    }

                    if (weaponFactory.bulletData[i].collisionEffectGameObject == null && weaponFactory.bulletData[i].bulletType == BulletData.Type.NormalBullet)
                    {
                        if (GUILayout.Button("Create collision effect example", EditorStyles.miniButton))
                        {
                            Camera sceneCam = SceneView.lastActiveSceneView.camera;
                            Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                            Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/Weapon/CollisionEffectX.prefab", typeof(GameObject)), new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create new bullet", GUILayout.Width(310)))
            {
                weaponFactory.bulletData.Add(new BulletData(null, null, 1, 25, BulletData.Type.NormalBullet, 0, BulletData.ActivationMode.WhenCollides, null));
                weaponFactory.bulletNames.Add("");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void WeaponCreator()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        using (new GUILayout.VerticalScope("Weapon creator", "window", GUILayout.Width(200)))
        {
            for (int i = 0; i < weaponFactory.weaponsData.Count; i++)
            {
                using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(30)))
                {
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                    {
                        weaponFactory.weaponsData.RemoveAt(i);
                        weaponFactory.weaponNames.RemoveAt(i);
                        if (weaponFactory.weaponsData.Count > 0)
                            i = 0;
                        else
                            break;
                    }

                    weaponPreview = AssetPreview.GetAssetPreview(weaponFactory.weaponsData[i].weaponGameObject);

                    if (weaponPreview == null)
                        weaponPreview = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/meleeIcon.png", typeof(Texture2D)) as Texture2D;

                    GUILayout.Label(weaponPreview, GUILayout.Height(45), GUILayout.Width(45));
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 55;
                        weaponFactory.weaponNames[i] = EditorGUILayout.TextField("Name: ", weaponFactory.weaponNames[i], GUILayout.Width(190));
                    }

                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 160;
                        weaponFactory.weaponsData[i].startInWeaponInventory = EditorGUILayout.Toggle("Start in weapon inventory: ", weaponFactory.weaponsData[i].startInWeaponInventory, GUILayout.Width(175));
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 85;
                        weaponFactory.weaponsData[i].weaponType = (WeaponFactory.WeaponType)EditorGUILayout.EnumPopup("Weapon type: ", weaponFactory.weaponsData[i].weaponType, GUILayout.Width(180));
                    }

                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 70;
                        weaponFactory.weaponsData[i].weaponCategory = EditorGUILayout.Popup("Category: ", weaponFactory.weaponsData[i].weaponCategory, ConvertListStringToArray(weaponFactory.weaponCategories), GUILayout.Width(170));
                    }

                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 60;
                        weaponFactory.weaponsData[i].weaponGameObject = EditorGUILayout.ObjectField("Weapon ", weaponFactory.weaponsData[i].weaponGameObject, typeof(GameObject), false, GUILayout.Width(170)) as GameObject;
                    }


                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 90;
                        weaponFactory.weaponsData[i].infiniteBullets = EditorGUILayout.Toggle((weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun) ? "Infinite bullets: " : "Infinite attacks: ", weaponFactory.weaponsData[i].infiniteBullets, GUILayout.Width(105));
                        if (!weaponFactory.weaponsData[i].infiniteBullets)
                        {
                            EditorGUIUtility.labelWidth = 145;
                            weaponFactory.weaponsData[i].defaultBulletQuantity = EditorGUILayout.IntField((weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun) ? "Bullet quantity (Default): " : "Max attacks (Default): ", weaponFactory.weaponsData[i].defaultBulletQuantity, GUILayout.Width(180));
                        }
                    }

                    using (new GUILayout.HorizontalScope("helpbox", GUILayout.Width(140)))
                    {
                        EditorGUIUtility.labelWidth = 120;
                        weaponFactory.weaponsData[i].timeToNextShot = EditorGUILayout.FloatField((weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun) ? "Time to next shot: " : "Time to next attack: ", weaponFactory.weaponsData[i].timeToNextShot, GUILayout.Width(160));
                    }
                    EditorGUILayout.EndHorizontal();

                    if (weaponFactory.weaponsData[i].timeToNextShot <= 0.12f)
                    {
                        string txt = (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun) ? "Time to next shot" : "Time to next attack";
                        EditorGUILayout.HelpBox(txt + " is less or equal to 0.12, this could have problems with the animations or sounds that depends of the state PlayerWeaponAttack. If you are not using this state don't worry, else, raise the value.", MessageType.Info);
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginHorizontal("helpbox");
                    if (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.MeleeWeapon)
                    {
                        EditorGUIUtility.labelWidth = 100;
                        weaponFactory.weaponsData[i].attackDuration = EditorGUILayout.FloatField("Attack duration: ", weaponFactory.weaponsData[i].attackDuration, GUILayout.Width(150));
                    }

                    if ((weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun))
                    {
                        EditorGUIUtility.labelWidth = 100;
                        weaponFactory.weaponsData[i].shootingEffectGameObject = EditorGUILayout.ObjectField("Shooting Effect ", weaponFactory.weaponsData[i].shootingEffectGameObject, typeof(GameObject), false, GUILayout.Width(214)) as GameObject;
                        EditorGUIUtility.labelWidth = 50;
                        weaponFactory.weaponsData[i].bulletToShoot = EditorGUILayout.Popup("Bullet ", weaponFactory.weaponsData[i].bulletToShoot, ConvertListStringToArray(weaponFactory.bulletNames), GUILayout.Width(150));
                    }

                    EditorGUILayout.EndHorizontal();

                    if (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.MeleeWeapon)
                    {
                        EditorGUILayout.BeginVertical("helpbox");
                        EditorGUIUtility.labelWidth = 155;

                        EditorGUILayout.BeginHorizontal();
                        weaponFactory.weaponsData[i].delayTimeBeforeAttack = EditorGUILayout.FloatField("Delay time before attack: ", weaponFactory.weaponsData[i].delayTimeBeforeAttack, GUILayout.Width(200));

                        if (GUILayout.Button("?", EditorStyles.miniButton, GUILayout.Width(20)))
                        {
                            if (!EditorPrefs.GetBool("showInfoAboutDelayTimeBeforeAttack", false))
                            {
                                EditorPrefs.SetBool("showInfoAboutDelayTimeBeforeAttack", true);
                            }
                            else
                            {
                                EditorPrefs.SetBool("showInfoAboutDelayTimeBeforeAttack", false);
                            }
                        }

                        EditorGUILayout.EndHorizontal();

                        if (EditorPrefs.GetBool("showInfoAboutDelayTimeBeforeAttack", false))
                        {
                            EditorGUILayout.HelpBox("This value allows you to define a time to wait before enable the collider of the weapon. You can use this for example to wait while the player lift up the weapon in the animation, and later enable the collider to cause dammage, and avoid to cause dammage from start.", MessageType.Info);
                        }

                        EditorGUILayout.EndVertical();
                    }

                    EditorGUIUtility.labelWidth = 175;
                    EditorGUILayout.BeginVertical("helpbox");
                    weaponFactory.weaponsData[i].stopMovementWhenAttack = EditorGUILayout.Toggle("Stop movement when attack: ", weaponFactory.weaponsData[i].stopMovementWhenAttack);

                    if (weaponFactory.weaponsData[i].stopMovementWhenAttack)
                    {
                        EditorGUIUtility.labelWidth = 100;
                        weaponFactory.weaponsData[i].stoppingTime = EditorGUILayout.FloatField("Stopping Time: ", weaponFactory.weaponsData[i].stoppingTime);
                        EditorGUIUtility.labelWidth = 195;
                        weaponFactory.weaponsData[i].iUseACustomMovementScript = EditorGUILayout.Toggle("I use a custom movement script: ", weaponFactory.weaponsData[i].iUseACustomMovementScript, GUILayout.Width(210));
                        if(weaponFactory.weaponsData[i].iUseACustomMovementScript)
                        {
                            EditorGUIUtility.labelWidth = 140;
                            weaponFactory.weaponsData[i].movementScript = EditorGUILayout.TextField("Movement script name: ", weaponFactory.weaponsData[i].movementScript);
                            EditorGUILayout.HelpBox("The movement script will be disabled for the 'Stopping Time'. You should to write the name of the movement script of the character that will use this weapon.", MessageType.Info);
                        }
                    }

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.EndHorizontal();

                    if (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun)
                    {
                        using (new GUILayout.VerticalScope("helpbox", GUILayout.Width(100)))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 205;
                            weaponFactory.weaponsData[i].delayTimeBeforeShotActivation = EditorGUILayout.FloatField("Delay time before shot activation: ", weaponFactory.weaponsData[i].delayTimeBeforeShotActivation, GUILayout.Width(280));

                            if (GUILayout.Button("?", EditorStyles.miniButton, GUILayout.Width(20)))
                            {
                                if (!EditorPrefs.GetBool("showInfoAboutDelayTimeBeforeShotActivation", false))
                                {
                                    EditorPrefs.SetBool("showInfoAboutDelayTimeBeforeShotActivation", true);
                                }
                                else
                                {
                                    EditorPrefs.SetBool("showInfoAboutDelayTimeBeforeShotActivation", false);
                                }
                            }

                            EditorGUILayout.EndHorizontal();

                            if (EditorPrefs.GetBool("showInfoAboutDelayTimeBeforeShotActivation", false))
                            {
                                EditorGUILayout.HelpBox("This value allows you to define a time to wait before send the bullet. You can use this for example to wait while the player lift up the weapon in the animation, and later enable the shooting, this avoid to shot instantly.", MessageType.Info);
                            }
                        }
                    }

                    if (weaponFactory.weaponsData[i].shootingEffectGameObject != null && weaponFactory.weaponsData[i].shootingEffectGameObject.GetComponent<SwitchOfComponent>() == null)
                    {
                        EditorGUILayout.LabelField("*The shooting effect should have the component \"SwitchOfComponent\" to destroy to itself.", EditorStyles.miniLabel);
                    }

                    if (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (weaponFactory.weaponsData[i].weaponGameObject == null)
                        {
                            if (GUILayout.Button("Create gun example", EditorStyles.miniButton))
                            {
                                Camera sceneCam = SceneView.lastActiveSceneView.camera;
                                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/Weapon/GunX.prefab", typeof(GameObject)), new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                            }
                        }

                        if (weaponFactory.weaponsData[i].shootingEffectGameObject == null)
                        {
                            if (GUILayout.Button("Create shooting effect example", EditorStyles.miniButton))
                            {
                                Camera sceneCam = SceneView.lastActiveSceneView.camera;
                                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/Weapon/ShootingEffectX.prefab", typeof(GameObject)), new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (weaponFactory.weaponsData[i].weaponGameObject == null)
                        {
                            if (GUILayout.Button("Create melee weapon example", EditorStyles.miniButton))
                            {
                                Camera sceneCam = SceneView.lastActiveSceneView.camera;
                                Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/Weapon/HammerWeapon.prefab", typeof(GameObject)), new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                            }
                        }
                    }

                    if (weaponFactory.weaponsData[i].weaponType == WeaponFactory.WeaponType.Gun)
                    {
                        if (weaponFactory.weaponsData[i].weaponGameObject != null && weaponFactory.weaponsData[i].weaponGameObject.transform.FindChild("BulletInstantiator") == null)
                        {
                            EditorGUILayout.HelpBox("The GameObject attached should have a child with name \"BulletInstantiator\".\nThe BulletInstantiator child will be the position to shoot the bullet.", MessageType.Warning);
                        }
                    }
                }

                EditorGUILayout.Space();
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create new gun", GUILayout.Width(280)))
            {
                weaponFactory.weaponsData.Add(new WeaponData(null, null, 0, true, 0.5f, 0, WeaponFactory.WeaponType.Gun, 0.01f, 0, false, false, "", 0, 0));
                weaponFactory.weaponNames.Add("");
            }

            if (GUILayout.Button("Create new melee weapon", GUILayout.Width(280)))
            {
                weaponFactory.weaponsData.Add(new WeaponData(null, null, 0, true, 0.8f, 0, WeaponFactory.WeaponType.MeleeWeapon, 0.3f, 0, true, false, "", 0, 0));
                weaponFactory.weaponNames.Add("");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }


    void WeaponOptions ()
    {
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        using (new GUILayout.HorizontalScope("Weapon categories and options","window", GUILayout.Height(30)))
        {
            //Weapon categories ----------------------------------------------------------------------- /
            using (new GUILayout.VerticalScope("Weapon categories", "window"))
            {
                EditorGUIUtility.labelWidth = 80;
                using (new GUILayout.VerticalScope("helpbox"))
                {
                    for (int i = 0; i < weaponFactory.weaponCategories.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        weaponFactory.weaponCategories[i] = EditorGUILayout.TextField("Category " + i + ": ", weaponFactory.weaponCategories[i], GUILayout.Width(230));
                        if (i != 0)
                        {
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                weaponFactory.weaponCategories.RemoveAt(i);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                if (GUILayout.Button("Add new weapon category"))
                {
                    weaponFactory.weaponCategories.Add("");
                }
            }
            //end Weapon categories ----------------------------------------------------------------------- /
            EditorGUILayout.Space();
            //Weapon options ----------------------------------------------------------------------- /
            using (new GUILayout.VerticalScope("Weapon options", "window", GUILayout.Height(30)))
            {
                using (new GUILayout.VerticalScope("helpbox"))
                {
                    EditorGUIUtility.labelWidth = 190;
                    weaponFactory.startWithGunTheFirsTime = EditorGUILayout.Toggle("Start with weapon the first time: ", weaponFactory.startWithGunTheFirsTime, GUILayout.Width(215));

                    if (weaponFactory.startWithGunTheFirsTime)
                    {
                        EditorGUIUtility.labelWidth = 100;
                        weaponFactory.gunToStartByFirstTime = EditorGUILayout.Popup("Weapon to start: ", weaponFactory.gunToStartByFirstTime, ConvertListStringToArray(weaponFactory.weaponNames), GUILayout.Width(230));
                    }   
                }

                using (new GUILayout.VerticalScope("helpbox"))
                {
                    EditorGUIUtility.labelWidth = 137;
                    weaponFactory.savingTypeOfBullets = (WeaponFactory.TypeSave)EditorGUILayout.EnumPopup("Saving type of bullets: ", weaponFactory.savingTypeOfBullets, GUILayout.Width(290));
                }
            }
            //end Weapon options ----------------------------------------------------------------------- /
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    string[] ConvertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }

}
