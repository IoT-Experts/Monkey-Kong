using UnityEngine;
using System.Collections;
using EkumeEnumerations;

public class LookAtTarget : MonoBehaviour
{
    
    [SerializeField] enum ObjectType { OtherObject, ObjectOfPlayer, ObjectOfEnemy }
    [SerializeField] ObjectType whatTypeOfObjectIsThis;
    [SerializeField] DirectionsXAxisEnum currentDirection;
    [SerializeField] TargetDetector targetDetector;

    [Header("Fill only if the type is ObjectOfEnemy")]
    [SerializeField] Enemy enemyEvents;
    [Space]
    [SerializeField] float velocityToLookAt;

    Vector3 originalRotation;

    float angleToLook;

    int frames; //Used to reduce the frames to call the angle calculator

    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

	void FixedUpdate ()
    {
	    if (targetDetector.locatedTarget != null)
        {
            frames++;
            if (frames > 5)
            {
                angleToLook = AngleCalculator();
                frames = 0;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angleToLook, Vector3.forward), Time.deltaTime * velocityToLookAt);
        }
        else
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, originalRotation, Time.deltaTime * 10);
        }
	}

    float AngleCalculator ()
    {
        float angle = 0;
        Vector3 dir = targetDetector.locatedTarget.position - transform.position;

        if (whatTypeOfObjectIsThis == ObjectType.OtherObject)
        {
            if (currentDirection == DirectionsXAxisEnum.Left)
                angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            else
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        else if (whatTypeOfObjectIsThis == ObjectType.ObjectOfEnemy)
        {
            if (enemyEvents.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
            {
                angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            }
            else if (enemyEvents.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
            {
                angle = Mathf.Atan2(-dir.y, dir.x) * Mathf.Rad2Deg;
            }

        }
        else if (whatTypeOfObjectIsThis == ObjectType.ObjectOfPlayer)
        {
            if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft) && targetDetector.locatedTarget.position.x < transform.position.x)
                angle = Mathf.Atan2(-dir.y, -dir.x) * -Mathf.Rad2Deg;
            else if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight) && targetDetector.locatedTarget.position.x > transform.position.x)
                angle = Mathf.Atan2(-dir.y, dir.x) * -Mathf.Rad2Deg;
        }

        return angle;
    }
}
