using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(PlayerLifeManager))]

public class PlayerLifeManagerEditor : Editor
{

    PlayerLifeManager playerLifeManager;

    void OnEnable()
    {
        playerLifeManager = (PlayerLifeManager)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Life options", EditorStyles.boldLabel);
            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Lives", EditorStyles.boldLabel);
                playerLifeManager.defaultTotalLives = EditorGUILayout.IntField("Default total lives: ", playerLifeManager.defaultTotalLives);
                playerLifeManager.countLive0 = EditorGUILayout.Toggle("Count live 0: ", playerLifeManager.countLive0);

                using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("What to do when the lives reaches 0", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 145;
                    playerLifeManager.killPlayerWhenLives0 = EditorGUILayout.Toggle("Kill player: ", playerLifeManager.killPlayerWhenLives0);
                    playerLifeManager.deleteAllLevelsClearedWhenLives0 = EditorGUILayout.Toggle("Delete all levels cleared: ", playerLifeManager.deleteAllLevelsClearedWhenLives0);
                    EditorGUIUtility.labelWidth = 245;
                    playerLifeManager.deleteAllLevelsClearedOfCurrentWorldWhenLives0 = EditorGUILayout.Toggle("Delete all levels cleared of current world: ", playerLifeManager.deleteAllLevelsClearedOfCurrentWorldWhenLives0);
                    playerLifeManager.deleteSavePointsOfCurrentLevelWhenLives0 = EditorGUILayout.Toggle("Delete save points of current level: ", playerLifeManager.deleteSavePointsOfCurrentLevelWhenLives0);
                }
            }
            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
                EditorGUIUtility.labelWidth = 110;
                playerLifeManager.defaultHealth = EditorGUILayout.FloatField("Default health: ", playerLifeManager.defaultHealth);
                playerLifeManager.countHealth0 = EditorGUILayout.Toggle("Count health 0: ", playerLifeManager.countHealth0);
                EditorGUIUtility.labelWidth = 185;
                playerLifeManager.fillLenghtOfLife = EditorGUILayout.Toggle("Fill health when the level start: ", playerLifeManager.fillLenghtOfLife);

                using (var verticalScope2 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("What to do when the health reaches 0", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 145;
                    playerLifeManager.killPlayerWhenHealth0 = EditorGUILayout.Toggle("Kill player: ", playerLifeManager.killPlayerWhenHealth0);
                    playerLifeManager.deleteAllLevelsClearedWhenHealth0 = EditorGUILayout.Toggle("Delete all levels cleared: ", playerLifeManager.deleteAllLevelsClearedWhenHealth0);
                    EditorGUIUtility.labelWidth = 245;
                    playerLifeManager.deleteAllLevelsClearedOfCurrentWorldWhenHealth0 = EditorGUILayout.Toggle("Delete all levels cleared of current world: ", playerLifeManager.deleteAllLevelsClearedOfCurrentWorldWhenHealth0);
                    playerLifeManager.deleteSavePointsOfCurrentLevelWhenHealth0 = EditorGUILayout.Toggle("Delete save points of current level: ", playerLifeManager.deleteSavePointsOfCurrentLevelWhenHealth0);
                }
            }

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;
                playerLifeManager.immunityTime = EditorGUILayout.FloatField("Immunity time: ", playerLifeManager.immunityTime);
                EditorGUILayout.LabelField("Excecuted later of life reduction.", EditorStyles.miniLabel);
            }

        }

        using (var verticalScope0 = new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("User interface", EditorStyles.boldLabel);

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;
                playerLifeManager.useHealthFilling = EditorGUILayout.Toggle("Use health filling: ", playerLifeManager.useHealthFilling);

                if(playerLifeManager.useHealthFilling)
                {
                    EditorGUIUtility.labelWidth = 125;
                    playerLifeManager.healthFillerImage = EditorGUILayout.ObjectField("Image (Type Filled): ", playerLifeManager.healthFillerImage, typeof(Image), true) as Image;

                    playerLifeManager.healthFillerImage = ButtonsOfInstantiateImageOfFill(playerLifeManager.healthFillerImage);

                    if (playerLifeManager.healthFillerImage != null && playerLifeManager.healthFillerImage.type != Image.Type.Filled)
                    {
                        EditorGUILayout.LabelField("*The image attached is not of type Filled.", EditorStyles.miniLabel);
                    }
                }
            }

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;
                playerLifeManager.useHealthCounter = EditorGUILayout.Toggle("Use health counter: ", playerLifeManager.useHealthCounter);

                if (playerLifeManager.useHealthCounter)
                {
                    EditorGUIUtility.labelWidth = 150;
                    playerLifeManager.healthCounterParent = EditorGUILayout.ObjectField("Health counter (Parent): ", playerLifeManager.healthCounterParent, typeof(RectTransform), true) as RectTransform;
                    playerLifeManager.healthIconForLifeCounter = EditorGUILayout.ObjectField("Health icon: ", playerLifeManager.healthIconForLifeCounter, typeof(GameObject), false) as GameObject;
                }
            }

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;
                playerLifeManager.showTotalLives = EditorGUILayout.Toggle("Show total lives: ", playerLifeManager.showTotalLives);

                if (playerLifeManager.showTotalLives)
                {
                    EditorGUIUtility.labelWidth = 150;
                    playerLifeManager.totalLivesText = EditorGUILayout.ObjectField("Total lives (Text): ", playerLifeManager.totalLivesText, typeof(Text), true) as Text;
                    playerLifeManager.formatTotalLives = EditorGUILayout.TextField("Format: ", playerLifeManager.formatTotalLives);

                    if(playerLifeManager.formatTotalLives != null && !playerLifeManager.formatTotalLives.Contains("0"))
                    {
                        EditorGUILayout.LabelField("*The format should have at least one 0.", EditorStyles.miniLabel);
                    }

                }
            }

            using (var verticalScope1 = new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 125;
                playerLifeManager.usePlayerIcon = EditorGUILayout.Toggle("Show player icon: ", playerLifeManager.usePlayerIcon);

                if (playerLifeManager.usePlayerIcon)
                {
                    EditorGUIUtility.labelWidth = 110;
                    playerLifeManager.playerIcon = EditorGUILayout.ObjectField("Player icon: ", playerLifeManager.playerIcon, typeof(Sprite), false, GUILayout.Width(170), GUILayout.Height(60)) as Sprite;
                    playerLifeManager.uIOfPlayerIcon = EditorGUILayout.ObjectField("UI of player icon: ", playerLifeManager.uIOfPlayerIcon, typeof(Image), true) as Image;
                }
            }
        }

        EditorUtility.SetDirty(playerLifeManager);
        Undo.RecordObject(playerLifeManager, "Undo player life");
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

}
