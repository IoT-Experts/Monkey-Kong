using UnityEngine;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class PlatformOfVelocity : MonoBehaviour
{
    [Header("Change the velocity of the Player when collides")]
    [SerializeField] DirectionsXAxisEnum sideToMove;

    [SerializeField] float velocityToAdd;
    [SerializeField] float retentionForTheOtheSide;

    [SerializeField] float velocityToAddIfCrouch;

    PlayerHorizontalMovement playerMovementInX;
    Rigidbody2D rigidbody2DCollided;

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            rigidbody2DCollided = other.collider.GetComponent<Rigidbody2D>();
            playerMovementInX = other.collider.GetComponent<PlayerHorizontalMovement>();
        }

        UpdateMovement(other);
    }

    void UpdateMovement (Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            playerMovementInX.constantReductionOfVelocity = false;
            playerMovementInX.gradualVelocity = false;

            if (sideToMove == DirectionsXAxisEnum.Right)
            {
                if (rigidbody2DCollided.velocity.x < 0)
                {
                    playerMovementInX.velocity = playerMovementInX.originalVelocity - retentionForTheOtheSide;
                }
                else
                {
                    rigidbody2DCollided.velocity = transform.right * (playerMovementInX.originalVelocity * velocityToAdd);
                }
            }
            else if (sideToMove == DirectionsXAxisEnum.Left)
            {
                if (rigidbody2DCollided.velocity.x > 0)
                {
                    playerMovementInX.velocity = playerMovementInX.originalVelocity - retentionForTheOtheSide;
                }
                else
                {
                    rigidbody2DCollided.velocity = -transform.right * (playerMovementInX.originalVelocity * velocityToAdd);
                }
            }

            //If the player or the mount is crouched down
            if ((other.collider.tag == "Player") ? PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsCrouchedDown)
                : PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsCrouchedDown))
            {
                other.collider.GetComponent<Rigidbody2D>().velocity =
                    ((sideToMove == DirectionsXAxisEnum.Right) ? transform.right :
                    -transform.right) * (other.collider.GetComponent<PlayerHorizontalMovement>().originalVelocity + velocityToAddIfCrouch);
            }
        }

    }
    void OnCollisionStay2D(Collision2D other)
    {
        UpdateMovement(other);
    }

    void OnCollisionExit2D (Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            playerMovementInX.velocity = playerMovementInX.originalVelocity;
            playerMovementInX.constantReductionOfVelocity = playerMovementInX.originalConstantReductionOfVelocity;
            playerMovementInX.gradualVelocity = playerMovementInX.originalGradualVelocity;

            other.collider.GetComponent<PlayerHorizontalMovement>().startToReduceVelocityInX = true;
        //    other.collider.GetComponent<PlayerHorizontalMovement>().velocityAddedInCrouchDown = true;
        }
    }
}
