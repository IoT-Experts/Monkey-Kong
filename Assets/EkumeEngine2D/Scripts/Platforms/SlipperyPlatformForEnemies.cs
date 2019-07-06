using UnityEngine;
using System.Collections;

public class SlipperyPlatformForEnemies : MonoBehaviour
{

    [Header("[Remember to add a Material2D with low friction to the collider]")]
    [Header("The new values for the IAEnemyMovement. (Sliding simulation)")]
    [SerializeField] float speedToReduceVelocity = 0.1f;
    [SerializeField] float speedToIncreaseVelocity = 5f;
    [Header("When the player exit of the platform")]
    [SerializeField] float speedToRecoverVelocities = 0.5f;
    [SerializeField] float timeToWaitToRecoverVelocities = 0.5f;

    bool startToRecoverOriginalVelocities = false;
    static float timerForWaitToRecoverVelocities;

    AIEnemyMovement enemyMovement;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<AIEnemyMovement>() != null)
        {
            enemyMovement = other.collider.GetComponent<AIEnemyMovement>();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.GetComponent<AIEnemyMovement>() != null)
        {
            enemyMovement.constantReductionOfVelocity = true;
            enemyMovement.gradualVelocity = true;
            enemyMovement.speedToReduceVelocity = speedToReduceVelocity;
            enemyMovement.speedToIncreaseVelocity = speedToIncreaseVelocity;

            startToRecoverOriginalVelocities = false;
            timerForWaitToRecoverVelocities = 0;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<AIEnemyMovement>() != null)
        {
            enemyMovement.constantReductionOfVelocity = enemyMovement.originalConstantReductionOfVelocity;
            enemyMovement.gradualVelocity = enemyMovement.originalGradualVelocity;

            if (enemyMovement.originalGradualVelocity || enemyMovement.originalConstantReductionOfVelocity)
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
                enemyMovement.speedToReduceVelocity = Mathf.SmoothStep(enemyMovement.speedToReduceVelocity, enemyMovement.originalSpeedToReduceVelocity, Time.deltaTime * speedToRecoverVelocities);
                enemyMovement.speedToIncreaseVelocity = Mathf.SmoothStep(enemyMovement.speedToIncreaseVelocity, enemyMovement.originalSpeedToIncreaseVelocity, Time.deltaTime * speedToRecoverVelocities);

                if (enemyMovement.speedToIncreaseVelocity == enemyMovement.originalSpeedToIncreaseVelocity && enemyMovement.speedToReduceVelocity == enemyMovement.originalSpeedToReduceVelocity)
                {
                    startToRecoverOriginalVelocities = false;
                }
            }
        }
    }
}
