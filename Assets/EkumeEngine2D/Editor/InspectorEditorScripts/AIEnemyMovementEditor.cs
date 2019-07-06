using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using EkumeEnumerations;

[CustomEditor(typeof(AIEnemyMovement))]
public class AIEnemyMovementEditor : Editor
{
    AIEnemyMovement aIEnemyMovement;
    Texture2D leftDirectionHeadDisabled;
    Texture2D leftDirectionHeadEnabled;

    Texture2D rightDirectionHeadDisabled;
    Texture2D rightDirectionHeadEnabled;

    void OnEnable()
    {
        aIEnemyMovement = (AIEnemyMovement)target;

        leftDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        leftDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;

        rightDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        rightDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            aIEnemyMovement.currentDirection = (DirectionsXAxisEnum)EditorGUILayout.EnumPopup("Current direction: ", aIEnemyMovement.currentDirection);

            using (var horizontalScope0 = new GUILayout.HorizontalScope("box"))
            {
                if (GUILayout.Button((aIEnemyMovement.currentDirection == DirectionsXAxisEnum.Left) ? leftDirectionHeadEnabled : leftDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    aIEnemyMovement.currentDirection = DirectionsXAxisEnum.Left;
                }

                if (GUILayout.Button((aIEnemyMovement.currentDirection == DirectionsXAxisEnum.Right) ? rightDirectionHeadEnabled : rightDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    aIEnemyMovement.currentDirection = DirectionsXAxisEnum.Right;
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Movement options", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope("box"))
            {
                aIEnemyMovement.constantMovement = EditorGUILayout.Toggle("Constant movement: ", aIEnemyMovement.constantMovement);
                if(aIEnemyMovement.constantMovement)
                    aIEnemyMovement.movementSpeed = EditorGUILayout.FloatField("Movement speed: ", aIEnemyMovement.movementSpeed);
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Reduce velocity little by little when it stop", EditorStyles.miniLabel);
                aIEnemyMovement.constantReductionOfVelocity = EditorGUILayout.Toggle("Activate: ", aIEnemyMovement.constantReductionOfVelocity);

                if (aIEnemyMovement.constantReductionOfVelocity)
                {
                    aIEnemyMovement.speedToReduceVelocity = EditorGUILayout.FloatField("Speed of reduction: ", aIEnemyMovement.speedToReduceVelocity);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Increase velocity gradually until reach max velocity", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("(The max velocity is the Movement speed)", EditorStyles.miniLabel);
                aIEnemyMovement.gradualVelocity = EditorGUILayout.Toggle("Activate: ", aIEnemyMovement.gradualVelocity);

                if (aIEnemyMovement.gradualVelocity)
                {
                    aIEnemyMovement.speedToIncreaseVelocity = EditorGUILayout.FloatField("Speed of increase: ", aIEnemyMovement.speedToIncreaseVelocity);
                }
            }


            using (new GUILayout.VerticalScope("box"))
            {

                using (new GUILayout.VerticalScope("box"))
                {
                    aIEnemyMovement.followTarget = EditorGUILayout.Toggle("Follow target: ", aIEnemyMovement.followTarget);

                    if (aIEnemyMovement.followTarget)
                    {
                        aIEnemyMovement.targetDetector = EditorGUILayout.ObjectField("Target detector: ", aIEnemyMovement.targetDetector, typeof(TargetDetector), true) as TargetDetector;
                        aIEnemyMovement.distanceOfTarget = EditorGUILayout.FloatField("Distance of target: ", aIEnemyMovement.distanceOfTarget);
                        aIEnemyMovement.followType = (AIEnemyMovement.FollowType)EditorGUILayout.EnumPopup("Tracking type: ", aIEnemyMovement.followType);

                        EditorGUIUtility.labelWidth = 180;
                        aIEnemyMovement.movementSpeedToFollow = EditorGUILayout.FloatField("Movement speed to follow: ", aIEnemyMovement.movementSpeedToFollow);
                        EditorGUIUtility.labelWidth = 150;
                        if (aIEnemyMovement.GetComponentInChildren<TargetDetector>() == null)
                        {
                            using (new GUILayout.VerticalScope("box"))
                            {
                                EditorGUILayout.LabelField("You have not a child with the component \"TargetDetector\".", EditorStyles.miniLabel);

                                if (GUILayout.Button("Create child with the component"))
                                {
                                    GameObject objectCreated = new GameObject();
                                    objectCreated.AddComponent<CircleCollider2D>();
                                    objectCreated.GetComponent<CircleCollider2D>().isTrigger = true;
                                    objectCreated.GetComponent<CircleCollider2D>().radius = 6.3f;
                                    objectCreated.AddComponent<TargetDetector>();
                                    objectCreated.transform.SetParent(aIEnemyMovement.transform);
                                    objectCreated.transform.position = aIEnemyMovement.transform.position;
                                    objectCreated.name = "TargetDetector";
                                    Selection.activeGameObject = objectCreated;
                                    EditorGUIUtility.PingObject(objectCreated);
                                    Debug.Log("The GameObject \"TargetDetector\" of type TargetDetector was created.", objectCreated);
                                    aIEnemyMovement.targetDetector = objectCreated.GetComponent<TargetDetector>();
                                }
                            }
                        }


                    }
                }

                if (aIEnemyMovement.followTarget && aIEnemyMovement.constantMovement)
                {
                    EditorGUILayout.HelpBox("The enemy will start with constant movement. If find the target, the constant movement will be disrupted to follow the target.", MessageType.Info);
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("What to do when find limit to walk", EditorStyles.boldLabel);

            using (new GUILayout.VerticalScope("box"))
            {
                aIEnemyMovement.jumpInLimit = EditorGUILayout.Toggle("Jump in limit: ", aIEnemyMovement.jumpInLimit);

                if (aIEnemyMovement.jumpInLimit)
                {
                    aIEnemyMovement.jumpForce = EditorGUILayout.FloatField("Jump force", aIEnemyMovement.jumpForce);

                    if(aIEnemyMovement.invertVelocityInLimit && aIEnemyMovement.followTarget)
                    {
                        EditorGUILayout.HelpBox("The option of Jump will be ignored while the enemy is not following the target because you have selected the option \"Invert velocity in limit\".", MessageType.Info);
                    }
                }
            }

            if (aIEnemyMovement.jumpInLimit || aIEnemyMovement.invertVelocityInLimit)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    aIEnemyMovement.groundChecker = EditorGUILayout.ObjectField("Ground checker: ", aIEnemyMovement.groundChecker, typeof(Transform), true) as Transform;
                }
            }

            if (aIEnemyMovement.constantMovement)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    aIEnemyMovement.invertVelocityInLimit = EditorGUILayout.Toggle("Invert velocity in limit: ", aIEnemyMovement.invertVelocityInLimit);
                    if (aIEnemyMovement.invertVelocityInLimit)
                    {
                        if (aIEnemyMovement.followTarget || aIEnemyMovement.jumpInLimit)
                        {
                            EditorGUILayout.HelpBox("When the enemy start to follow the target, will ignore this option because you have the option \"Follow target\" or \"Jump in limit\" active", MessageType.Info);
                        }
                    }
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                aIEnemyMovement.detectsGroundInFront = EditorGUILayout.Toggle("Detects ground in front: ", aIEnemyMovement.detectsGroundInFront);
                if (aIEnemyMovement.detectsGroundInFront)
                    aIEnemyMovement.frontGroundDetector = EditorGUILayout.ObjectField("Front ground detector: ", aIEnemyMovement.frontGroundDetector, typeof(Transform), true) as Transform;
            }

            using (new GUILayout.VerticalScope("box"))
            {
                if(aIEnemyMovement.jumpInLimit)
                    EditorGUILayout.LabelField("Will jump when it detects an obstacle", EditorStyles.miniLabel);
                else if(aIEnemyMovement.invertVelocityInLimit)
                    EditorGUILayout.LabelField("Will return when it detects an obstacle.", EditorStyles.miniLabel);
                aIEnemyMovement.detectsWallInFront = EditorGUILayout.Toggle("Detects wall in front: ", aIEnemyMovement.detectsWallInFront);

                if (aIEnemyMovement.detectsWallInFront)
                {
                    if (aIEnemyMovement.GetComponentInChildren<ObstacleDetectorForAIEnemy>() == null)
                    {
                        EditorGUILayout.LabelField("You have not a child with the component \"ObstacleDetectorForIAEnemy\".", EditorStyles.miniLabel);

                        if(GUILayout.Button("Create child with the component"))
                        {
                            GameObject objectCreated = new GameObject();
                            objectCreated.AddComponent<BoxCollider2D>();
                            objectCreated.GetComponent<BoxCollider2D>().isTrigger = true;
                            objectCreated.GetComponent<BoxCollider2D>().size = new Vector2(0.202f, 1.916f);
                            objectCreated.AddComponent<ObstacleDetectorForAIEnemy>();
                            objectCreated.transform.SetParent(aIEnemyMovement.transform);
                            objectCreated.transform.position = aIEnemyMovement.transform.position;
                            objectCreated.name = "ObstacleDetector";
                            Selection.activeGameObject = objectCreated;
                            EditorGUIUtility.PingObject(objectCreated);
                            Debug.Log("The GameObject \"ObstacleDetector\" of type ObstacleDetectorForIAEnemy was created.", objectCreated);
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(aIEnemyMovement);
        Undo.RecordObject(aIEnemyMovement, "Undo iAEnemyMovement");
    }
}
