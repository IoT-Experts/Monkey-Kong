using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class InstantiateObjects_PlayerState : MonoBehaviour
{
    public List<PlayerStatesEnum> stateRequired = new List<PlayerStatesEnum>();
    public List<GameObject> objectToInstantiate = new List<GameObject>();
    public List<Transform> positionToInstantiate = new List<Transform>();
    public List<bool> instantiateWhileStateTrue = new List<bool>();
    public List<bool> destroyWhenStateFalse = new List<bool>();
    public List<bool> instantiateByFixedTime = new List<bool> ();
    public List<float> timeToKeepInstantiated = new List<float>();

    List<bool> canStartTimer = new List<bool>();
    List<float> timer = new List<float>();
    List<bool> startTimer = new List<bool>();
    List<GameObject> instantiatedObjects = new List<GameObject>();

    void Start ()
    {
        for(int i = 0; i < stateRequired.Count; i++)
        {
            canStartTimer.Add(true);
            timer.Add(0);
            startTimer.Add(false);
            instantiatedObjects.Add(null);
        }
    }

    void Update()
    {
        for (int i = 0; i < stateRequired.Count; i++)
        {
            if (instantiateWhileStateTrue[i] && PlayerStates.GetPlayerStateValue(stateRequired[i]))
            {
                if(instantiatedObjects[i] == null)
                    instantiatedObjects[i] = (Instantiate(objectToInstantiate[i], positionToInstantiate[i].position, positionToInstantiate[i].rotation) as GameObject);
            }
            else if (canStartTimer[i] && instantiateByFixedTime[i] && PlayerStates.GetPlayerStateValue(stateRequired[i]))
            {
                if (!startTimer[i])
                {
                    startTimer[i] = true;

                    if (instantiatedObjects[i] == null)
                        instantiatedObjects[i] = (Instantiate(objectToInstantiate[i], positionToInstantiate[i].position, positionToInstantiate[i].rotation) as GameObject);

                    canStartTimer[i] = false;
                }
            }

            if (startTimer[i])
            {
                timer[i] += Time.deltaTime;

                if (timer[i] >= timeToKeepInstantiated[i])
                {
                    if (instantiatedObjects[i] != null)
                        Destroy(instantiatedObjects[i]);

                    timer[i] = 0;
                    startTimer[i] = false;
                }
            }

            if (!PlayerStates.GetPlayerStateValue(stateRequired[i]))
            {
                canStartTimer[i] = true;
                if (destroyWhenStateFalse[i])
                {
                    if (instantiatedObjects[i] != null)
                        Destroy(instantiatedObjects[i]);
                }
            }
        }
    }
}
