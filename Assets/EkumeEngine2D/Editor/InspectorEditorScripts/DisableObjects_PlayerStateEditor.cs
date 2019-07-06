using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;

[CustomEditor(typeof(DisableObjects_PlayerState))]

public class DisableObjects_PlayerStateEditor : Editor
{

    DisableObjects_PlayerState disableObjects;

    void OnEnable()
    {
        disableObjects = (DisableObjects_PlayerState)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            for(int i = 0; i < disableObjects.stateActivator.Count; i++)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 165;
                    disableObjects.stateActivator[i] = (PlayerStatesEnum)EditorGUILayout.EnumPopup("State to disable object: ", disableObjects.stateActivator[i]);
                    disableObjects.objectToDisable[i] = EditorGUILayout.ObjectField("Object to disable: ", disableObjects.objectToDisable[i], typeof(GameObject), true) as GameObject;

                    disableObjects.disableWhileStateTrue[i] = EditorGUILayout.Toggle("Disable while state is true: ", disableObjects.disableWhileStateTrue[i]);

                    if(disableObjects.disableWhileStateTrue[i])
                    {
                        disableObjects.disableByFixedTime[i] = false;
                    }

                    disableObjects.disableByFixedTime[i] = EditorGUILayout.Toggle("Disable by fixed time: ", disableObjects.disableByFixedTime[i]);

                    if (disableObjects.disableByFixedTime[i])
                    {
                        disableObjects.disableWhileStateTrue[i] = false;
                    }

                    if (!disableObjects.disableByFixedTime[i] && !disableObjects.disableWhileStateTrue[i])
                    {
                        disableObjects.disableWhileStateTrue[i] = true;
                    }

                    if (disableObjects.disableByFixedTime[i])
                        disableObjects.timeToKeepDisabled[i] = EditorGUILayout.FloatField("Time to keep disabled: ", disableObjects.timeToKeepDisabled[i]);

                    disableObjects.enableWhenStateFalse[i] = EditorGUILayout.Toggle("Enable when state is false: ", disableObjects.enableWhenStateFalse[i]);

                    if (GUILayout.Button("Delete"))
                    {
                        disableObjects.stateActivator.RemoveAt(i);
                        disableObjects.objectToDisable.RemoveAt(i);
                        disableObjects.disableWhileStateTrue.RemoveAt(i);
                        disableObjects.enableWhenStateFalse.RemoveAt(i);
                        disableObjects.disableByFixedTime.RemoveAt(i);
                        disableObjects.timeToKeepDisabled.RemoveAt(i);
                    }
                }
            }

            if(GUILayout.Button("Add new"))
            {
                disableObjects.stateActivator.Add(PlayerStatesEnum.UsingPowerToFly);
                disableObjects.objectToDisable.Add(null);
                disableObjects.disableWhileStateTrue.Add(true);
                disableObjects.enableWhenStateFalse.Add(true);
                disableObjects.disableByFixedTime.Add(false);
                disableObjects.timeToKeepDisabled.Add(0);
            }
        }

        EditorUtility.SetDirty(disableObjects);
        Undo.RecordObject(disableObjects, "Undo disableObjects");
    }
}
