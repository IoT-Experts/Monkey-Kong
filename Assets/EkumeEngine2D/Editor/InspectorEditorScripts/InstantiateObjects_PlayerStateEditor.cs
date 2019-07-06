using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;

[CustomEditor(typeof(InstantiateObjects_PlayerState))]

public class InstantiateObjects_PlayerStateEditor : Editor
{

    InstantiateObjects_PlayerState instantiateObjects;

    void OnEnable()
    {
        instantiateObjects = (InstantiateObjects_PlayerState)target;
    }

    public override void OnInspectorGUI()
    {
        using (var verticalScope = new GUILayout.VerticalScope("box"))
        {
            for(int i = 0; i < instantiateObjects.stateRequired.Count; i++)
            {
                using (var verticalScope0 = new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 142;
                    instantiateObjects.stateRequired[i] = (PlayerStatesEnum)EditorGUILayout.EnumPopup("State required: ", instantiateObjects.stateRequired[i]);
                    instantiateObjects.objectToInstantiate[i] = EditorGUILayout.ObjectField("Object to instantiate: ", instantiateObjects.objectToInstantiate[i], typeof(GameObject), true) as GameObject;
                    instantiateObjects.positionToInstantiate[i] = EditorGUILayout.ObjectField("Position to instantiate: ", instantiateObjects.positionToInstantiate[i], typeof(Transform), true) as Transform;
                    EditorGUIUtility.labelWidth = 170;
                    instantiateObjects.instantiateWhileStateTrue[i] = EditorGUILayout.Toggle("Keep while state is true: ", instantiateObjects.instantiateWhileStateTrue[i]);

                    if(instantiateObjects.instantiateWhileStateTrue[i])
                    {
                        instantiateObjects.instantiateByFixedTime[i] = false;
                    }

                    instantiateObjects.instantiateByFixedTime[i] = EditorGUILayout.Toggle("Keep by fixed time: ", instantiateObjects.instantiateByFixedTime[i]);

                    if (instantiateObjects.instantiateByFixedTime[i])
                    {
                        instantiateObjects.instantiateWhileStateTrue[i] = false;
                    }

                    if (!instantiateObjects.instantiateByFixedTime[i] && !instantiateObjects.instantiateWhileStateTrue[i])
                    {
                        instantiateObjects.instantiateWhileStateTrue[i] = true;
                    }

                    if (instantiateObjects.instantiateByFixedTime[i])
                        instantiateObjects.timeToKeepInstantiated[i] = EditorGUILayout.FloatField("Time to keep instantiated: ", instantiateObjects.timeToKeepInstantiated[i]);

                    instantiateObjects.destroyWhenStateFalse[i] = EditorGUILayout.Toggle("Destroy when state is false: ", instantiateObjects.destroyWhenStateFalse[i]);

                    if (GUILayout.Button("Delete"))
                    {
                        instantiateObjects.stateRequired.RemoveAt(i);
                        instantiateObjects.objectToInstantiate.RemoveAt(i);
                        instantiateObjects.instantiateWhileStateTrue.RemoveAt(i);
                        instantiateObjects.destroyWhenStateFalse.RemoveAt(i);
                        instantiateObjects.instantiateByFixedTime.RemoveAt(i);
                        instantiateObjects.timeToKeepInstantiated.RemoveAt(i);
                        instantiateObjects.positionToInstantiate.RemoveAt(i);
                    }
                }
            }

            if(GUILayout.Button("Add new"))
            {
                instantiateObjects.stateRequired.Add(PlayerStatesEnum.UsingPowerToFly);
                instantiateObjects.objectToInstantiate.Add(null);
                instantiateObjects.instantiateWhileStateTrue.Add(true);
                instantiateObjects.destroyWhenStateFalse.Add(true);
                instantiateObjects.instantiateByFixedTime.Add(false);
                instantiateObjects.timeToKeepInstantiated.Add(0);
                instantiateObjects.positionToInstantiate.Add(null);
            }
        }

        EditorUtility.SetDirty(instantiateObjects);
        Undo.RecordObject(instantiateObjects, "Undo instantiateObjects");
    }
}
