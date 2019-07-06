using UnityEngine;
using EkumeEnumerations;

public class Mount : MonoBehaviour
{
    Rigidbody2D thisRigidBody;

    void Awake ()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();        
    }

    void Update ()
    {
        if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == this.gameObject)
        {
            if (thisRigidBody.velocity.x >= 0.05f || thisRigidBody.velocity.x <= -0.05f)
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, true);
            else
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsMovingInXAxis, false);

            if (thisRigidBody.velocity.y > 0)
            {
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYPositive, true);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYNegative, false);
            }
            else if (thisRigidBody.velocity.y < 0)
            {
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYNegative, true);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYPositive, false);
            }
            else
            {
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYNegative, false);
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerVelocityYPositive, false);
            }
        }
    }

}
