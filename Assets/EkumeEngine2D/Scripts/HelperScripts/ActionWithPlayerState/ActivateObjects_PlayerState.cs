using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class ActivateObjects_PlayerState : MonoBehaviour
{
    public List<PlayerStatesEnum> stateActivator = new List<PlayerStatesEnum>();
    public List<GameObject> objectToActivates = new List<GameObject>();
    public List<bool> activateWhileStateTrue = new List<bool>();
    public List<bool> disableWhenStateFalse = new List<bool>();
    public List<bool> activateByFixedTime = new List<bool> ();
    public List<float> timeToKeepEnable = new List<float>();

    List<bool> canStartTimer = new List<bool>();
    List<float> timer = new List<float>();
    List<bool> startTimer = new List<bool>();

    void Start ()
    {
        for(int i = 0; i < stateActivator.Count; i++)
        {
            canStartTimer.Add(true);
            timer.Add(0);
            startTimer.Add(false);
        }
    }

    void Update()
    {
        for (int i = 0; i < stateActivator.Count; i++)
        {
            if (activateWhileStateTrue[i] && PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                objectToActivates[i].SetActive(true);
            }
            else if (canStartTimer[i] && activateByFixedTime[i] && PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                if (!startTimer[i])
                {
                    startTimer[i] = true;
                    objectToActivates[i].SetActive(true);
                    canStartTimer[i] = false;
                }
            }

            if (startTimer[i])
            {
                timer[i] += Time.deltaTime;

                if (timer[i] >= timeToKeepEnable[i])
                {
                    objectToActivates[i].SetActive(false);
                    timer[i] = 0;
                    startTimer[i] = false;
                }
            }

            if (!PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                canStartTimer[i] = true;
                if (disableWhenStateFalse[i])
                {
                    objectToActivates[i].SetActive(false);
                }
            }
        }
    }
}
