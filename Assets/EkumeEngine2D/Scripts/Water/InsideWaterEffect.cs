using UnityEngine;
using System.Collections;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class InsideWaterEffect : MonoBehaviour
{

    [SerializeField] float newRigidbodyGravity = 3; // 3 recomended
    [SerializeField] float newRigidbodyLinearDrag = 20; // 20 recomended
    [SerializeField] float velocityToReduceInsideWater = 3; // 3 recomended

    static float newVelocityForPlayer = 0;
    static float newVelocityForMount = 0;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (newVelocityForPlayer == 0)
            {
                if (other.GetComponent<PlayerHorizontalMovement>().originalVelocity >= 0)
                {
                    newVelocityForPlayer = other.GetComponent<PlayerHorizontalMovement>().velocity - velocityToReduceInsideWater;
                }
                else
                {
                    newVelocityForPlayer = other.GetComponent<PlayerHorizontalMovement>().velocity + velocityToReduceInsideWater;
                }
            }
        }
        else if (other.tag == "Mount")
        {
            if (newVelocityForMount == 0)
            {
                if (other.GetComponent<PlayerHorizontalMovement>().originalVelocity >= 0)
                {
                    newVelocityForMount = other.GetComponent<PlayerHorizontalMovement>().velocity - velocityToReduceInsideWater;
                }
                else
                {
                    newVelocityForMount = other.GetComponent<PlayerHorizontalMovement>().velocity + velocityToReduceInsideWater;
                }
            }
        }
	}
    
    void OnTriggerStay2D (Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            other.GetComponent<Rigidbody2D>().gravityScale = newRigidbodyGravity;
            other.GetComponent<Rigidbody2D>().drag = newRigidbodyLinearDrag;

            if (other.GetComponent<PlayerJump>() != null)
            {
                other.GetComponent<PlayerJump>().activateDoubleJump = false;
                other.GetComponent<PlayerJump>().noLimitOfJumps = true;
            }

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.IsUnderWater, true);
        }

        if(other.tag == "Player")
        {
            if (other.GetComponent<PlayerHorizontalMovement>() != null)
            {
                other.GetComponent<PlayerHorizontalMovement>().velocity = newVelocityForPlayer;
            }
        }
        else if (other.tag == "Mount")
        {
            if (other.GetComponent<PlayerHorizontalMovement>() != null)
            {
                other.GetComponent<PlayerHorizontalMovement>().velocity = newVelocityForMount;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            if ((other.tag == "Player" && !PlayerPowers.startTimerToDisablePowerToFly) || other.tag == "Mount")
            {
                other.GetComponent<PlayerJump>().activateDoubleJump = other.GetComponent<PlayerJump>().originalActivateDoubleJump;
                other.GetComponent<PlayerJump>().noLimitOfJumps = other.GetComponent<PlayerJump>().originalNoLimitOfJumps;
            }

            other.GetComponent<Rigidbody2D>().gravityScale = other.GetComponent<PlayerHorizontalMovement>().originalRigidbodyGravity;
            other.GetComponent<Rigidbody2D>().drag = other.GetComponent<PlayerHorizontalMovement>().originalRigidbodyLinearDrag;
            other.GetComponent<PlayerHorizontalMovement>().velocity = other.GetComponent<PlayerHorizontalMovement>().originalVelocity;

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.IsUnderWater, false);
        }
    }
}
