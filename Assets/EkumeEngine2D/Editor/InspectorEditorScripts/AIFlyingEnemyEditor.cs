using UnityEngine;
using System.Collections;
using UnityEditor;
using EkumeEnumerations;
[CustomEditor(typeof(AIFlyingEnemy))]

public class AIFlyingEnemyEditor : Editor
{

    AIFlyingEnemy aIFlyingEnemy;

    Texture2D leftDirectionHeadDisabled;
    Texture2D leftDirectionHeadEnabled;

    Texture2D rightDirectionHeadDisabled;
    Texture2D rightDirectionHeadEnabled;

    void OnEnable()
    {
        aIFlyingEnemy = (AIFlyingEnemy)target;

        leftDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        leftDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/LeftDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;

        rightDirectionHeadDisabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadDisabled.png", typeof(Texture2D)) as Texture2D;
        rightDirectionHeadEnabled = AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Sprites/EditorIcons/RightDirectionHeadEnabled.png", typeof(Texture2D)) as Texture2D;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            aIFlyingEnemy.currentDirection = (DirectionsXAxisEnum)EditorGUILayout.EnumPopup("Current direction: ", aIFlyingEnemy.currentDirection);

            using (var horizontalScope0 = new GUILayout.HorizontalScope("box"))
            {
                if (GUILayout.Button((aIFlyingEnemy.currentDirection == DirectionsXAxisEnum.Left) ? leftDirectionHeadEnabled : leftDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    aIFlyingEnemy.currentDirection = DirectionsXAxisEnum.Left;
                }

                if (GUILayout.Button((aIFlyingEnemy.currentDirection == DirectionsXAxisEnum.Right) ? rightDirectionHeadEnabled : rightDirectionHeadDisabled, GUILayout.Height(40)))
                {
                    aIFlyingEnemy.currentDirection = DirectionsXAxisEnum.Right;
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Movement type", EditorStyles.boldLabel);
            aIFlyingEnemy.movementType = (AIFlyingEnemy.MovementType)EditorGUILayout.EnumPopup("Movement type: ", aIFlyingEnemy.movementType);
            aIFlyingEnemy.trackingType = (AIFlyingEnemy.FollowType)EditorGUILayout.EnumPopup("Tracking type: ", aIFlyingEnemy.trackingType);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            aIFlyingEnemy.velocityOfMovement = EditorGUILayout.FloatField("Movement velocity: ", aIFlyingEnemy.velocityOfMovement);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            aIFlyingEnemy.targetDetector = EditorGUILayout.ObjectField("Target detector: ", aIFlyingEnemy.targetDetector, typeof(TargetDetector), true) as TargetDetector;
        }

        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                aIFlyingEnemy.followInX = EditorGUILayout.Toggle("Follow in X: ", aIFlyingEnemy.followInX);
                if (aIFlyingEnemy.followInX)
                {
                    aIFlyingEnemy.xSeparation = EditorGUILayout.FloatField("Separation in X: ", aIFlyingEnemy.xSeparation);
                    aIFlyingEnemy.invertXSeparation = EditorGUILayout.Toggle("Invert separation in X: ", aIFlyingEnemy.invertXSeparation);
                    if (aIFlyingEnemy.invertXSeparation)
                        EditorGUILayout.LabelField("*Will be inverted the separation depending of the player (target) direction.", EditorStyles.miniLabel);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                aIFlyingEnemy.followInY = EditorGUILayout.Toggle("Follow in Y: ", aIFlyingEnemy.followInY);
                if (aIFlyingEnemy.followInY)
                    aIFlyingEnemy.ySeparation = EditorGUILayout.FloatField("Separation in Y: ", aIFlyingEnemy.ySeparation);
            }
        }

        EditorUtility.SetDirty(aIFlyingEnemy);
        Undo.RecordObject(aIFlyingEnemy, "Undo iAFlyingEnemy");

    }
}
