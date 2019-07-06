using UnityEngine;
using System.Collections;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class VelocityInversorOfPlayer : MonoBehaviour
{
    [SerializeField] enum ActivatorOfAction { OnCollisionEnter2D, OnTriggerEnter2D }
    [Header("Only recommended when player have constant velocity", order = 0)]
    [Header("This script reverses the velocity of player", order = 1)]
    [SerializeField] ActivatorOfAction activatorOfAction;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activatorOfAction == ActivatorOfAction.OnTriggerEnter2D)
        {
            if (other.tag == "Player" | (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
            {
                if(other.GetComponent<PlayerHorizontalMovement>() != null)
                    InvertVelocity(other.GetComponent<PlayerHorizontalMovement>());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (activatorOfAction == ActivatorOfAction.OnCollisionEnter2D)
        {
            if (other.collider.tag == "Player" || (other.collider.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
            {
                if (other.collider.GetComponent<PlayerHorizontalMovement>() != null)
                    InvertVelocity(other.collider.GetComponent<PlayerHorizontalMovement>());
            }
        }
    }

    void InvertVelocity (PlayerHorizontalMovement playerMovement)
    {
        playerMovement.velocity = playerMovement.velocity * (-1);
    }

}
