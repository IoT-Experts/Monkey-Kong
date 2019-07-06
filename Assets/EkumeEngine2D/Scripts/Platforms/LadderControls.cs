using UnityEngine;
using System.Collections;
using EkumeEnumerations;

public class LadderControls : MonoBehaviour
{

    [HideInInspector] public bool enableControlsForSides;
    [HideInInspector] public float velocityToGoUp;
    [HideInInspector] public float velocityToGoDown;
    [HideInInspector] public float velocityForSides;
    [HideInInspector] public int controlToGoUp;
    [HideInInspector] public int controlToGoDown;
    [HideInInspector] public int controlToGoLeft;
    [HideInInspector] public int controlToGoRight;    

    void Update ()
    {
        if(InputControls.GetControl(controlToGoDown))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (velocityToGoDown * Time.deltaTime), transform.position.z);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToDown, true);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYNegative, true);
        }
        else
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToDown, false);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYNegative, false);
        }

        if (InputControls.GetControl(controlToGoUp))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (velocityToGoUp * Time.deltaTime), transform.position.z);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToUp, true);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYPositive, true);
        }
        else
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToUp, false);
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYPositive, false);
        }

        if (enableControlsForSides)
        {
            if (InputControls.GetControl(controlToGoRight))
            {
                transform.position = new Vector3(transform.position.x + (velocityForSides * Time.deltaTime), transform.position.y, transform.position.z);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToRight, true);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, true);
            }
            else if(InputControls.GetControlUp(controlToGoRight))
            {
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToRight, false);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, false);
            }

            if (InputControls.GetControl(controlToGoLeft))
            {
                transform.position = new Vector3(transform.position.x - (velocityForSides * Time.deltaTime), transform.position.y, transform.position.z);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToLeft, true);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, true);
            }
            else if (InputControls.GetControlUp(controlToGoLeft))
            {
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadderToLeft, false);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, false);
            }
        }

        if(InputControls.GetControl(controlToGoDown) || InputControls.GetControl(controlToGoUp)
        || ((enableControlsForSides) ? InputControls.GetControl(controlToGoLeft) || InputControls.GetControl(controlToGoRight) : false))
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadder, true);
        else
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerMovesInLadder, false);
    }
}
