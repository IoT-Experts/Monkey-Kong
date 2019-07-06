using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MountLifeManager))]

public class MountLifeManagerEditor : Editor
{

    MountLifeManager mountLifeManager;

    void OnEnable()
    {
        mountLifeManager = (MountLifeManager)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 152;
            mountLifeManager.rideTheMount = EditorGUILayout.ObjectField("Object to ride the player: ", mountLifeManager.rideTheMount, typeof(RideTheMount), true) as RideTheMount;
            if (mountLifeManager.rideTheMount == null)
            {
                if (GUILayout.Button("Find the object with script \"RideTheMount\"", EditorStyles.miniButton))
                {
                    foreach (Transform child in mountLifeManager.transform)
                    {
                        if (child.GetComponent<RideTheMount>() != null)
                        {
                            mountLifeManager.rideTheMount = child.GetComponent<RideTheMount>();
                            break;
                        }
                    }

                    if (mountLifeManager.rideTheMount == null)
                    {
                        Debug.LogWarning("The mount does not have any object in childs with the script \"RideTheMount\", you should create an object like child in the mount with the corresponding script.");
                    }
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 115;
            EditorGUILayout.LabelField("Life options", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope("box"))
            {
                mountLifeManager.health = EditorGUILayout.FloatField("Health: ", mountLifeManager.health);
                mountLifeManager.countHealth0 = EditorGUILayout.Toggle("Count health 0: ", mountLifeManager.countHealth0);
            }
            using (new GUILayout.VerticalScope("box"))
            {
                mountLifeManager.immunityTime = EditorGUILayout.FloatField("Immunity Time: ", mountLifeManager.immunityTime);
            }
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 170;
                mountLifeManager.timeToDestroyWhenDie = EditorGUILayout.FloatField("Delay to destroy when dies: ", mountLifeManager.timeToDestroyWhenDie);
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Mount UI", EditorStyles.boldLabel);

            EditorGUIUtility.labelWidth = 115;
            mountLifeManager.useUIForMount = EditorGUILayout.Toggle("Use UI for mount: ", mountLifeManager.useUIForMount);

            if (mountLifeManager.useUIForMount)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    mountLifeManager.mountUI = EditorGUILayout.ObjectField("Mount UI (Parent): ", mountLifeManager.mountUI, typeof(GameObject), true) as GameObject;
                    if (mountLifeManager.mountUI != null && mountLifeManager.mountUI.activeInHierarchy)
                    {
                        EditorGUILayout.LabelField("*The GameObject should be disabled.");
                    }
                }

                using (new GUILayout.VerticalScope("box"))
                {
                    mountLifeManager.useHealthFilling = EditorGUILayout.Toggle("Use health filling: ", mountLifeManager.useHealthFilling);
                    if (mountLifeManager.useHealthFilling)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            mountLifeManager.healthFiller = EditorGUILayout.ObjectField("Image (Type Filled): ", mountLifeManager.healthFiller, typeof(Image), true) as Image;

                            if (mountLifeManager.healthFiller != null && mountLifeManager.healthFiller.type != Image.Type.Filled)
                            {
                                EditorGUILayout.LabelField("*The image attached is not of type Filled.", EditorStyles.miniLabel);
                            }

                            if (mountLifeManager.mountUI != null && mountLifeManager.mountUI.activeInHierarchy)
                            {
                                EditorGUILayout.LabelField("*The GameObject should be disabled.");
                            }
                        }
                    }
                }

                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 125;
                    mountLifeManager.showIconForMount = EditorGUILayout.Toggle("Show icon of mount: ", mountLifeManager.showIconForMount);
                    if (mountLifeManager.showIconForMount)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            EditorGUIUtility.labelWidth = 115;
                            mountLifeManager.mountIcon = EditorGUILayout.ObjectField("Mount icon: ", mountLifeManager.mountIcon, typeof(Sprite), false, GUILayout.Width(175), GUILayout.Height(60)) as Sprite;
                            mountLifeManager.uIOfIcon = EditorGUILayout.ObjectField("UI of the icon: ", mountLifeManager.uIOfIcon, typeof(Image), true) as Image;
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(mountLifeManager);
        Undo.RecordObject(mountLifeManager, "Undo mountLifeManager");
    }
}
