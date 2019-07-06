using UnityEngine;
using System.Collections;
using EkumeLists;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class ParkourWall : MonoBehaviour
{
    [SerializeField] float forceToAddInX;
    [SerializeField] float forceToAddInY;

    [SerializeField] ListOfInputControls inputControlOfJump;

    [SerializeField] float velocityToSlideDown;

    bool isInCollider;
    float timerToReactivateJump;
    bool startTimer;
    bool canJump;

    float timerToWaitToSlideDown;
    bool startTimerToWait;
    bool canSlideDown;

    PlayerJump playerJump;
    PlayerHorizontalMovement playerMovement;
    Rigidbody2D playerRB;

    void Start()
    {
        if (Player.instance.GetComponent<PlayerHorizontalMovement>() != null)
            playerMovement = Player.instance.GetComponent<PlayerHorizontalMovement>();

        playerRB = Player.instance.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            if (Player.instance.GetComponent<PlayerJump>() != null)
                playerJump = other.collider.GetComponent<PlayerJump>();

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInParkourWall, true);

            isInCollider = true;
            startTimer = true;
            canJump = true;
            startTimerToWait = true;
            timerToWaitToSlideDown = 0;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInParkourWall, false);

            isInCollider = false;
            playerJump = null;
            canSlideDown = false;
            startTimerToWait = false;
        }
    }

    void Update()
    {
        if (startTimerToWait)
        {
            timerToWaitToSlideDown += Time.deltaTime;

            if (timerToWaitToSlideDown >= 0.05)
            {
                canSlideDown = true;
            }
        }

        if (startTimer)
        {
            timerToReactivateJump += Time.deltaTime;

            if (timerToReactivateJump > 0.5)
            {
                canJump = true;
                startTimer = false;
                timerToReactivateJump = 0;
            }
        }

        if (isInCollider)
        {
            if (canSlideDown)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, -velocityToSlideDown);
            }

            if (canJump)
            {
                playerJump.isGrounded = true;
                playerJump.isInDoubleJump = false;

                if (InputControls.GetControlDown(inputControlOfJump.Index))
                {
                    startTimerToWait = true;
                    canSlideDown = false;
                    timerToWaitToSlideDown = 0;

                    if (playerMovement != null)
                    {
                        playerMovement.enabled = false;
                        StartCoroutine(EnablePlayerMovement(playerJump));
                    }

                   playerRB.velocity = new Vector2(forceToAddInX, forceToAddInY);

                    canJump = false;
                }
                startTimer = true;
            }
        }
    }

    IEnumerator EnablePlayerMovement(PlayerJump player)
    {
        yield return new WaitForSeconds(0.1f);
           playerMovement.enabled = true;
     }
}
