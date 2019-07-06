using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;

[CustomEditor(typeof(PlayerPowers))]

public class PlayerPowersEditor : Editor
{

    PlayerPowers playerPowers;

    void OnEnable()
    {
        playerPowers = (PlayerPowers)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Powers manager", EditorStyles.boldLabel);

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power to fly", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 110;

                playerPowers.usePowerToFly = EditorGUILayout.Toggle("Use power to fly: ", playerPowers.usePowerToFly);

                if (playerPowers.usePowerToFly)
                {
                        using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                        {
                            EditorGUIUtility.labelWidth = 155;
                            playerPowers.showCountdownOfPowerToFly = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerToFly);

                            if (playerPowers.showCountdownOfPowerToFly)
                            {
                                playerPowers.imageCountdownToFly = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownToFly, typeof(Image), true) as Image;

                                playerPowers.imageCountdownToFly = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownToFly);

                                if (playerPowers.imageCountdownToFly != null && playerPowers.imageCountdownToFly.type != Image.Type.Filled)
                                {
                                    EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                                }
                            }
                        
                    }
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        playerPowers.jumpForceToFly = EditorGUILayout.FloatField("Force to fly: ", playerPowers.jumpForceToFly);
                    }
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of object magnet: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 170;

                playerPowers.usePowerObjectMagnet = EditorGUILayout.Toggle("Use power of object magnet: ", playerPowers.usePowerObjectMagnet);

                if (playerPowers.usePowerObjectMagnet)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerObjectMagnet = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerObjectMagnet);

                        if (playerPowers.showCountdownOfPowerObjectMagnet)
                        {
                            playerPowers.imageCountdownObjectMagnet = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownObjectMagnet, typeof(Image), true) as Image;

                            playerPowers.imageCountdownObjectMagnet = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownObjectMagnet);

                            if (playerPowers.imageCountdownObjectMagnet != null && playerPowers.imageCountdownObjectMagnet.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }


                    using (new GUILayout.VerticalScope("box"))
                    {
                        playerPowers.velocityToAttract = EditorGUILayout.FloatField("Velocity to attract: ", playerPowers.velocityToAttract);
                    }

                    using (new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("Tags of the GameObjects to attract with magnet", EditorStyles.boldLabel);

                        for (int i = 0; i < playerPowers.tagsToAttract.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            playerPowers.tagsToAttract[i] = EditorGUILayout.TagField(playerPowers.tagsToAttract[i]);
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                playerPowers.tagsToAttract.RemoveAt(i);
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add new tag to attract with magnet", EditorStyles.miniButton))
                        {
                            playerPowers.tagsToAttract.Add("");
                        }
                    }

                    EditorGUILayout.HelpBox("Add the script ObjectMagnetPower to an object with a Collider2D of type trigger, and set like child of the player to attract the objects when the power is active.", MessageType.Info);
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of Score duplicator: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 215;

                playerPowers.usePowerScoreDuplicator = EditorGUILayout.Toggle("Use power of Score duplicator: ", playerPowers.usePowerScoreDuplicator);

                if (playerPowers.usePowerScoreDuplicator)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerScoreDuplicator = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerScoreDuplicator);

                        if (playerPowers.showCountdownOfPowerScoreDuplicator)
                        {
                            playerPowers.imageCountdownScoreDuplicator = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownScoreDuplicator, typeof(Image), true) as Image;

                            playerPowers.imageCountdownScoreDuplicator = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownScoreDuplicator);

                            if (playerPowers.imageCountdownScoreDuplicator != null && playerPowers.imageCountdownScoreDuplicator.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }                   

                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("Scores to duplicate", EditorStyles.boldLabel);
                        for (int i = 0; i < playerPowers.ScoresToDuplicate.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            playerPowers.ScoresToDuplicate[i] = EditorGUILayout.Popup(playerPowers.ScoresToDuplicate[i], ScoresList());
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                playerPowers.ScoresToDuplicate.RemoveAt(i);
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add Score to duplicate", EditorStyles.miniButton))
                        {
                            playerPowers.ScoresToDuplicate.Add(0);
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of protector shield: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 175;

                playerPowers.usePowerProtectorShield = EditorGUILayout.Toggle("Use power of protector shield: ", playerPowers.usePowerProtectorShield);

                if (playerPowers.usePowerProtectorShield)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerProtectorShield = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerProtectorShield);

                        if (playerPowers.showCountdownOfPowerProtectorShield)
                        {
                            playerPowers.imageCountdownProtectorShield = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownProtectorShield, typeof(Image), true) as Image;

                            playerPowers.imageCountdownProtectorShield = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownProtectorShield);

                            if (playerPowers.imageCountdownProtectorShield != null && playerPowers.imageCountdownProtectorShield.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of traps converter: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 175;

                playerPowers.usePowerTrapsConverter = EditorGUILayout.Toggle("Use power of traps converter: ", playerPowers.usePowerTrapsConverter);

                if (playerPowers.usePowerTrapsConverter)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerTrapsConverter = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerTrapsConverter);

                        if (playerPowers.showCountdownOfPowerTrapsConverter)
                        {
                            playerPowers.imageCountdownTrapsConverter = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownTrapsConverter, typeof(Image), true) as Image;

                            playerPowers.imageCountdownTrapsConverter = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownTrapsConverter);

                            if (playerPowers.imageCountdownTrapsConverter != null && playerPowers.imageCountdownTrapsConverter.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }
                

                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField("Tag of the objects to convert", EditorStyles.boldLabel);
                        for (int i = 0; i < playerPowers.tagOfObjectsToConvert.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            playerPowers.tagOfObjectsToConvert[i] = EditorGUILayout.TagField(playerPowers.tagOfObjectsToConvert[i]);
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                playerPowers.tagOfObjectsToConvert.RemoveAt(i);
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add tag of object to convert", EditorStyles.miniButton))
                        {
                            playerPowers.tagOfObjectsToConvert.Add("");
                        }
                    }

                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        playerPowers.replacementObject = EditorGUILayout.ObjectField("Replacement object: ", playerPowers.replacementObject, typeof(GameObject), true) as GameObject;
                    }
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of killer shield: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 150;

                playerPowers.usePowerKillerShield = EditorGUILayout.Toggle("Use power of killer shield: ", playerPowers.usePowerKillerShield);

                if (playerPowers.usePowerKillerShield)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerKillerShield = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerKillerShield);

                        if (playerPowers.showCountdownOfPowerKillerShield)
                        {
                            playerPowers.imageCountdownKillerShield = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownKillerShield, typeof(Image), true) as Image;

                            playerPowers.imageCountdownKillerShield = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownKillerShield);

                            if (playerPowers.imageCountdownKillerShield != null && playerPowers.imageCountdownKillerShield.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }                    

                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.killerShieldToActivate = EditorGUILayout.ObjectField("Killer shield to activate: ", playerPowers.killerShieldToActivate, typeof(GameObject), true) as GameObject;

                        if (playerPowers.killerShieldToActivate != null)
                        {
                            if(playerPowers.killerShieldToActivate.GetComponent<Collider2D>() == null || (playerPowers.killerShieldToActivate.GetComponent<Collider2D>() != null && !playerPowers.killerShieldToActivate.GetComponent<Collider2D>().isTrigger))
                                EditorGUILayout.LabelField("*This object should have a collider of type trigger.", EditorStyles.miniLabel);

                            if(playerPowers.killerShieldToActivate.activeSelf)
                                EditorGUILayout.LabelField("*This object should be currently disabled.", EditorStyles.miniLabel);

                                EditorGUILayout.LabelField("Note: This object should have some script to kill the enemy.", EditorStyles.miniLabel);
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Power of Jetpack: ", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 135;

                playerPowers.usePowerJetpack = EditorGUILayout.Toggle("Use power of Jetpack: ", playerPowers.usePowerJetpack);

                if (playerPowers.usePowerJetpack)
                {
                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 155;
                        playerPowers.showCountdownOfPowerJetpack = EditorGUILayout.Toggle("Show countdown (Image): ", playerPowers.showCountdownOfPowerJetpack);

                        if (playerPowers.showCountdownOfPowerJetpack)
                        {
                            playerPowers.imageCountdownJetpack = EditorGUILayout.ObjectField("Image (Type Filled): ", playerPowers.imageCountdownJetpack, typeof(Image), true) as Image;

                            playerPowers.imageCountdownJetpack = ButtonsOfInstantiateImageOfFill(playerPowers.imageCountdownJetpack);

                            if (playerPowers.imageCountdownJetpack != null && playerPowers.imageCountdownJetpack.type != Image.Type.Filled)
                            {
                                EditorGUILayout.HelpBox("*The image attached is not of type Filled.", MessageType.Error);
                            }
                        }
                    }                   

                    using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                    {
                        EditorGUIUtility.labelWidth = 190;
                        playerPowers.disableJumpWithJetpack = EditorGUILayout.Toggle("Disable jump when use Jetpack: ", playerPowers.disableJumpWithJetpack);
                        EditorGUIUtility.labelWidth = 90;
                        playerPowers.inputControlJetpack = EditorGUILayout.Popup("Jetpack input: ", playerPowers.inputControlJetpack, convertListStringToArray(InputControlsManager.instance.inputNames));

                        using (var verticalScope3 = new GUILayout.VerticalScope("box"))
                        {
                            EditorGUIUtility.labelWidth = 190;
                            playerPowers.maxVelocityOfJetpack = EditorGUILayout.FloatField("Max velocity of Jetpack: ", playerPowers.maxVelocityOfJetpack);
                            playerPowers.velocityToReachMaxVel = EditorGUILayout.FloatField("Velocity to reach max velocity:", playerPowers.velocityToReachMaxVel);
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(playerPowers);
        Undo.RecordObject(playerPowers, "Undo playerPowers");
    }

    Image ButtonsOfInstantiateImageOfFill(Image image)
    {
        if (image == null)
        {
            using (var verticalScope3 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Create examples of image filler.", EditorStyles.miniLabel);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Create horizontal", EditorStyles.miniButton))
                {
                    GameObject instantiatedObject = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/HorizontalFillerImageUI.prefab", typeof(GameObject)) as GameObject);
                    Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
                    instantiatedObject.transform.SetParent(canvas.transform);
                    instantiatedObject.transform.localScale = Vector3.one;
                    image = instantiatedObject.GetComponent<Image>();
                }

                if (GUILayout.Button("Create circular", EditorStyles.miniButton))
                {
                    GameObject instantiatedObject = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/CircularFillerImageUI.prefab", typeof(GameObject)) as GameObject);
                    Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
                    instantiatedObject.transform.SetParent(canvas.transform);
                    instantiatedObject.transform.localScale = Vector3.one;
                    image = instantiatedObject.GetComponent<Image>();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        return image;
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

    string[] ScoresList ()
    {
        string[] ScoreList = new string[ScoreTypesManager.instance.ScoresData.Count];

        for (int i = 0; i < ScoreList.Length; i++)
        {
            ScoreList[i] = ScoreTypesManager.instance.ScoresData[i].scoreName;
        }

        return ScoreList;
    }

}
