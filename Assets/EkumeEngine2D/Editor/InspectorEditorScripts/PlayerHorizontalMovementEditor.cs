using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;

[CustomEditor(typeof(PlayerHorizontalMovement))]
public class PlayerHorizontalMovementEditor : Editor
{

    PlayerHorizontalMovement playerMovement;
    Texture2D leftDirectionHeadDisabled;
    Texture2D leftDirectionHeadEnabled;

    Texture2D rightDirectionHeadDisabled;
    Texture2D rightDirectionHeadEnabled;

    void OnEnable()
    {
        playerMovement = (PlayerHorizontalMovement)target;
        leftDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        leftDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;

        rightDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        rightDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            playerMovement.currentDirection = (DirectionsXAxisEnum)EditorGUILayout.EnumPopup("Current direction: ", playerMovement.currentDirection);

            using (var horizontalScope0 = new GUILayout.HorizontalScope("box"))
            {
                if (GUILayout.Button((playerMovement.currentDirection == DirectionsXAxisEnum.Left) ? leftDirectionHeadEnabled : leftDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    playerMovement.currentDirection = DirectionsXAxisEnum.Left;
                }

                if (GUILayout.Button((playerMovement.currentDirection == DirectionsXAxisEnum.Right) ? rightDirectionHeadEnabled : rightDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    playerMovement.currentDirection = DirectionsXAxisEnum.Right;
                }
            }
            EditorGUIUtility.labelWidth = 155;
            playerMovement.howToChangeDirection = (PlayerHorizontalMovement.HowToChangeDirection)EditorGUILayout.EnumPopup("How to change direction: ", playerMovement.howToChangeDirection);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Movement options", EditorStyles.boldLabel);
            playerMovement.velocity = EditorGUILayout.FloatField("Velocity of player: ", playerMovement.velocity);

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Reduce velocity little by little when it stop", EditorStyles.miniLabel);
                playerMovement.constantReductionOfVelocity = EditorGUILayout.Toggle("Activate: ", playerMovement.constantReductionOfVelocity);

                if (playerMovement.constantReductionOfVelocity)
                {
                    playerMovement.speedToReduceVelocity = EditorGUILayout.FloatField("Speed of reduction: ", playerMovement.speedToReduceVelocity);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Increase velocity gradually until reach max velocity", EditorStyles.miniLabel);
                EditorGUILayout.LabelField("(The max velocity is the Velocity of player)", EditorStyles.miniLabel);
                playerMovement.gradualVelocity = EditorGUILayout.Toggle("Activate: ", playerMovement.gradualVelocity);

                if (playerMovement.gradualVelocity)
                {
                    playerMovement.speedToIncreaseVelocity = EditorGUILayout.FloatField("Speed of increase: ", playerMovement.speedToIncreaseVelocity);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Constant velocity", EditorStyles.boldLabel);

                EditorGUIUtility.labelWidth = 155;
                playerMovement.constantVelocityRight = EditorGUILayout.Toggle("Constant velocity to right: ", playerMovement.constantVelocityRight);

                if (playerMovement.constantVelocityRight)
                    playerMovement.constantVelocityLeft = false;

                playerMovement.constantVelocityLeft = EditorGUILayout.Toggle("Constant velocity to left: ", playerMovement.constantVelocityLeft);

                if (playerMovement.constantVelocityLeft)
                    playerMovement.constantVelocityRight = false;

                playerMovement.startConstantVelocityWithInput = EditorGUILayout.Toggle("Start with input control: ", playerMovement.startConstantVelocityWithInput);

                if (playerMovement.startConstantVelocityWithInput)
                {
                    playerMovement.inputControlToStartConstantVelocity = EditorGUILayout.Popup("Input to start: ", playerMovement.inputControlToStartConstantVelocity, convertListStringToArray(InputControlsManager.instance.inputNames));
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Input controls to move", EditorStyles.boldLabel);
                using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                {
                    playerMovement.canMoveToRight = EditorGUILayout.Toggle("Can move to right: ", playerMovement.canMoveToRight);

                    if (playerMovement.canMoveToRight)
                    {
                        playerMovement.inputControlToRight = EditorGUILayout.Popup("Control right: ", playerMovement.inputControlToRight, convertListStringToArray(InputControlsManager.instance.inputNames));
                    }
                }

                using (var verticalScope3 = new GUILayout.VerticalScope("box"))
                {
                    playerMovement.canMoveToLeft = EditorGUILayout.Toggle("Can move to left: ", playerMovement.canMoveToLeft);

                    if (playerMovement.canMoveToLeft)
                    {
                        playerMovement.inputControlToLeft = EditorGUILayout.Popup("Control left: ", playerMovement.inputControlToLeft, convertListStringToArray(InputControlsManager.instance.inputNames));
                    }
                }

                using (var verticalScope3 = new GUILayout.VerticalScope("box"))
                {
                    playerMovement.playerCanCrouchDown = EditorGUILayout.Toggle("Can crouch down: ", playerMovement.playerCanCrouchDown);

                    if (playerMovement.playerCanCrouchDown)
                    {
                        playerMovement.inputControlToCrouchDown = EditorGUILayout.Popup("Control to crouch down: ", playerMovement.inputControlToCrouchDown, convertListStringToArray(InputControlsManager.instance.inputNames));
                        playerMovement.canMoveIfCrouchDown = EditorGUILayout.Toggle("Can move if crouch down: ", playerMovement.canMoveIfCrouchDown);
                        EditorGUIUtility.labelWidth = 170;
                        playerMovement.canMoveIfJumpingCrouched = EditorGUILayout.Toggle("Can move if jump crouched: ", playerMovement.canMoveIfJumpingCrouched);
                  /*      using (var verticalScope4 = new GUILayout.VerticalScope("box"))
                        {
                            playerMovement.addImpulseIfCrouchMoving = EditorGUILayout.Toggle("Boost it if crouch moving: ", playerMovement.addImpulseIfCrouchMoving);

                            if (playerMovement.addImpulseIfCrouchMoving)
                            {
                                playerMovement.impulseToAdd = EditorGUILayout.FloatField("Boost to add: ", playerMovement.impulseToAdd);
                                playerMovement.velocityToReduceImpulse = EditorGUILayout.FloatField("Velocity to reduce boost: ", playerMovement.velocityToReduceImpulse);
                            }
                        }
                        */
                    }
                }
            }
        }

        EditorUtility.SetDirty(playerMovement);
        Undo.RecordObject(playerMovement, "Undo playerMovement");
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