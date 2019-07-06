using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;

[CustomEditor(typeof(ActivateObjects_PlayerState))]

public class ActivateObjects_PlayerStateEditor : Editor
{

    ActivateObjects_PlayerState activateObjects;

    void OnEnable()
    {
        activateObjects = (ActivateObjects_PlayerState)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            for(int i = 0; i < activateObjects.stateActivator.Count; i++)
            {
                using (var verticalScope0 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 165;
                    activateObjects.stateActivator[i] = (PlayerStatesEnum)EditorGUILayout.EnumPopup("State activator: ", activateObjects.stateActivator[i]);
                    activateObjects.objectToActivates[i] = EditorGUILayout.ObjectField("Object to activate: ", activateObjects.objectToActivates[i], typeof(GameObject), true) as GameObject;

                    activateObjects.activateWhileStateTrue[i] = EditorGUILayout.Toggle("Activate when state is true: ", activateObjects.activateWhileStateTrue[i]);

                    if(activateObjects.activateWhileStateTrue[i])
                    {
                        activateObjects.activateByFixedTime[i] = false;
                    }

                    activateObjects.activateByFixedTime[i] = EditorGUILayout.Toggle("Activate by fixed time: ", activateObjects.activateByFixedTime[i]);

                    if (activateObjects.activateByFixedTime[i])
                    {
                        activateObjects.activateWhileStateTrue[i] = false;
                    }

                    if (!activateObjects.activateByFixedTime[i] && !activateObjects.activateWhileStateTrue[i])
                    {
                        activateObjects.activateWhileStateTrue[i] = true;
                    }

                    if (activateObjects.activateByFixedTime[i])
                        activateObjects.timeToKeepEnable[i] = EditorGUILayout.FloatField("Time to keep enable: ", activateObjects.timeToKeepEnable[i]);

                    activateObjects.disableWhenStateFalse[i] = EditorGUILayout.Toggle("Disable when state is false: ", activateObjects.disableWhenStateFalse[i]);

                    if (GUILayout.Button("Delete"))
                    {
                        activateObjects.stateActivator.RemoveAt(i);
                        activateObjects.objectToActivates.RemoveAt(i);
                        activateObjects.activateWhileStateTrue.RemoveAt(i);
                        activateObjects.disableWhenStateFalse.RemoveAt(i);
                        activateObjects.activateByFixedTime.RemoveAt(i);
                        activateObjects.timeToKeepEnable.RemoveAt(i);
                    }
                }
            }

            if(GUILayout.Button("Add new"))
            {
                activateObjects.stateActivator.Add(PlayerStatesEnum.UsingPowerToFly);
                activateObjects.objectToActivates.Add(null);
                activateObjects.activateWhileStateTrue.Add(true);
                activateObjects.disableWhenStateFalse.Add(true);
                activateObjects.activateByFixedTime.Add(false);
                activateObjects.timeToKeepEnable.Add(0);
            }
        }

        EditorUtility.SetDirty(activateObjects);
        Undo.RecordObject(activateObjects, "Undo activateObjects");
    }
}
