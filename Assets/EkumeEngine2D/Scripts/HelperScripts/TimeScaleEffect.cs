using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class TimeScaleEffect : MonoBehaviour
{
    [SerializeField] float newTimeVelocity;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            Time.timeScale = newTimeVelocity;
            Time.fixedDeltaTime = newTimeVelocity * 0.02f;
        }
    }	

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 1 * 0.02f;
        }
    }

}
