using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Point
{
    public Transform wayPointPosition;
    public float velocityInThisPoint;
    public float timeToWaitItInThisPoint;
    public Transform bezierPoint1;
    public Transform bezierPoint2;
    public PathFollower.MovementMethod movementMethod;

    public Point (Transform wayPointPosition, float velocityInThisPoint, float timeToWaitItInThisPoint, PathFollower.MovementMethod howToMove)
    {
        this.wayPointPosition = wayPointPosition;
        this.velocityInThisPoint = velocityInThisPoint;
        this.timeToWaitItInThisPoint = timeToWaitItInThisPoint;
        this.movementMethod = howToMove;
    }
}

public class PathFollower : MonoBehaviour
{
    public enum WhenMovesTheObject { WhenTheGameStart, WhenATagCollidesWithThis, WhenATagTriggerWithThis, WhileATagKeepCollisionWithThis, WhileATagKeepTriggerWithThis, CalledFromScripting }
    public enum HowToMove { ConstantMovement, FinishMovementWhenReachesTheEnd }
    public enum MovementMethod { MoveTowards, Lerp }
    public enum RotationAndLookAt { Not, LookAt2D, LookAt3D, CopyRotationOfEachWayPoint }
    public enum HandleCapFunctions { CircleCap, ConeCap, CubeCap, CylinterCap, DotCap, SphereCap, RectangleCap }
    public enum CurrentDirectionOfObject { Right, Left }
    public enum RotationMethod { Slerp, RotateTowards, Lerp }

    public WhenMovesTheObject whenMovesTheObject;
    public HowToMove howToMove;
    public CurrentDirectionOfObject currentDirection;
    public List<string> tagsThatCanActivate = new List<string>();
    //if selected is: FinishMovementWhenReachesTheEnd
    public bool canReturnTheObject;

    public bool usingCurves;
    public Transform objectToMove;
    public List<Point> points = new List<Point>() { new Point(null, 0, 0, MovementMethod.MoveTowards) };
    public bool loopMovement;
    public bool turnObjWhenIsGoingBack;
    public RotationAndLookAt rotationOrLookAt;
    public RotationMethod rotationMethod;
    public float velocityToRotate;

#if UNITY_EDITOR

    public bool showRotations;
    public bool showMovementArrows;

    public Color wayColor = new Vector4(1, 0.92f, 0.016f, 0.75f);
    public float waySize = 2;

    public Color controlPointsColor = Color.blue;
    public Handles.CapFunction controlPointsCap = Handles.CircleHandleCap;
    public HandleCapFunctions controlPointsCapEnum = HandleCapFunctions.CircleCap;
    public float controlPointsSize = 0.27f;

    public Color colorOfLineOfControlPoint = Color.white;
    public Color wayPointsColor = new Vector4(1, 0.92f, 0.016f, 0.75f);

    public Handles.CapFunction wayPointsCap = Handles.SphereHandleCap;
    public HandleCapFunctions wayPointsCapEnum = HandleCapFunctions.SphereCap;
    public float wayPointsSize = 0.2f;

    public Color movableControlPointColor = Color.magenta;
    public Handles.CapFunction movableControlPointCap = Handles.CubeHandleCap;
    public HandleCapFunctions movableControlPointCapEnum = HandleCapFunctions.CubeCap;
    public float movableControlPointSize = 0.2f;

    public float arrowsSize = 1.15f;

#endif

    public bool followInZ = true;

    public float bezierTime = 0;

    public int nextPointNumber = 0;
    public int currentPointNumber = 0;

    bool positiveDirection = false;

    public bool startMovement = false;
    bool reachedTheEnd;

    bool startedTheMovement;

    bool timeWaitedToMove;
    float timerToWaint;


#if UNITY_EDITOR

