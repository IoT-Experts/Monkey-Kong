using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PathFollower))]
[CanEditMultipleObjects]
public class PathFollowerEditor : Editor
{

    PathFollower pathFollower;

    int selectedMenu = 0;
    string[] toolbarOptions = new string[] { "Paths", "Gizmos" };

    void OnEnable()
    {
        pathFollower = (PathFollower)target;
    }

    void OnSceneGUI()
    {
        for (int i = 0; i < pathFollower.points.Count; i++)
        {
            if (pathFollower.points[i] != null && pathFollower.points[i].wayPointPosition != null)
            {
                Handles.color = pathFollower.controlPointsColor;

                EditorGUI.BeginChangeCheck();

                Vector3 pos = Handles.FreeMoveHandle(pathFollower.points[i].wayPointPosition.position, pathFollower.points[i].wayPointPosition.rotation, pathFollower.controlPointsSize, new Vector3(0, 0, 0), pathFollower.controlPointsCap);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathFollower.points[i].wayPointPosition, "Moved point " + i);
                    pathFollower.points[i].wayPointPosition.position = pos;
                }
            }
        }

        for (int i = 0; i < pathFollower.points.Count; i++)
        {
            if (pathFollower.usingCurves)
            {
                if (pathFollower.points[i] != null && pathFollower.points[i].wayPointPosition != null)
                {
                    if (pathFollower.loopMovement || i != pathFollower.points.Count - 1)
                    {

                        Handles.color = pathFollower.movableControlPointColor;

                        EditorGUI.BeginChangeCheck();

                        Vector3 pos = Handles.FreeMoveHandle(pathFollower.points[i].bezierPoint1.position, new Quaternion(0, 0, 0, 0), pathFollower.movableControlPointSize, new Vector3(0, 0, 0), pathFollower.movableControlPointCap);

                        Handles.color = pathFollower.colorOfLineOfControlPoint;
                        Handles.DrawLine(pathFollower.points[i].bezierPoint1.position, pathFollower.points[i].wayPointPosition.position);

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(pathFollower.points[i].bezierPoint1, "Moved control point A" + i);
                            pathFollower.points[i].bezierPoint1.position = pos;
                        }

                        Handles.color = pathFollower.movableControlPointColor;

                        EditorGUI.BeginChangeCheck();

                        Vector3 pos2 = Handles.FreeMoveHandle(pathFollower.points[i].bezierPoint2.position, new Quaternion(0, 0, 0, 0), pathFollower.movableControlPointSize, new Vector3(0, 0, 0), pathFollower.movableControlPointCap);

                        Handles.color = pathFollower.colorOfLineOfControlPoint;

                        Handles.DrawLine(pathFollower.points[i].bezierPoint2.position, pathFollower.points[i].wayPointPosition.position);

                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(pathFollower.points[i].bezierPoint2, "Moved control point B" + i);
                            pathFollower.points[i].bezierPoint2.position = pos2;
                        }

                        if (pathFollower.showMovementArrows)
                        {
                            Vector3 movement = new Vector3();
                            Vector3 movement2 = new Vector3();

                            EditorGUI.BeginChangeCheck();

                            Handles.color = Handles.xAxisColor;
                            movement = Handles.Slider(pathFollower.points[i].bezierPoint1.position, Vector3.right, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);
                            movement2 = Handles.Slider(pathFollower.points[i].bezierPoint2.position, Vector3.right, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(pathFollower.points[i].bezierPoint1, "Moved control point X" + i);
                                Undo.RecordObject(pathFollower.points[i].bezierPoint2, "Moved control point2 X" + i);
                                pathFollower.points[i].bezierPoint1.position = movement;
                                pathFollower.points[i].bezierPoint2.position = movement2;
                            }

                            EditorGUI.BeginChangeCheck();
                            Handles.color = Handles.yAxisColor;
                            movement = Handles.Slider(pathFollower.points[i].bezierPoint1.position, Vector3.up, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);
                            movement2 = Handles.Slider(pathFollower.points[i].bezierPoint2.position, Vector3.up, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(pathFollower.points[i].bezierPoint1, "Moved control point Y" + i);
                                Undo.RecordObject(pathFollower.points[i].bezierPoint2, "Moved control point2 Y" + i);
                                pathFollower.points[i].bezierPoint1.position = movement;
                                pathFollower.points[i].bezierPoint2.position = movement2;
                            }

                            EditorGUI.BeginChangeCheck();
                            Handles.color = Handles.zAxisColor;
                            movement = Handles.Slider(pathFollower.points[i].bezierPoint1.position, Vector3.forward, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);
                            movement2 = Handles.Slider(pathFollower.points[i].bezierPoint2.position, Vector3.forward, pathFollower.arrowsSize, new Handles.CapFunction(Handles.ArrowHandleCap), 0);

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(pathFollower.points[i].bezierPoint1, "Moved control point Z" + i);
                                Undo.RecordObject(pathFollower.points[i].bezierPoint2, "Moved control point2 Z" + i);
                                pathFollower.points[i].bezierPoint1.position = movement;
                                pathFollower.points[i].bezierPoint2.position = movement2;
                            }
                        }

                    }
                }
            }

            if (pathFollower.showRotations && pathFollower.rotationOrLookAt == PathFollower.RotationAndLookAt.CopyRotationOfEachWayPoint)
            {
                EditorGUI.BeginChangeCheck();
                Quaternion rot = Handles.RotationHandle(pathFollower.points[i].wayPointPosition.rotation, pathFollower.points[i].wayPointPosition.position);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(pathFollower.points[i].wayPointPosition, "Rotated RotateAt Point");
                    pathFollower.points[i].wayPointPosition.rotation = rot;
                }
            }

        }
    }

    public override void OnInspectorGUI()
    {

        selectedMenu = GUILayout.Toolbar(selectedMenu, toolbarOptions);

        switch (selectedMenu)
        {
            case 0:
                PathOptions();
                break;
            case 1:
                GizmosOptions();
                break;
        }

        EditorUtility.SetDirty(pathFollower);
        Undo.RecordObject(pathFollower, "Undo pathFollower");
    }

    void GizmosOptions()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.LabelField("Way", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                pathFollower.wayColor = EditorGUILayout.ColorField("Color ", pathFollower.wayColor);
                pathFollower.waySize = EditorGUILayout.FloatField("Size ", pathFollower.waySize);
                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Way points", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 45;
                pathFollower.wayPointsColor = EditorGUILayout.ColorField("Color ", pathFollower.wayPointsColor);
                EditorGUIUtility.labelWidth = 38;
                pathFollower.wayPointsSize = EditorGUILayout.FloatField("Size ", pathFollower.wayPointsSize, GUILayout.Width(80));
                EditorGUIUtility.labelWidth = 47;
                pathFollower.wayPointsCapEnum = (PathFollower.HandleCapFunctions)EditorGUILayout.EnumPopup("Shape ", pathFollower.wayPointsCapEnum);
                pathFollower.wayPointsCap = HandleCapSelection(pathFollower.wayPointsCapEnum);
                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Control points", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 45;
                pathFollower.controlPointsColor = EditorGUILayout.ColorField("Color ", pathFollower.controlPointsColor);
                EditorGUIUtility.labelWidth = 38;
                pathFollower.controlPointsSize = EditorGUILayout.FloatField("Size ", pathFollower.controlPointsSize, GUILayout.Width(80));
                EditorGUIUtility.labelWidth = 47;
                pathFollower.controlPointsCapEnum = (PathFollower.HandleCapFunctions)EditorGUILayout.EnumPopup("Shape ", pathFollower.controlPointsCapEnum);
                pathFollower.controlPointsCap = HandleCapSelection(pathFollower.controlPointsCapEnum);
                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Control points of curves", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 45;
                pathFollower.movableControlPointColor = EditorGUILayout.ColorField("Color ", pathFollower.movableControlPointColor);
                EditorGUIUtility.labelWidth = 38;
                pathFollower.movableControlPointSize = EditorGUILayout.FloatField("Size ", pathFollower.movableControlPointSize, GUILayout.Width(80));
                EditorGUIUtility.labelWidth = 47;
                pathFollower.movableControlPointCapEnum = (PathFollower.HandleCapFunctions)EditorGUILayout.EnumPopup("Shape ", pathFollower.movableControlPointCapEnum);
                pathFollower.movableControlPointCap = HandleCapSelection(pathFollower.movableControlPointCapEnum);
                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("Lines of control points of curves", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 45;
                pathFollower.colorOfLineOfControlPoint = EditorGUILayout.ColorField("Color ", pathFollower.colorOfLineOfControlPoint);
                EditorGUILayout.EndHorizontal();
            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 135;
                pathFollower.arrowsSize = EditorGUILayout.FloatField("Movable arrows size: ", pathFollower.arrowsSize);
            }

            using (new GUILayout.VerticalScope("box"))
            {
                if (GUILayout.Button("Set this gizmos parameters for all Path Followers"))
                {
                    PathFollower[] scriptsOfPathFollower = FindObjectsOfType<PathFollower>();
                    foreach (PathFollower follower in scriptsOfPathFollower)
                    {
                        follower.colorOfLineOfControlPoint = pathFollower.colorOfLineOfControlPoint;
                        follower.controlPointsColor = pathFollower.controlPointsColor;
                        follower.movableControlPointColor = pathFollower.movableControlPointColor;
                        follower.wayColor = pathFollower.wayColor;
                        follower.wayPointsColor = pathFollower.wayPointsColor;

                        follower.controlPointsCap = pathFollower.controlPointsCap;
                        follower.controlPointsCapEnum = pathFollower.controlPointsCapEnum;

                        follower.movableControlPointCap = pathFollower.movableControlPointCap;
                        follower.movableControlPointCapEnum = pathFollower.movableControlPointCapEnum;

                        follower.wayPointsCap = pathFollower.wayPointsCap;
                        follower.wayPointsCapEnum = pathFollower.wayPointsCapEnum;

                        follower.controlPointsSize = pathFollower.controlPointsSize;
                        follower.movableControlPointSize = pathFollower.movableControlPointSize;
                        follower.wayPointsSize = pathFollower.wayPointsSize;
                        follower.waySize = pathFollower.waySize;

                        follower.arrowsSize = pathFollower.arrowsSize;
                    }
                }
            }
        }
        SceneView.RepaintAll();
    }

    Handles.CapFunction HandleCapSelection(PathFollower.HandleCapFunctions handleCapEnum)
    {
        Handles.CapFunction function = Handles.SphereHandleCap;

        switch (handleCapEnum)
        {
            case PathFollower.HandleCapFunctions.CircleCap:
                function = Handles.CircleHandleCap;
                break;
            case PathFollower.HandleCapFunctions.ConeCap:
                function = Handles.ConeHandleCap;
                break;
            case PathFollower.HandleCapFunctions.CubeCap:
                function = Handles.CubeHandleCap;
                break;
            case PathFollower.HandleCapFunctions.CylinterCap:
                function = Handles.CylinderHandleCap;
                break;
            case PathFollower.HandleCapFunctions.DotCap:
                function = Handles.DotHandleCap;
                break;
            case PathFollower.HandleCapFunctions.RectangleCap:
                function = Handles.RectangleHandleCap;
                break;
            case PathFollower.HandleCapFunctions.SphereCap:
                function = Handles.SphereHandleCap;
                break;

        }

        return function;
    }

    void PathOptions()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 120;
            pathFollower.objectToMove = EditorGUILayout.ObjectField("Object to move: ", pathFollower.objectToMove, typeof(Transform), true) as Transform;
        }

        using (new GUILayout.VerticalScope("box"))
        {
            pathFollower.usingCurves = EditorGUILayout.Toggle("Use curves: ", pathFollower.usingCurves);
        }
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 47;
                EditorGUILayout.LabelField("Way Point", GUILayout.Width(160));
                EditorGUILayout.LabelField("Speed", GUILayout.Width(45));
                EditorGUILayout.LabelField("Wait", GUILayout.Width(35));
                EditorGUILayout.LabelField("Method", GUILayout.Width(90));
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < pathFollower.points.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 20;
                    pathFollower.points[i].wayPointPosition = EditorGUILayout.ObjectField(+i + ": ", pathFollower.points[i].wayPointPosition, typeof(Transform), true, GUILayout.Width(160)) as Transform;
                    pathFollower.points[i].velocityInThisPoint = EditorGUILayout.FloatField(pathFollower.points[i].velocityInThisPoint, GUILayout.Width(45));
                    pathFollower.points[i].timeToWaitItInThisPoint = EditorGUILayout.FloatField(pathFollower.points[i].timeToWaitItInThisPoint, GUILayout.Width(35));
                    pathFollower.points[i].movementMethod = (PathFollower.MovementMethod)EditorGUILayout.EnumPopup(pathFollower.points[i].movementMethod, GUILayout.Width(90));

                    if (pathFollower.points[i].wayPointPosition != null && pathFollower.points[i].bezierPoint1 == null && pathFollower.points[i].bezierPoint2 == null)
                    {
                        GameObject point1 = new GameObject();
                        GameObject point2 = new GameObject();

                        point1.hideFlags = HideFlags.HideInHierarchy;
                        point2.hideFlags = HideFlags.HideInHierarchy;

                        point1.name = "ControlPoint1";
                        point2.name = "ControlPoint2";

                        point1.transform.position = pathFollower.points[i].wayPointPosition.position + new Vector3(-1, 1, 0);
                        point2.transform.position = pathFollower.points[i].wayPointPosition.position + new Vector3(1, 1, 0);

                        pathFollower.points[i].bezierPoint1 = point1.transform;
                        pathFollower.points[i].bezierPoint2 = point2.transform;

                        pathFollower.points[i].bezierPoint1.SetParent(pathFollower.points[i].wayPointPosition);
                        pathFollower.points[i].bezierPoint2.SetParent(pathFollower.points[i].wayPointPosition);
                    }


                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                    {
                        pathFollower.points.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();

                }
            }

            if (GUILayout.Button("Add new point"))
            {
                pathFollower.points.Add(null);
            }
        }

        if (pathFollower.usingCurves)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 155;
                pathFollower.showMovementArrows = EditorGUILayout.Toggle("Show movement arrows: ", pathFollower.showMovementArrows);
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 155;
            pathFollower.whenMovesTheObject = (PathFollower.WhenMovesTheObject)EditorGUILayout.EnumPopup("When it moves the object: ", pathFollower.whenMovesTheObject);

            if (pathFollower.whenMovesTheObject != PathFollower.WhenMovesTheObject.WhenTheGameStart && pathFollower.whenMovesTheObject != PathFollower.WhenMovesTheObject.CalledFromScripting)
            {
                if (pathFollower.GetComponent<Collider2D>() == null && pathFollower.GetComponent<Collider>() == null)
                    EditorGUILayout.LabelField("-> This object don't have a collider.", EditorStyles.boldLabel);
            }

            if (pathFollower.whenMovesTheObject == PathFollower.WhenMovesTheObject.CalledFromScripting)
                EditorGUILayout.LabelField("-> Read the documentation to activate the movement.", EditorStyles.boldLabel);

            pathFollower.howToMove = (PathFollower.HowToMove)EditorGUILayout.EnumPopup("How to move: ", pathFollower.howToMove);

            if (pathFollower.howToMove == PathFollower.HowToMove.FinishMovementWhenReachesTheEnd)
            {
                EditorGUIUtility.labelWidth = 180;
                pathFollower.canReturnTheObject = EditorGUILayout.Toggle("It can be returned the object?", pathFollower.canReturnTheObject);
            }
        }

        if (pathFollower.whenMovesTheObject != PathFollower.WhenMovesTheObject.WhenTheGameStart)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("Tags that can activate", EditorStyles.boldLabel);
                    for (int i = 0; i < pathFollower.tagsThatCanActivate.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUIUtility.labelWidth = 100;
                        pathFollower.tagsThatCanActivate[i] = EditorGUILayout.TagField("Tag: ", pathFollower.tagsThatCanActivate[i]);

                        if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
                        {
                            pathFollower.tagsThatCanActivate.RemoveAt(i);
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                if (GUILayout.Button("Add new tag"))
                {
                    pathFollower.tagsThatCanActivate.Add("");
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            pathFollower.followInZ = EditorGUILayout.Toggle("Follow axis Z: ", pathFollower.followInZ);
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 188;
            if (pathFollower.points.Count > 2)
            {
                pathFollower.loopMovement = EditorGUILayout.Toggle("Loop movement: ", pathFollower.loopMovement);
            }
            else
            {
                pathFollower.loopMovement = false;
            }

            pathFollower.turnObjWhenIsGoingBack = EditorGUILayout.Toggle("Turn object when is going back: ", pathFollower.turnObjWhenIsGoingBack);

            if (pathFollower.turnObjWhenIsGoingBack)
                pathFollower.currentDirection = (PathFollower.CurrentDirectionOfObject)EditorGUILayout.EnumPopup("Current direction of the obj: ", pathFollower.currentDirection);

            pathFollower.rotationOrLookAt = (PathFollower.RotationAndLookAt)EditorGUILayout.EnumPopup("Rotations/Look At: ", pathFollower.rotationOrLookAt);

            if (pathFollower.rotationOrLookAt == PathFollower.RotationAndLookAt.CopyRotationOfEachWayPoint)
            {
                pathFollower.showRotations = EditorGUILayout.Toggle("Show Rotations: ", pathFollower.showRotations);
                pathFollower.velocityToRotate = EditorGUILayout.FloatField("Velocity to rotate: ", pathFollower.velocityToRotate);
            }
            else if (pathFollower.rotationOrLookAt != PathFollower.RotationAndLookAt.Not)
            {
                pathFollower.rotationMethod = (PathFollower.RotationMethod)EditorGUILayout.EnumPopup("Rotation Method: ", pathFollower.rotationMethod);
                pathFollower.velocityToRotate = EditorGUILayout.FloatField("Velocity to rotate: ", pathFollower.velocityToRotate);
            }
        }
    }
}