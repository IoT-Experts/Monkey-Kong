using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectInstantiator : MonoBehaviour
{

    enum WhenStart { OnTriggerEnter2D, OnCollisionEnter2D }
    [SerializeField] bool instantiateWhenGameStart;
    [HideWhenTrue("instantiateWhenGameStart")]
    [SerializeField] WhenStart whenStartToInstantiate;
    [SerializeField] Transform whereInstantiate;
    [HideWhenTrue("instantiateWhenGameStart")]
    [Header("Fill the tags only if will start with collision or trigger")]
    [SerializeField] List<string> tagsToActivate;
    [SerializeField] List<GameObject> objToInstantiate = new List<GameObject>();
    [SerializeField] float timeToInstantiate;
    [SerializeField] float timeToNextGroup;
    [SerializeField] int maxGroups;

    float timerToInstantiate;
    float timerToNextGroup;
    int objsInstantiatedOfList;
    bool inTimerToNextGroup;
    int groupsInstantiated;
    bool startToInstantiate;

    void Start ()
    {
        if (instantiateWhenGameStart)
            startToInstantiate = true;
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (!instantiateWhenGameStart)
        {
            if (whenStartToInstantiate == WhenStart.OnCollisionEnter2D && tagsToActivate.Contains(other.collider.tag))
            {
                startToInstantiate = true;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (!instantiateWhenGameStart)
        {
            if (whenStartToInstantiate == WhenStart.OnTriggerEnter2D && tagsToActivate.Contains(other.tag))
            {
                startToInstantiate = true;
            }
        }
    }

    void Update()
    {
        if (startToInstantiate)
        {
            if (groupsInstantiated < maxGroups)
            {
                if (!inTimerToNextGroup)
                {
                    timerToInstantiate += Time.deltaTime;
                    if (timerToInstantiate >= timeToInstantiate)
                    {
                        if (objsInstantiatedOfList < objToInstantiate.Count)
                        {
                            Instantiate(objToInstantiate[objsInstantiatedOfList], whereInstantiate.position, objToInstantiate[objsInstantiatedOfList].transform.rotation);
                            timerToInstantiate = 0;
                            objsInstantiatedOfList++;

                            if (objsInstantiatedOfList >= objToInstantiate.Count)
                            {
                                objsInstantiatedOfList = 0;
                                inTimerToNextGroup = true;
                            }
                        }
                    }
                }
                else
                {
                    timerToNextGroup += Time.deltaTime;

                    if (timerToNextGroup > timeToNextGroup)
                    {
                        inTimerToNextGroup = false;
                        timerToNextGroup = 0;
                        groupsInstantiated++;
                    }
                }
            }
        }
    }
}
