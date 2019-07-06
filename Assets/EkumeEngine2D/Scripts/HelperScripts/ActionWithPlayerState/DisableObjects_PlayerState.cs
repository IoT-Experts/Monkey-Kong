using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class DisableObjects_PlayerState : MonoBehaviour
{
    public List<PlayerStatesEnum> stateActivator = new List<PlayerStatesEnum>();
    public List<GameObject> objectToDisable = new List<GameObject>();
    public List<bool> disableWhileStateTrue = new List<bool>();
    public List<bool> enableWhenStateFalse = new List<bool>();
    public List<bool> disableByFixedTime = new List<bool> ();
    public List<float> timeToKeepDisabled = new List<float>();

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
            if (disableWhileStateTrue[i] && PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                objectToDisable[i].SetActive(false);
            }
            else if (canStartTimer[i] && disableByFixedTime[i] && PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                if (!startTimer[i])
                {
                    startTimer[i] = true;
                    objectToDisable[i].SetActive(false);
                    canStartTimer[i] = false;
                }
            }

            if (startTimer[i])
            {
                timer[i] += Time.deltaTime;

                if (timer[i] >= timeToKeepDisabled[i])
                {
                    objectToDisable[i].SetActive(true);
                    timer[i] = 0;
                    startTimer[i] = false;
                }
            }

            if (!PlayerStates.GetPlayerStateValue(stateActivator[i]))
            {
                canStartTimer[i] = true;
                if (enableWhenStateFalse[i])
                {
                    objectToDisable[i].SetActive(true);
                }
            }
        }
    }
}
