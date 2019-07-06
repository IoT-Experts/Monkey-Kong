using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SwitchOfComponent))]

public class SwitchOfComponentEditor : Editor
{
    SwitchOfComponent switchOfComponent;

    void OnEnable()
    {
        switchOfComponent = (SwitchOfComponent)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                switchOfComponent.actionToDo = (SwitchOfComponent.ActionsForComponentEnum)EditorGUILayout.EnumPopup("Action: ", switchOfComponent.actionToDo);
                switchOfComponent.activatorOfAction = (SwitchOfComponent.ActivatorOfAction)EditorGUILayout.EnumPopup("Action activator: ", switchOfComponent.activatorOfAction);
            }

            if (switchOfComponent.activatorOfAction != SwitchOfComponent.ActivatorOfAction.WhenTheObjectStart)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("What tags can activates the action?");
                    switchOfComponent.anyTag = EditorGUILayout.Toggle("Any tag: ", switchOfComponent.anyTag);
                    if (!switchOfComponent.anyTag)
                    {
                        if (switchOfComponent.tagsThatActivate.Count > 0)
                        {
                            using (new GUILayout.VerticalScope("box"))
                            {
                                for (int i = 0; i < switchOfComponent.tagsThatActivate.Count; i++)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    switchOfComponent.tagsThatActivate[i] = EditorGUILayout.TagField("Tag " + i + ": ", switchOfComponent.tagsThatActivate[i]);
                                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                                    {
                                        switchOfComponent.tagsThatActivate.RemoveAt(i);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                        }

                        if (GUILayout.Button("Add new tag", EditorStyles.miniButton))
                        {
                            switchOfComponent.tagsThatActivate.Add("");
                        }
                    }
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                switchOfComponent.typeOfComponent = (SwitchOfComponent.TypeOfComponentEnum)EditorGUILayout.EnumPopup("Component: ", switchOfComponent.typeOfComponent);

                switch (switchOfComponent.typeOfComponent)
                {
                    case SwitchOfComponent.TypeOfComponentEnum.Script:
                        switchOfComponent.behaviourComponent = EditorGUILayout.ObjectField("Script: ", switchOfComponent.behaviourComponent, typeof(MonoBehaviour), true) as MonoBehaviour;
                        break;
                    case SwitchOfComponent.TypeOfComponentEnum.Collider2D:
                        switchOfComponent.behaviourComponent = EditorGUILayout.ObjectField("Collider2D: ", switchOfComponent.behaviourComponent, typeof(Collider2D), true) as Collider2D;
                        break;
                    case SwitchOfComponent.TypeOfComponentEnum.otherComponentType:
                        switchOfComponent.component = EditorGUILayout.ObjectField("Component to destroy: ", switchOfComponent.component, typeof(Component), true) as Component;
                        if (switchOfComponent.actionToDo != SwitchOfComponent.ActionsForComponentEnum.Destroy)
                        {
                            EditorGUILayout.LabelField("*This component can be only destroyed.", EditorStyles.miniLabel);
                            EditorGUILayout.LabelField("*Enable and disable actions will nor works.", EditorStyles.miniLabel);
                        }
                        break;
                    case SwitchOfComponent.TypeOfComponentEnum.SpriteRenderer:
                        switchOfComponent.spriteRendererComp = EditorGUILayout.ObjectField("SpriteRenderer: ", switchOfComponent.spriteRendererComp, typeof(SpriteRenderer), true) as SpriteRenderer;
                        break;
                    case SwitchOfComponent.TypeOfComponentEnum.OtherGameObject:
                        switchOfComponent.gameObjectComp = EditorGUILayout.ObjectField("GameObject: ", switchOfComponent.gameObjectComp, typeof(GameObject), true) as GameObject;
                        break;
                    case SwitchOfComponent.TypeOfComponentEnum.GameObjectThatCollides:
                        if (switchOfComponent.activatorOfAction == SwitchOfComponent.ActivatorOfAction.WhenTheObjectStart)
                        {
                            EditorGUILayout.LabelField("Do you want make some action when some object collides with this?", EditorStyles.miniLabel);
                            EditorGUILayout.LabelField("*You are activating this action when the object start..", EditorStyles.miniLabel);
                            EditorGUILayout.LabelField("*You can't detect collisions when the object start...", EditorStyles.miniLabel);
                        }
                        break;
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                switchOfComponent.secondsToActivateAction = EditorGUILayout.FloatField("Delay to activate: ", switchOfComponent.secondsToActivateAction);
            }

        }

        EditorUtility.SetDirty(switchOfComponent);
        Undo.RecordObject(switchOfComponent, "Undo switchOfComponent");
    }
}
