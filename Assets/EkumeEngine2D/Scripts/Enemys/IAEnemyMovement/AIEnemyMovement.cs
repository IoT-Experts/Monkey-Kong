using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof(Enemy))]
public class AIEnemyMovement : MonoBehaviour
{
    public enum FollowType { FollowAlwaysIfDetects, FollowOnlyIfIsInDetectionZone }

    public DirectionsXAxisEnum currentDirection; //In which direction is watching the enemy?
    public float movementSpeed; //Movement velocity
    public float movementSpeedToFollow;
    public bool constantMovement;
    public bool constantReductionOfVelocity;
    public float speedToReduceVelocity;
    public bool gradualVelocity;
    public float speedToIncreaseVelocity;
    public bool jumpInLimit;
    public float jumpForce;
    public Transform groundChecker;
    public bool followTarget;
    public TargetDetector targetDetector;
    public float distanceOfTarget; //Distance to keep of the target
    public FollowType followType;
    //What to do when find limit to walk
    public bool invertVelocityInLimit;
    public bool detectsGroundInFront;
    public Transform frontGroundDetector; //It will jump when detects obstacle
    public bool detectsWallInFront;
    Transform targetToTrack;
    bool startTracking;
    Rigidbody2D thisRigidbody;
    bool isGrounded;
    Enemy enemyStates;
    float enemySpeed;

    //-------------
    //Reduction of velocity
    public bool startToReduceVelocityInX;
    public float velocityTimeOfReduction = 0;

    //Gradual increase of velocity
    float timerForGradualVelocity;
    //----------------------------------------------------------------------------------------------------------- /

    //Save the original values of the variables (Made because some platforms change the values)

    [HideInInspector] public float originalVelocityOfMovement;
    [HideInInspector] public float originalSpeedToReduceVelocity;
    [HideInInspector] public float originalSpeedToIncreaseVelocity;
    [HideInInspector] public bool originalConstantReductionOfVelocity;
    [HideInInspector] public bool originalGradualVelocity;
    [HideInInspector] public float originalSpeedToFollow;

    //----------------------------------------------------------------------------------------------------------- /

    void Awake()
    {
        originalSpeedToFollow = movementSpeedToFollow;
        thisRigidbody = GetComponent<Rigidbody2D>();
        enemyStates = GetComponent<Enemy>();
    }

    void Start ()
    {
        if (currentDirection == DirectionsXAxisEnum.Right)
            enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
        else if (currentDirection == DirectionsXAxisEnum.Left)
            enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
    }

