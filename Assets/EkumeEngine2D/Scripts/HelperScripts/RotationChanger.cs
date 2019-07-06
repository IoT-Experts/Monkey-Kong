using UnityEngine;
using System.Collections;

public class RotationChanger : MonoBehaviour {

    public enum RotationType { MoveTowards, Lerp }
    [SerializeField] enum ActivatorOfAction { OnTriggerEnter2D, OnCollisionEnter2D, WhenTheObjectStart }
    [SerializeField] ActivatorOfAction activatorOfAction;
	[SerializeField] Transform objectToRotate;
    [SerializeField] Vector3 newValues;
    [SerializeField] bool rotateInstantly;
    [HideWhenTrue("rotateInstantly")]
    [SerializeField] RotationType rotationType;
    [HideWhenTrue("rotateInstantly")]
    [SerializeField] float velocityToRotate;
    
    bool startTimer;

    void Start ()
    {
        if (activatorOfAction == ActivatorOfAction.WhenTheObjectStart)
            RotateObject();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activatorOfAction == ActivatorOfAction.OnTriggerEnter2D)
                RotateObject();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            if (activatorOfAction == ActivatorOfAction.OnCollisionEnter2D)
                RotateObject();
        }
    }

    void RotateObject()
    {
        if (rotateInstantly)
            objectToRotate.Rotate(newValues);
        else
            startTimer = true;
    }

    void Update ()
    {
        if (startTimer)
        {            
            if(rotationType == RotationType.Lerp)
                objectToRotate.eulerAngles = Vector3.Lerp(objectToRotate.eulerAngles, newValues, Time.deltaTime * velocityToRotate);
            else if (rotationType == RotationType.MoveTowards)
                objectToRotate.eulerAngles = Vector3.MoveTowards(objectToRotate.eulerAngles, newValues, Time.deltaTime * velocityToRotate);

            if (Vector3.Distance(objectToRotate.localEulerAngles, newValues) <= 0.05)
                startTimer = false;
        }
    }
}
