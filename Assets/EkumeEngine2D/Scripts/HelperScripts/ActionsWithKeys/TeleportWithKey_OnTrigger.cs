using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class TeleportWithKey_OnTrigger : MonoBehaviour
{
    public int inputControl;
    public Transform positionToTeleport;
    public InputControlsManager inputControlsManager;

    bool playerIsInTrigger;

    Transform player;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            playerIsInTrigger = true;
            player = other.transform;
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            playerIsInTrigger = false;
            player = other.transform;
        }
    }

    void Update ()
    {
        if(playerIsInTrigger)
        {
            if(InputControls.GetControlDown(inputControl))
            {
                player.transform.position = new Vector3(positionToTeleport.position.x, positionToTeleport.position.y, player.position.z);
                playerIsInTrigger = false;
            }
        }

    }


}