    void OnDrawGizmos()
    {
        Handles.color = wayColor;
        if (points.Count > 2 && loopMovement && points[points.Count - 1] != null && points[points.Count - 1].wayPointPosition != null)
        {
            if (usingCurves)
            {
                Handles.DrawBezier(points[0].wayPointPosition.position,
                    points[points.Count - 1].wayPointPosition.position,
                    points[points.Count - 1].bezierPoint1.position,
                    points[points.Count - 1].bezierPoint2.position,
                    wayColor,
                    null,
                    waySize);
            }
            else
            {
                Handles.color = wayColor;
                Handles.DrawLine(points[0].wayPointPosition.position, points[points.Count - 1].wayPointPosition.position);
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i] != null && points[i].wayPointPosition != null)
            {

                if (i != points.Count - 1 && points[i + 1] != null && points[i + 1].wayPointPosition != null)
                {
                    if (usingCurves)
                    {
                        Handles.DrawBezier(points[i].wayPointPosition.position,
                            points[i + 1].wayPointPosition.position,
                            points[i].bezierPoint1.position,
                            points[i].bezierPoint2.position,
                            wayColor,
                           null,
                            waySize);
                    }
                    else
                    {
                        Handles.color = wayColor;
                        Handles.DrawLine(points[i].wayPointPosition.position, points[i + 1].wayPointPosition.position);
                    }
                }

                Handles.Label(points[i].wayPointPosition.position, i.ToString(), EditorStyles.boldLabel);
                Handles.color = wayPointsColor;
                wayPointsCap(i, points[i].wayPointPosition.position, points[i].wayPointPosition.rotation, wayPointsSize, EventType.Repaint); //The repaint event was added unnecessarily. Any event is necessary but the function request some event. The event was added for Unity 5.6

            }
        }
    }
    

