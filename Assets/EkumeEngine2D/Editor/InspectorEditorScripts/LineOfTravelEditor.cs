using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;

[CustomEditor(typeof(LineOfTravel))]
public class LineOfTravelEditor : Editor
{

    LineOfTravel lineOfTravel;

    void OnEnable()
    {
        lineOfTravel = (LineOfTravel)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                lineOfTravel.orientationOfLine = (LineOfTravel.Orientation)EditorGUILayout.EnumPopup("Orientation of the line ", lineOfTravel.orientationOfLine);
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Object in world", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Object in line", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < lineOfTravel.objectsToShowInLine.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    lineOfTravel.objectsToShowInLine[i] = EditorGUILayout.ObjectField(lineOfTravel.objectsToShowInLine[i], typeof(Transform), true) as Transform;
                    lineOfTravel.travelersUI[i] = EditorGUILayout.ObjectField(lineOfTravel.travelersUI[i], typeof(RectTransform), true) as RectTransform;
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(25)))
                    {
                        lineOfTravel.objectsToShowInLine.RemoveAt(i);
                        lineOfTravel.travelersUI.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal();
                lineOfTravel.findPlayer = EditorGUILayout.Toggle("Find Player Automatically", lineOfTravel.findPlayer);

                if(lineOfTravel.findPlayer)
                    lineOfTravel.playerObjectInLine = EditorGUILayout.ObjectField(lineOfTravel.playerObjectInLine, typeof(RectTransform), true) as RectTransform;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                lineOfTravel.findGoal = EditorGUILayout.Toggle("Find Goal Automatically", lineOfTravel.findGoal);

                if (lineOfTravel.findGoal)
                {
                    lineOfTravel.goalObjectInLine = EditorGUILayout.ObjectField(lineOfTravel.goalObjectInLine, typeof(RectTransform), true) as RectTransform;
                }
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                lineOfTravel.findSavePoints = EditorGUILayout.Toggle("Find Save Points Automatically", lineOfTravel.findSavePoints);

                if (lineOfTravel.findSavePoints)
                {
                    lineOfTravel.savePointObjectInLine = EditorGUILayout.ObjectField(lineOfTravel.savePointObjectInLine, typeof(RectTransform), true) as RectTransform;
                }

                EditorGUILayout.EndHorizontal();

                if (lineOfTravel.findSavePoints)
                {
                    EditorGUILayout.HelpBox("The icons of the save point will be duplicated depending of the quantity of  save points in the scene.", MessageType.None);
                }

                if (lineOfTravel.findGoal)
                {
                    if (FindObjectOfType<WinLevel>() == null)
                        EditorGUILayout.HelpBox("It is necessary add the component WinLevel to some object in the game to use this option.", MessageType.Error);
                }

                if (GUILayout.Button("Add new object", EditorStyles.miniButton))
                {
                    lineOfTravel.objectsToShowInLine.Add(null);
                    lineOfTravel.travelersUI.Add(null);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                if (lineOfTravel.findPlayer && lineOfTravel.findGoal)
                {
                    EditorGUIUtility.labelWidth = 300;
                    lineOfTravel.autoAsignStartAndEnd = EditorGUILayout.Toggle("Asing Player like Start, and Goal like End of scene", lineOfTravel.autoAsignStartAndEnd);
                }

                EditorGUIUtility.labelWidth = 150;
                if (!lineOfTravel.autoAsignStartAndEnd || (!lineOfTravel.findPlayer || !lineOfTravel.findGoal))
                {
                    lineOfTravel.startOfScene = EditorGUILayout.ObjectField("Start of the scene", lineOfTravel.startOfScene, typeof(Transform), true) as Transform;
                    lineOfTravel.endOfScene = EditorGUILayout.ObjectField("End of the scene", lineOfTravel.endOfScene, typeof(Transform), true) as Transform;

                    if (lineOfTravel.startOfScene == null && lineOfTravel.endOfScene == null)
                        EditorGUILayout.HelpBox("Create two new objects in the scene to indicate the start and the end of the scene.", MessageType.Info);
                }
            }

            using (new GUILayout.VerticalScope("box"))
            {
                lineOfTravel.aPointUI = EditorGUILayout.ObjectField("Start of the line in UI", lineOfTravel.aPointUI, typeof(RectTransform), true) as RectTransform;
                lineOfTravel.bPointUI = EditorGUILayout.ObjectField("End of the line in UI", lineOfTravel.bPointUI, typeof(RectTransform), true) as RectTransform;
                if (lineOfTravel.aPointUI == null && lineOfTravel.bPointUI == null)
                    EditorGUILayout.HelpBox("Create two new objects and put like child of the line in the UI to indicate the start and the end of the line.", MessageType.Info);

            }

            using (new GUILayout.VerticalScope("box"))
            {
                lineOfTravel.delayToRefreshPositions = EditorGUILayout.FloatField("Delay to refresh positions", lineOfTravel.delayToRefreshPositions);
            }
        }

        EditorUtility.SetDirty(lineOfTravel);
        Undo.RecordObject(lineOfTravel, "Undo lineOfTravel");
    }
}
