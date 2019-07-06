using UnityEngine;
using System.Collections;
using EkumeEnumerations;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Rigidbody2D))]
public class AIFlyingEnemy : MonoBehaviour
{

    public enum MovementType { Lerp, MoveTowards }
    public MovementType movementType;

    public bool followInX;
    public float xSeparation;
    public bool followInY;
    public float ySeparation;
    public bool invertXSeparation; //Invert the separation depending of the target direction
    public enum FollowType { FollowAlwaysIfDetects, FollowOnlyIfIsInDetectionZone }
    public DirectionsXAxisEnum currentDirection; //In which direction is watching the enemy?
    public float velocityOfMovement; //Movement velocity
    public TargetDetector targetDetector;
    public FollowType trackingType;

    Transform targetToTrack;
    bool startTracking;
    Enemy eventDetection;

    [HideInInspector] public float originalVelocity;

    void Awake()
    {
        originalVelocity = velocityOfMovement;

        eventDetection = GetComponent<Enemy>();

        if (targetDetector == null)
        {
            Debug.LogError("The GameObject " + gameObject.name + " does not have a TargetDetector in the component AIFlyingEnemy");
        }
    }

    void Start()
    {
        if (currentDirection == DirectionsXAxisEnum.Right)
            eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
        else if (currentDirection == DirectionsXAxisEnum.Left)
            eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
    }

    void MoveEnemyToTarget(float separationInX, float separationInY)
    {
        if (movementType == MovementType.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                  (followInX) ? targetToTrack.position.x + separationInX : transform.position.x,
                  (followInY) ? targetToTrack.position.y + separationInY : transform.position.y,
                  transform.position.z), Time.deltaTime * velocityOfMovement);
        }
        else if (movementType == MovementType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(
                (followInX) ? targetToTrack.position.x + separationInX : transform.position.x,
                (followInY) ? targetToTrack.position.y + separationInY : transform.position.y,
                transform.position.z), Time.deltaTime * velocityOfMovement);
        }
    }

    void Update ()
    {
        if (targetToTrack == null)
            targetToTrack = targetDetector.locatedTarget;

        if (targetToTrack != null)
        {
            startTracking = true;
            if (targetDetector.locatedTarget == null && trackingType == FollowType.FollowOnlyIfIsInDetectionZone)
                startTracking = false;
        }
        else
        {
            startTracking = false;
            
        }

        if (startTracking)
        {
            //MOVEMENT
            eventDetection.SetStateValue(EnemyStatesEnum.EnemyIsMoving, true);

            float _separationInX = 0;
            if (invertXSeparation)
            {
                if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft))
                {
                    if (followInX) _separationInX = xSeparation;
                }
                else if ((PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight)))
                {
                    if (followInX) _separationInX = -xSeparation;
                }
            }

            if (targetToTrack.position.x < this.transform.position.x)
            {

                MoveEnemyToTarget((invertXSeparation) ? _separationInX : xSeparation, ySeparation);

                if (!eventDetection.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

                    if (transform.FindChild("UIOfLifeOfMonster") != null)
                    {
                        transform.FindChild("UIOfLifeOfMonster").transform.localScale = new Vector3(transform.FindChild("UIOfLifeOfMonster").transform.localScale.x * -1, transform.FindChild("UIOfLifeOfMonster").transform.localScale.y, transform.FindChild("UIOfLifeOfMonster").transform.localScale.z);
                    }
                }

                eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
                eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, false);

            }
            else if (targetToTrack.position.x > this.transform.position.x)
            {
                MoveEnemyToTarget((invertXSeparation) ? _separationInX : xSeparation, ySeparation);

                if (!eventDetection.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

                    if (transform.FindChild("UIOfLifeOfMonster") != null)
                    {
                        transform.FindChild("UIOfLifeOfMonster").transform.localScale = new Vector3(transform.FindChild("UIOfLifeOfMonster").transform.localScale.x * -1, transform.FindChild("UIOfLifeOfMonster").transform.localScale.y, transform.FindChild("UIOfLifeOfMonster").transform.localScale.z);
                    }
                }

                eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
                eventDetection.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, false);
            }
        }
        else
        {
            eventDetection.SetStateValue(EnemyStatesEnum.EnemyIsMoving, false);
        }
    }
}
