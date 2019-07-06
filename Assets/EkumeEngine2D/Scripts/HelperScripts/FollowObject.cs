using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour
{
    public enum MovementMethod { Lerp, MoveTowards }
    [Space()]
    [SerializeField] bool moveInstantly;
    [HideWhenTrue("moveInstantly")]
    [SerializeField] MovementMethod movementMethod;
    [HideWhenTrue("moveInstantly")]
    [SerializeField] float velocityToMove;
    [Space()]
    [SerializeField] Transform targetToFollow;

    [SerializeField] bool followInX;
    [HideWhenFalse("followInX")]
    [SerializeField] float xSeparation;

    [SerializeField] bool followInY;
    [HideWhenFalse("followInY")]
    [SerializeField] float ySeparation;

    bool isChild;

#if UNITY_EDITOR
    void Start ()
    {
        if (targetToFollow == null)
            Debug.LogError("The variable \"Target To Follow\" of the script " + GetType().Name + " in the GameObject " + gameObject.name + " is null. Please fill the variable with the GameObject to follow.");
    }
#endif

    void Awake ()
    {
        if (moveInstantly && followInX && followInY)
        {
            isChild = true;
            transform.position = new Vector3(
                  (followInX) ? targetToFollow.position.x + xSeparation : transform.position.x,
                  (followInY) ? targetToFollow.position.y + ySeparation : transform.position.y,
                  transform.position.z);

            transform.SetParent(targetToFollow);
        }
    }

    void Update ()
    {
        if (moveInstantly && !isChild)
        {
            transform.position = new Vector3 (
                (followInX) ? targetToFollow.position.x + xSeparation : transform.position.x,
                (followInY) ? targetToFollow.position.y + ySeparation : transform.position.y,
                transform.position.z);
        }
        else if (movementMethod == MovementMethod.Lerp)
        {
            transform.position = Vector3.Lerp (transform.position, new Vector3 (
                 (followInX) ? targetToFollow.position.x + xSeparation : transform.position.x,
                 (followInY) ? targetToFollow.position.y + ySeparation : transform.position.y,
                 transform.position.z), Time.deltaTime * velocityToMove);
        }
        else if (movementMethod == MovementMethod.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (
                  (followInX) ? targetToFollow.position.x + xSeparation : transform.position.x,
                  (followInY) ? targetToFollow.position.y + ySeparation : transform.position.y,
                  transform.position.z), Time.deltaTime * velocityToMove);
        }
    }
}