#endif

    void Start()
    {
        if (points.Count > 0 && points[0].wayPointPosition != null && objectToMove != null)
        {
            objectToMove.position = new Vector3(
                    points[0].wayPointPosition.position.x,
                    points[0].wayPointPosition.position.y,
                    ((followInZ) ? points[0].wayPointPosition.position.z : objectToMove.position.z));
        }

        if (whenMovesTheObject == WhenMovesTheObject.WhenTheGameStart)
        {
            startMovement = true;
        }
    }

    //2D COLLISIONS
    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagsThatCanActivate.Contains(other.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhenATagTriggerWithThis || whenMovesTheObject == WhenMovesTheObject.WhileATagKeepTriggerWithThis)
            {
                if ((!reachedTheEnd && howToMove == HowToMove.FinishMovementWhenReachesTheEnd) || howToMove == HowToMove.ConstantMovement)
                    startMovement = true;
            }

            if (reachedTheEnd && canReturnTheObject)
            {
                startMovement = true;
                reachedTheEnd = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tagsThatCanActivate.Contains(other.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhileATagKeepTriggerWithThis)
            {
                startMovement = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (tagsThatCanActivate.Contains(other.collider.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhenATagCollidesWithThis || whenMovesTheObject == WhenMovesTheObject.WhileATagKeepCollisionWithThis)
            {
                if ((!reachedTheEnd && howToMove == HowToMove.FinishMovementWhenReachesTheEnd) || howToMove == HowToMove.ConstantMovement)
                    startMovement = true;
            }

            if (reachedTheEnd && canReturnTheObject)
            {
                startMovement = true;
                reachedTheEnd = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (tagsThatCanActivate.Contains(other.collider.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhileATagKeepCollisionWithThis)
            {
                startMovement = false;
            }
        }
    }

    //3D COLLISIONS
    void OnTriggerEnter(Collider other)
    {
        if (tagsThatCanActivate.Contains(other.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhenATagTriggerWithThis || whenMovesTheObject == WhenMovesTheObject.WhileATagKeepTriggerWithThis)
            {
                if ((!reachedTheEnd && howToMove == HowToMove.FinishMovementWhenReachesTheEnd) || howToMove == HowToMove.ConstantMovement)
                    startMovement = true;
            }

            if (reachedTheEnd && canReturnTheObject)
            {
                startMovement = true;
                reachedTheEnd = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (tagsThatCanActivate.Contains(other.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhileATagKeepTriggerWithThis)
            {
                startMovement = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (tagsThatCanActivate.Contains(other.collider.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhenATagCollidesWithThis || whenMovesTheObject == WhenMovesTheObject.WhileATagKeepCollisionWithThis)
            {
                if ((!reachedTheEnd && howToMove == HowToMove.FinishMovementWhenReachesTheEnd) || howToMove == HowToMove.ConstantMovement)
                    startMovement = true;
            }

            if (reachedTheEnd && canReturnTheObject)
            {
                startMovement = true;
                reachedTheEnd = false;
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (tagsThatCanActivate.Contains(other.collider.tag))
        {
            if (whenMovesTheObject == WhenMovesTheObject.WhileATagKeepCollisionWithThis)
            {
                startMovement = false;
            }
        }
    }

    void ChangeDirectionOfTheObject ()
    {
        if (currentDirection == CurrentDirectionOfObject.Left)
            currentDirection = CurrentDirectionOfObject.Right;
        else
            currentDirection = CurrentDirectionOfObject.Left;

        objectToMove.localScale = new Vector3(objectToMove.localScale.x * -1, objectToMove.localScale.y, objectToMove.localScale.z);
    }

    void RotateObject (Quaternion newAngle)
    {
        if(rotationMethod == RotationMethod.Slerp)
            objectToMove.rotation = Quaternion.Slerp(objectToMove.rotation, newAngle, velocityToRotate * Time.deltaTime);
        else if (rotationMethod == RotationMethod.RotateTowards)
            objectToMove.rotation = Quaternion.RotateTowards(objectToMove.rotation, newAngle, velocityToRotate * Time.deltaTime * 10);
        else if (rotationMethod == RotationMethod.Lerp)
            objectToMove.rotation = Quaternion.Lerp(objectToMove.rotation, newAngle, velocityToRotate * Time.deltaTime);
    }

    void Update()
    {
        if (rotationOrLookAt == RotationAndLookAt.LookAt2D)
        {
            Vector3 dir = points[nextPointNumber].wayPointPosition.position - objectToMove.position;
            float angle = 0;
            if (currentDirection == CurrentDirectionOfObject.Right)
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            else
                angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;

            RotateObject(Quaternion.AngleAxis(angle, Vector3.forward));
        }
        else if (rotationOrLookAt == RotationAndLookAt.LookAt3D)
        {
            Vector3 dir = points[nextPointNumber].wayPointPosition.position - objectToMove.position;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            // slerp to the desired rotation over time
            RotateObject(rot);
        }
        else if (rotationOrLookAt == RotationAndLookAt.CopyRotationOfEachWayPoint)
        {
            RotateObject(points[currentPointNumber].wayPointPosition.rotation);
        }

        if (startMovement && objectToMove != null && points.Count > 0 && points[nextPointNumber].wayPointPosition != null)
        {
            if (Vector3.Distance(
                objectToMove.position,
                new Vector3(
                    points[nextPointNumber].wayPointPosition.position.x,
                    points[nextPointNumber].wayPointPosition.position.y,
                    ((followInZ) ? points[nextPointNumber].wayPointPosition.position.z : objectToMove.position.z))
                ) <= 0.05f)
            {
                if (nextPointNumber == 0)
                {
                    positiveDirection = true;

                    if (turnObjWhenIsGoingBack && startedTheMovement)
                    {
                        ChangeDirectionOfTheObject();
                    }

                    startedTheMovement = true;
                }

                if (nextPointNumber == points.Count - 1)
                {
                    if (howToMove == HowToMove.FinishMovementWhenReachesTheEnd)
                    {
                        startMovement = false;
                    }

                    reachedTheEnd = true;

                    positiveDirection = false;

                    if (turnObjWhenIsGoingBack)
                    {
                        ChangeDirectionOfTheObject();
                    }
                }

                if (positiveDirection)
                {
                    nextPointNumber += 1;
                    currentPointNumber = nextPointNumber - 1;
                }
                else
                {
                    if (loopMovement)
                    {
                        nextPointNumber = 0;
                        currentPointNumber = points.Count - 1;
                    }
                    else
                    {
                        nextPointNumber -= 1;
                        currentPointNumber = nextPointNumber + 1;
                    }
                }

                timeWaitedToMove = false;
            }

            if (!timeWaitedToMove)
            {
                timerToWaint += Time.deltaTime;

                if (timerToWaint > points[currentPointNumber].timeToWaitItInThisPoint)
                {
                    timeWaitedToMove = true;
                    timerToWaint = 0;

                    bezierTime = 0;
                }
            }

            if (startMovement && timeWaitedToMove) //Avoid to move if the variable "startMovement" is turned false inside the conditional.
            {
                if (usingCurves)
                {
                    if (positiveDirection)
                    {
                        if (nextPointNumber != 0)
                        {
                            Vector3 newPosition = CurvePosition(points[currentPointNumber].wayPointPosition.position, points[currentPointNumber].bezierPoint1.position, points[currentPointNumber].bezierPoint2.position, points[nextPointNumber].wayPointPosition.position, points[currentPointNumber].velocityInThisPoint, points[currentPointNumber].movementMethod);
                            objectToMove.position = new Vector3(newPosition.x, newPosition.y, ((followInZ) ? newPosition.z : objectToMove.position.z));
                        }
                    }
                    else
                    {
                        if (nextPointNumber == 0 && currentPointNumber == points.Count - 1 && points.Count > 2)
                        {
                            Vector3 newPosition = objectToMove.position = CurvePosition(points[currentPointNumber].wayPointPosition.position, points[currentPointNumber].bezierPoint2.position, points[currentPointNumber].bezierPoint1.position, points[nextPointNumber].wayPointPosition.position, points[currentPointNumber].velocityInThisPoint, points[currentPointNumber].movementMethod);
                            objectToMove.position = new Vector3(newPosition.x, newPosition.y, ((followInZ) ? newPosition.z : objectToMove.position.z));
                        }
                        else
                        {
                            Vector3 newPosition = CurvePosition(points[currentPointNumber].wayPointPosition.position, points[nextPointNumber].bezierPoint2.position, points[nextPointNumber].bezierPoint1.position, points[nextPointNumber].wayPointPosition.position, points[nextPointNumber].velocityInThisPoint, points[currentPointNumber].movementMethod);
                            objectToMove.position = new Vector3(newPosition.x, newPosition.y, ((followInZ) ? newPosition.z : objectToMove.position.z));
                        }
                    }
                }
                else
                {
                    if (points[currentPointNumber].movementMethod == MovementMethod.MoveTowards)
                    {
                        objectToMove.position = Vector3.MoveTowards(objectToMove.position,
                            new Vector3(
                            points[nextPointNumber].wayPointPosition.position.x,
                            points[nextPointNumber].wayPointPosition.position.y,
                            ((followInZ) ? points[nextPointNumber].wayPointPosition.position.z : objectToMove.position.z)),
                            Time.deltaTime * points[nextPointNumber].velocityInThisPoint);
                    }
                    else
                    {
                        objectToMove.position = Vector3.Lerp(objectToMove.position,
                           new Vector3(
                           points[nextPointNumber].wayPointPosition.position.x,
                           points[nextPointNumber].wayPointPosition.position.y,
                           ((followInZ) ? points[nextPointNumber].wayPointPosition.position.z : objectToMove.position.z)),
                           Time.deltaTime * points[nextPointNumber].velocityInThisPoint);
                    }
                }
            }
        }
    }

    Vector3 CurvePosition(Vector3 startPosition, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPosition, float velocity, MovementMethod movementMethod)
    {
        List<Vector3> pointsInCurve = new List<Vector3>();
        float distance = 0;

        for(float i = 0; i <= 1; i += 0.005f)
        {
            pointsInCurve.Add((((((-startPosition + (3 * (controlPoint1 - controlPoint2)) + endPosition) * i) + ((3 * (startPosition + controlPoint2)) - (6 * controlPoint1))) * i + (3 * (controlPoint1 - startPosition))) * i) + startPosition);
        }

        for (int i = 0; i < pointsInCurve.Count - 1; i++)
        {
            distance += Vector3.Distance(pointsInCurve[i], pointsInCurve[i + 1]);
        }

        if (movementMethod == MovementMethod.MoveTowards)
        {
           bezierTime = Mathf.MoveTowards(bezierTime, 1, velocity * Time.deltaTime / distance);
        }
        else
            bezierTime = Mathf.Lerp(bezierTime, 1, velocity * Time.deltaTime / distance);

        return (((((-startPosition + (3 * (controlPoint1 - controlPoint2)) + endPosition) * bezierTime) + ((3 * (startPosition + controlPoint2)) - (6 * controlPoint1))) * bezierTime + (3 * (controlPoint1 - startPosition))) * bezierTime) + startPosition;
   }  

}
