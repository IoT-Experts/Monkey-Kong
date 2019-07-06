using UnityEngine;
using System.Collections;
using EkumeEnumerations;

public class MoveObjectWithKey_OnTrigger : MonoBehaviour
{
    public InputControlsManager inputControlsManager;
    public int inputControl;

    public float velocityToMove;
    public Transform newPosition;

    public Transform objectToMove;
    public bool canReturnToPos1;

    Vector3 position1;
    bool startMovement;
    bool goingToPos1;
    bool goingToPos2 = true;
    bool canPressKey;

    void Start ()
    {
        position1 = objectToMove.transform.position;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            canPressKey = true;
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            canPressKey = false;
        }
    }

    void Update ()
    {
        if (canPressKey)
        {
            if (InputControls.GetControlDown(inputControl))
            {
                startMovement = true;

                if (canReturnToPos1)
                {
                    if (goingToPos1)
                    {
                        goingToPos1 = false;
                        goingToPos2 = true;
                    }
                    else if (goingToPos2)
                    {
                        goingToPos2 = false;
                        goingToPos1 = true;
                    }
                }
                else
                {
                    goingToPos1 = true;
                    goingToPos2 = false;
                }

            }
        }

        if(startMovement)
        {
            if (goingToPos1)
            {
                objectToMove.position = Vector3.MoveTowards(objectToMove.transform.position, newPosition.position, Time.deltaTime * velocityToMove);

                if (Vector2.Distance(objectToMove.position, newPosition.position) < 0.05f)
                {
                    startMovement = false;
                }

            }
            else if(goingToPos2)
            {
                objectToMove.position = Vector3.MoveTowards(objectToMove.transform.position, position1, Time.deltaTime * velocityToMove);

                if (Vector2.Distance(objectToMove.position, position1) < 0.05f)
                {
                    startMovement = false;
                }

            }
        }
    }
}