    public void ObstacleDetection(Collider2D other) //This function is called from the script TargetDetector in the child TargetDetector
    {
        if (detectsWallInFront)
        {
            if (jumpInLimit && (!invertVelocityInLimit || (invertVelocityInLimit && startTracking)))
            {
                if (startTracking)
                {
                    if (isGrounded)
                    {
                        if (targetToTrack.position.y > other.transform.position.y || this.transform.position.y > targetToTrack.transform.position.y + 1.1f)
                        {
                            Jump();
                        }
                    }
                }
                else if (constantMovement && !startTracking && jumpInLimit && detectsGroundInFront && isGrounded)
                {
                    Jump();
                }
            }
            else if (invertVelocityInLimit)
            {
                if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
                {
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, false);
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
                    InvertCharacter();
                }
                else if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
                {
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, false);
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
                    InvertCharacter();
                }
            }
        }
    }

    void InvertCharacter ()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        if (gradualVelocity)
            timerForGradualVelocity = 0;

        if (transform.FindChild("UIOfLifeOfMonster") != null)
        {
            transform.FindChild("UIOfLifeOfMonster").transform.localScale = new Vector3(transform.FindChild("UIOfLifeOfMonster").transform.localScale.x * -1, transform.FindChild("UIOfLifeOfMonster").transform.localScale.y, transform.FindChild("UIOfLifeOfMonster").transform.localScale.z);
        }
    }

    int frames = 0;
    void Update()
    {
        if (startToReduceVelocityInX)
            VelocityReductionInAxisX();

        if (thisRigidbody.velocity.x >= 0.01f || thisRigidbody.velocity.x <= -0.01f)
        {
            enemyStates.SetStateValue(EnemyStatesEnum.EnemyIsMoving, true);
        }
        else
        {
            enemyStates.SetStateValue(EnemyStatesEnum.EnemyIsMoving, false);
            if (gradualVelocity)
                timerForGradualVelocity = 0;
        }

        //Invert direction if does not find ground
        if (invertVelocityInLimit && detectsGroundInFront)
        {
            bool groundInFront = Physics2D.OverlapCircle(frontGroundDetector.position, 0.32f, 1 << LayerMask.NameToLayer("Ground"));

            if (!groundInFront && isGrounded)
            {
                if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
                {
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, false);
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
                    InvertCharacter();
                }
                else if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
                {
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, false);
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
                    InvertCharacter();
                }
            }
        }

        if (constantMovement)
        {
            if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
            {
                ChangeVelocityInX(-enemySpeed);
            }
            else if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
            {
                ChangeVelocityInX(enemySpeed);
            }
        }

        frames++;
        if (frames > 5)
        {
            frames = 0;

            if (!startTracking)
                enemySpeed = movementSpeed;
            else
                enemySpeed = movementSpeedToFollow;

            if (followTarget)
            {
                if (targetToTrack == null)
                    targetToTrack = targetDetector.locatedTarget;

                if (targetToTrack != null)
                {
                    startTracking = true;
                    if (targetDetector.locatedTarget == null && followType == FollowType.FollowOnlyIfIsInDetectionZone)
                        startTracking = false;
                }
                else
                {
                    startTracking = false;
                }

                if (startTracking)
                {
                    //MOVEMENT
                    if (targetToTrack.position.x < this.transform.position.x)
                    {

                        if ((this.transform.position.x - targetToTrack.position.x) > distanceOfTarget)
                        {
                            startToReduceVelocityInX = false;
                            ChangeVelocityInX(-enemySpeed);
                        }
                        else
                        {
                            if (constantReductionOfVelocity)
                            {
                                velocityTimeOfReduction = 0;
                                startToReduceVelocityInX = true;
                            }
                            else
                            {
                                //If the variable "constantReductionOfVelocity" is false, the velocity of the object will be 0 if the key "Up" is raised
                                thisRigidbody.velocity = (new Vector2(0, thisRigidbody.velocity.y));
                            }
                        }

                        if (!enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
                        {
                            InvertCharacter();
                        }

                        enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, true);
                        enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, false);

                    }
                    else if (targetToTrack.position.x > this.transform.position.x)
                    {

                        if ((targetToTrack.position.x - this.transform.position.x) > distanceOfTarget)
                        {
                            startToReduceVelocityInX = false;
                            ChangeVelocityInX(enemySpeed);
                        }
                        else
                        {
                            if (constantReductionOfVelocity)
                            {
                                velocityTimeOfReduction = 0;
                                startToReduceVelocityInX = true;
                            }
                            else
                            {
                                //If the variable "constantReductionOfVelocity" is false, the velocity of the object will be 0 if the key "Up" is raised
                                thisRigidbody.velocity = (new Vector2(0, thisRigidbody.velocity.y));
                            }
                        }

                        if (!enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
                        {
                            InvertCharacter();
                        }

                        enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsRight, true);
                        enemyStates.SetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft, false);
                    }
                }
            }


            if (groundChecker != null)
            {
                isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.32f, 1 << LayerMask.NameToLayer("Ground")); //Jump detection

                if (isGrounded)
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyIsGrounded, true);
                else
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyIsGrounded, false);

                if (jumpInLimit && (!invertVelocityInLimit || (invertVelocityInLimit && startTracking)))
                {
                    if (detectsGroundInFront && isGrounded)
                    {
                        bool groundInFrontOrDown = Physics2D.Linecast(frontGroundDetector.position, new Vector2(frontGroundDetector.position.x, -150), 1 << LayerMask.NameToLayer("Ground"));

                        if (!groundInFrontOrDown)
                        {
                            Jump();
                        }
                    }
                }
            }
        }

    }

    void Jump()
    {
        thisRigidbody.velocity = new Vector2(thisRigidbody.velocity.x, jumpForce);
    }

    // Constant reduction of velocity
    void VelocityReductionInAxisX()
    {
        velocityTimeOfReduction += Time.deltaTime * speedToReduceVelocity;
        float lastVelocityY = thisRigidbody.velocity.y;
        thisRigidbody.velocity = Vector2.Lerp(thisRigidbody.velocity, Vector2.zero, velocityTimeOfReduction);
        thisRigidbody.velocity = new Vector2(thisRigidbody.velocity.x, lastVelocityY);

        //If the velocity is 0 in X then stop the reduction of the velocity.
        if (thisRigidbody.velocity.x == 0)
        {
            startToReduceVelocityInX = false;
        }
    }

    void ChangeVelocityInX(float newVelocityX)
    {
        if (!gradualVelocity)
        {
            thisRigidbody.velocity = new Vector2(newVelocityX, thisRigidbody.velocity.y);
        }
        else
        {
            timerForGradualVelocity += Time.deltaTime * speedToIncreaseVelocity;
            thisRigidbody.velocity = new Vector2(Mathf.Lerp(thisRigidbody.velocity.x, newVelocityX, timerForGradualVelocity), thisRigidbody.velocity.y);
        }
    }
}
