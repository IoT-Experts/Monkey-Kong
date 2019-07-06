using UnityEngine;
using System.Collections;

public class SlipperyPlatformForPlayer : MonoBehaviour
{
    [Header("The new values for the PlayerHorizontalMovement. (Sliding simulation)")]
    [Header("[Remember to add a Material2D with low friction to the collider]")]
    [SerializeField] float speedToReduceVelocity = 0.1f;
    [SerializeField] float speedToIncreaseVelocity = 0.15f;
    [Header("When the player go out of the platform")]
    [SerializeField] float speedToRecoverVelocities = 0.5f;
    [SerializeField] float timeToWaitToRecoverVelocities = 0.5f;

    bool startToRecoverOriginalVelocities = false;
    static float timerForWaitToRecoverVelocities;

    PlayerHorizontalMovement playerMovementInX;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            playerMovementInX = other.collider.GetComponent<PlayerHorizontalMovement>();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            playerMovementInX.constantReductionOfVelocity = true;
            playerMovementInX.gradualVelocity = true;
            playerMovementInX.speedToReduceVelocity = speedToReduceVelocity;
            playerMovementInX.speedToIncreaseVelocity = speedToIncreaseVelocity;

            startToRecoverOriginalVelocities = false;
            timerForWaitToRecoverVelocities = 0;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            playerMovementInX.constantReductionOfVelocity = playerMovementInX.originalConstantReductionOfVelocity;
            playerMovementInX.gradualVelocity = playerMovementInX.originalGradualVelocity;

            if (playerMovementInX.originalGradualVelocity || playerMovementInX.originalConstantReductionOfVelocity)
                startToRecoverOriginalVelocities = true;
        }
    }

    void Update()
    {
        if (startToRecoverOriginalVelocities)
        {
            timerForWaitToRecoverVelocities += Time.deltaTime;
            if (timerForWaitToRecoverVelocities >= timeToWaitToRecoverVelocities)
            {
                playerMovementInX.speedToReduceVelocity = Mathf.SmoothStep(playerMovementInX.speedToReduceVelocity, playerMovementInX.originalSpeedToReduceVelocity, Time.deltaTime * speedToRecoverVelocities);
                playerMovementInX.speedToIncreaseVelocity = Mathf.SmoothStep(playerMovementInX.speedToIncreaseVelocity, playerMovementInX.originalSpeedToIncreaseVelocity, Time.deltaTime * speedToRecoverVelocities);

                if (playerMovementInX.speedToIncreaseVelocity == playerMovementInX.originalSpeedToIncreaseVelocity && playerMovementInX.speedToReduceVelocity == playerMovementInX.originalSpeedToReduceVelocity)
                {
                    startToRecoverOriginalVelocities = false;
                }
            }
        }
    }
}
