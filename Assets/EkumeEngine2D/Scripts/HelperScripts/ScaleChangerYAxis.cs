using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleChangerYAxis : MonoBehaviour
{
    enum ActionActivator { OnTriggerEnter2D, OnCollisionEnter2D }

    [SerializeField] ActionActivator actionToActive;
    [SerializeField] List<string> tagsThatCanActivates = new List<string>();
    [SerializeField] Transform objectToScaler;
    [SerializeField] float newScaleInY;
    [SerializeField] float velocityToScaler;
    [SerializeField] bool recoverScale;
    [HideWhenFalse("recoverScale")]
    [SerializeField] float timeToKeepScale;
    [HideWhenFalse("recoverScale")]
    [SerializeField] float velocityToRecoverScale;

    bool startReduction;
    bool startRecoverScale;
    bool startTimerOfKeepScale;
    float timerOfKeepScale;

    float originalScaleInY;

    void Start ()
    {
        originalScaleInY = objectToScaler.transform.localScale.y;
    }

    bool TagIsInList(string tag)
    {
        bool tagIsInList = false;

        for (int i = 0; i < tagsThatCanActivates.Count; i++)
        {
            if (tagsThatCanActivates[i] == tag)
                tagIsInList = true;
        }

        return tagIsInList;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(actionToActive == ActionActivator.OnTriggerEnter2D && TagIsInList(other.tag))
        {
            if (!startReduction && !startTimerOfKeepScale)
                 startReduction = true;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (actionToActive == ActionActivator.OnCollisionEnter2D && TagIsInList(other.collider.tag))
        {
            if (!startReduction && !startTimerOfKeepScale)
                startReduction = true;
        }
    }

    void Update ()
    {
        if (startReduction)
        {
            objectToScaler.transform.localScale = Vector3.MoveTowards(objectToScaler.transform.localScale, new Vector3(objectToScaler.transform.localScale.x, newScaleInY, objectToScaler.transform.localScale.z), Time.deltaTime * velocityToScaler);
            startRecoverScale = false;
            if (Vector2.Distance(objectToScaler.transform.localScale, new Vector3(objectToScaler.transform.localScale.x, newScaleInY, objectToScaler.transform.localScale.z)) < 0.02f && recoverScale)
            {
                startTimerOfKeepScale = true;
                startReduction = false;
            }
        }

        if(startTimerOfKeepScale)
        {
            timerOfKeepScale += Time.deltaTime;

            if(timerOfKeepScale >= timeToKeepScale)
            {
                startRecoverScale = true;
                startTimerOfKeepScale = false;
                timerOfKeepScale = 0;
            }
        }

        if(startRecoverScale)
        {
            objectToScaler.transform.localScale = Vector3.MoveTowards(objectToScaler.transform.localScale, new Vector3(objectToScaler.transform.localScale.x, originalScaleInY, objectToScaler.transform.localScale.z), Time.deltaTime * velocityToRecoverScale);
        }
    }

}
