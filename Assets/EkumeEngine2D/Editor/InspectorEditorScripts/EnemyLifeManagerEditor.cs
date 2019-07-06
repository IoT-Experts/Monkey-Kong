using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(EnemyLifeManager))]

public class EnemyLifeManagerEditor : Editor
{
    EnemyLifeManager enemyLifeManager;

    void OnEnable()
    {
        enemyLifeManager = (EnemyLifeManager)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            enemyLifeManager.health = EditorGUILayout.FloatField("Health: ", enemyLifeManager.health);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 150;
            enemyLifeManager.showUIOfHealth = EditorGUILayout.Toggle("Show UI of health: ", enemyLifeManager.showUIOfHealth);
            if (enemyLifeManager.showUIOfHealth)
            {
                EditorGUIUtility.labelWidth = 180;
                using (new GUILayout.VerticalScope("box"))
                {
                    enemyLifeManager.showUIOfHealthSinceStart = EditorGUILayout.Toggle("Show UI of health since start: ", enemyLifeManager.showUIOfHealthSinceStart);
                }
                using (new GUILayout.VerticalScope("box"))
                {
                    enemyLifeManager.uIHealthContainer = EditorGUILayout.ObjectField("UI of health container: ", enemyLifeManager.uIHealthContainer, typeof(GameObject), true) as GameObject;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        enemyLifeManager.useHealthFilling = EditorGUILayout.Toggle("Use health filling: ", enemyLifeManager.useHealthFilling);
                        if (enemyLifeManager.useHealthFilling)
                            enemyLifeManager.healthFiller = EditorGUILayout.ObjectField("Image (Type Filled): ", enemyLifeManager.healthFiller, typeof(Image), true) as Image;
                    }

                    using (new GUILayout.VerticalScope("box"))
                    {
                        enemyLifeManager.showHealthPoints = EditorGUILayout.Toggle("Show health points: ", enemyLifeManager.showHealthPoints);
                        if (enemyLifeManager.showHealthPoints)
                        {
                            enemyLifeManager.healthPointsText = EditorGUILayout.ObjectField("Health points (Text): ", enemyLifeManager.healthPointsText, typeof(Text), true) as Text;
                            enemyLifeManager.showCurrentDivTotal = EditorGUILayout.Toggle("Show current div total: ", enemyLifeManager.showCurrentDivTotal);
                        }
                    }
                }

                if (enemyLifeManager.uIHealthContainer == null && enemyLifeManager.healthPointsText == null && enemyLifeManager.healthFiller == null)
                {
                    if (GUILayout.Button("Create example of UI for health"))
                    {
                        enemyLifeManager.useHealthFilling = true;

                        GameObject instantiatedObj = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/EkumeEngine2D/Prefabs/EditorPrefabs/UIOfLifeOfMonster.prefab", typeof(GameObject)), enemyLifeManager.transform.position, Quaternion.identity) as GameObject;
                        instantiatedObj.transform.SetParent(enemyLifeManager.transform);
                        instantiatedObj.transform.position = new Vector3(instantiatedObj.transform.position.x, instantiatedObj.transform.position.y + 2, instantiatedObj.transform.position.z);

                        instantiatedObj.name = "UIOfLife";
                        enemyLifeManager.uIHealthContainer = instantiatedObj;
                        enemyLifeManager.healthFiller = instantiatedObj.transform.FindChild("LifeFillerBG").transform.FindChild("LifeFiller").GetComponent<Image>();
                    }
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 190;
            enemyLifeManager.timeToDestroyObjWhenDie = EditorGUILayout.FloatField("Delay to destroy it when dies: ", enemyLifeManager.timeToDestroyObjWhenDie);
        }

        EditorUtility.SetDirty(enemyLifeManager);
        Undo.RecordObject(enemyLifeManager, "Undo enemyLifeManager");

    }
}