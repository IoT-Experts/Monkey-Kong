using UnityEngine;
using System.Collections;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class Ladders : MonoBehaviour
{
    public bool enableControlsForSides;
    public float velocityToGoUp;
    public float velocityToGoDown;
    public float velocityForSides;
    public int controlToGoUp;
    public int controlToGoDown;
    public int controlToGoLeft;
    public int controlToGoRight;
    public bool centerCharacterInLadder;

    bool isInTrigger;
    GameObject player;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" && !PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount))
        {
            player = other.gameObject;
            isInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount))
        {
            isInTrigger = false;
            GetOutOfLadder();
        }
    }

    void SetIntoLadder ()
    {
        player.AddComponent<LadderControls>();
        player.GetComponent<PlayerHorizontalMovement>().enabled = false;
        SetValuesOfVariables(player.GetComponent<LadderControls>());
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerInLadder, true);
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, false);
    }

    void GetOutOfLadder ()
    {
        Destroy(player.GetComponent<LadderControls>());
        player.GetComponent<PlayerHorizontalMovement>().enabled = true;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerInLadder, false);
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadder, false);
    }

    void SetValuesOfVariables (LadderControls controls)
    {
        controls.enableControlsForSides = enableControlsForSides;
        controls.velocityToGoUp = velocityToGoUp;
        controls.velocityToGoDown = velocityToGoDown;
        controls.velocityForSides = velocityForSides;
        controls.controlToGoDown = controlToGoDown;
        controls.controlToGoUp = controlToGoUp;
        controls.controlToGoRight = controlToGoRight;
        controls.controlToGoLeft = controlToGoLeft;
    }

    void Update ()
    {        
        if(isInTrigger)
        {
            if (InputControls.GetControlDown(controlToGoUp) || InputControls.GetControlDown(controlToGoDown))
            {
                if (player.GetComponent<LadderControls>() == null)
                {
                    SetIntoLadder();
                }
            }

            if((InputControls.GetControlDown(controlToGoLeft) || InputControls.GetControlDown(controlToGoRight)) 
                && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsGrounded) 
                && player.GetComponent<LadderControls>() != null)
            {
                GetOutOfLadder();
            }

            if(centerCharacterInLadder && player.GetComponent<LadderControls>() != null && (InputControls.GetControl(controlToGoUp) || InputControls.GetControl(controlToGoDown)))
            {
                player.transform.position = new Vector3(Mathf.MoveTowards(player.transform.position.x, transform.position.x, Time.deltaTime * 10), player.transform.position.y, player.transform.position.z);
            }
        }
        else if(player != null)
        {
            if (player.GetComponent<LadderControls>() != null)
            {
                GetOutOfLadder();
                player = null;
            }
        }
    }
}
