using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(IgnoreCollisions))]

public class IgnoreCollisionsEditor : Editor
{
    IgnoreCollisions ignoreCollisions;

    void OnEnable()
    {
        ignoreCollisions = (IgnoreCollisions)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.HelpBox("This script ignore the collisions or triggers between this game object and some tags/layers.", MessageType.None);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            ignoreCollisions.ignoreTriggersToo = EditorGUILayout.Toggle("Ignore triggers too: ", ignoreCollisions.ignoreTriggersToo);
            if(ignoreCollisions.ignoreTriggersToo)
                EditorGUILayout.HelpBox("The colliders marked as IsTrigger will be ignored along with the colliders.", MessageType.None);
        }

            using (new GUILayout.VerticalScope("box"))
        {
            ignoreCollisions.ignoreTag = EditorGUILayout.Toggle("Ignore tags: ", ignoreCollisions.ignoreTag);

            if (ignoreCollisions.ignoreTag)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    for (int i = 0; i < ignoreCollisions.tagsToIgnore.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ignoreCollisions.tagsToIgnore[i] = EditorGUILayout.TagField("Tag " + i + ": ", ignoreCollisions.tagsToIgnore[i]);

                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                        {
                            ignoreCollisions.tagsToIgnore.RemoveAt(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Add new tag"))
                    {
                        ignoreCollisions.tagsToIgnore.Add("");
                    }
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            ignoreCollisions.ignoreLayer = EditorGUILayout.Toggle("Ignore layers: ", ignoreCollisions.ignoreLayer);

            if (ignoreCollisions.ignoreLayer)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    for (int i = 0; i < ignoreCollisions.layersToIgnore.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        ignoreCollisions.layersToIgnore[i] = EditorGUILayout.LayerField("Layer " + i + ": ", ignoreCollisions.layersToIgnore[i]);

                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                        {
                            ignoreCollisions.layersToIgnore.RemoveAt(i);
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Add new layer"))
                    {
                        ignoreCollisions.layersToIgnore.Add(0);
                    }
                }
            }
        }

        EditorUtility.SetDirty(ignoreCollisions);
        Undo.RecordObject(ignoreCollisions, "Undo ignoreCollisions");
    }
}
