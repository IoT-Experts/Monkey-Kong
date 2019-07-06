using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{ 
    void Awake ()
    {
        if(activeEvents == null)
            InicializeStates(); //Start all events in false
    }

    void OnEnable()
    {
        if (activeEvents == null)
            InicializeStates(); //Start all events in false
    }

    public Dictionary<EnemyStatesEnum, bool> activeEvents;

    public void InicializeStates()
    {
        activeEvents = new Dictionary<EnemyStatesEnum, bool>();

        activeEvents.Add(EnemyStatesEnum.EnemyDie, false);
        activeEvents.Add(EnemyStatesEnum.EnemyDirectionIsLeft, false);
        activeEvents.Add(EnemyStatesEnum.EnemyDirectionIsRight, false);
        activeEvents.Add(EnemyStatesEnum.EnemyIsGrounded, false);
        activeEvents.Add(EnemyStatesEnum.EnemyIsMoving, false);
        activeEvents.Add(EnemyStatesEnum.EnemyLoseHealthPoint, false);
        activeEvents.Add(EnemyStatesEnum.EnemyAttack, false);
    }

    public void SetStateValue(EnemyStatesEnum eventToChange, bool newValue)
    {
        activeEvents[eventToChange] = newValue;
    }

    public bool GetStateValue(EnemyStatesEnum customEvent)
    {
        return activeEvents[customEvent];
    }
}
