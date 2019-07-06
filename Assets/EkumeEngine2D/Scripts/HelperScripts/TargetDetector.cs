using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TargetDetector : MonoBehaviour
{
    [SerializeField] List<string> tagsOfTargets = new List<string>();

    [HideInInspector] public Transform locatedTarget;
    List<Transform> objectsInTrigger = new List<Transform>();

    bool wantToDetectPlayer;

    void Awake ()
    {
        if (tagsOfTargets.Contains("Player"))
            wantToDetectPlayer = true;
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            if (!GetComponent<Rigidbody2D>())
            {
                if (transform.parent == null)
                {
                    Debug.LogWarning("The Rigidbody2D with the component TargetDetector should be marked as Kinematic. Please mark as kinematic the Rigidbody2D of the GameObject " + gameObject.name);
                }
                else
                {
                    Debug.LogWarning("The Rigidbody2D with the component TargetDetector should be marked as Kinematic. Please mark as kinematic the Rigidbody2D of the GameObject " + gameObject.name + " (His parent is: " + transform.parent.name + ")");
                }
            }
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagsOfTargets.Contains(other.tag))
        {
            if(!objectsInTrigger.Contains(other.transform))
                objectsInTrigger.Add(other.transform);
        }

        if (wantToDetectPlayer)
        {
            if (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (!objectsInTrigger.Contains(other.transform.GetComponentInChildren<Player>().transform))
                    objectsInTrigger.Add(other.transform.GetComponentInChildren<Player>().transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tagsOfTargets.Contains(other.tag))
        {
            if(objectsInTrigger.Contains(other.transform))
               objectsInTrigger.Remove(other.transform);
        }

        if (wantToDetectPlayer)
        {
            if (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (objectsInTrigger.Contains(other.transform.GetComponentInChildren<Player>().transform))
                    objectsInTrigger.Remove(other.transform.GetComponentInChildren<Player>().transform);
            }
        }
    }

    void LateUpdate()
    {
        for(int i = 0; i < objectsInTrigger.Count; i++)
        {
            if(objectsInTrigger[i] == null)
                objectsInTrigger.RemoveAt(i);
        }

        if (!objectsInTrigger.Contains(null))
        {
            if (objectsInTrigger.Count > 0)
            {
                if (objectsInTrigger.Count > 1)
                {
                    float[] theDistances = new float[objectsInTrigger.Count];
                    for (int i = 0; i < objectsInTrigger.Count; i++)
                    {
                        theDistances[i] = Vector2.Distance(transform.position, objectsInTrigger[i].position);
                    }

                    locatedTarget = objectsInTrigger[0];
                    for (int i = 1; i < objectsInTrigger.Count; i++)
                    {
                        if (theDistances[i] < theDistances[i - 1])
                        {
                            locatedTarget = objectsInTrigger[i];
                        }
                    }
                }
                else if (objectsInTrigger.Count == 1)
                {
                    locatedTarget = objectsInTrigger[0];
                }
            }
            else if (objectsInTrigger.Count == 0)
            {
                locatedTarget = null;
            }
        }
    }
}