using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class RideTheMount : MonoBehaviour
{
    public enum HowToRide { JumpOnTop, WhenEnterInTrigger, PressingInput }

    public HowToRide howToRide;
    public int inputControlToRide;
    public bool disableWeaponOfPlayer;

    GameObject mountParent;

    bool isInTrigger = false;

    void Awake ()
    {
        mountParent = transform.root.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (howToRide == HowToRide.PressingInput)
            {
                isInTrigger = false;
            }
        }
    }

    void Update ()
    {
        if (isInTrigger && InputControls.GetControlDown(inputControlToRide))
            RideInMount();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (howToRide == HowToRide.PressingInput)
            {
                isInTrigger = true;
            }

            if ((howToRide == HowToRide.JumpOnTop && Player.instance.GetComponent<Rigidbody2D>().velocity.y < 0.1 && Player.instance.GetComponent<Rigidbody2D>().velocity.y != 0)
                || howToRide == HowToRide.WhenEnterInTrigger)
            {
                RideInMount();
            }
        }
    }


    void RideInMount()
    {
        if (DismountOfPlayer.ridingActivated)
        {
            if (mountParent.GetComponent<MountLifeManager>() != null && mountParent.GetComponent<MountLifeManager>().mountUI != null)
                mountParent.GetComponent<MountLifeManager>().mountUI.SetActive(true);

            Player.playerTransform.position = new Vector3(transform.position.x, transform.position.y, Player.playerTransform.position.z); //Transport the player to the position of the mount

            Player.playerTransform.SetParent(this.transform); //Makes child of the mount

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount, true); //Send the event of "mount" to the player

            if (mountParent.GetComponent<PlayerHorizontalMovement>() != null)
            {
                if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft)
                   && mountParent.GetComponent<PlayerHorizontalMovement>().currentDirection == DirectionsXAxisEnum.Right) //If the current direction of the player is left and the current direction of the mount is right
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); //Turn the player

                    //Send the new values of the events
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft, false);
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight, true);
                }
                else if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight)
                     && mountParent.GetComponent<PlayerHorizontalMovement>().currentDirection == DirectionsXAxisEnum.Left) //If the current direction of the player is right and the current direction of the mount is left
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); //Turn the player

                    //Send the new values of the events
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft, true);
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight, false);
                }
            }

            //Enable the mount scripts
            if (mountParent.GetComponent<PlayerJump>() != null)
                mountParent.GetComponent<PlayerJump>().enabled = true;

            if (mountParent.GetComponent<PlayerHorizontalMovement>() != null)
                mountParent.GetComponent<PlayerHorizontalMovement>().enabled = true;

            if (mountParent.GetComponent<MountLifeManager>() != null)
                mountParent.GetComponent<MountLifeManager>().enabled = true;

            if (mountParent.GetComponent<UseWeapon>() != null)
                mountParent.GetComponent<UseWeapon>().enabled = true;

            if (mountParent.GetComponent<JetpackPower>() != null)
                mountParent.GetComponent<JetpackPower>().enabled = true;

            if (mountParent.GetComponent<AnimatorManagerOfPlayer>() != null)
                mountParent.GetComponent<AnimatorManagerOfPlayer>().enabled = true;

            Player.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Player.instance.GetComponent<Rigidbody2D>().isKinematic = true;
            //--------------------------

            //Disable the player scripts
            if (Player.instance.GetComponent<PlayerHorizontalMovement>() != null)
                Player.instance.GetComponent<PlayerHorizontalMovement>().enabled = false;

            if (Player.instance.GetComponent<PlayerJump>() != null)
                Player.instance.GetComponent<PlayerJump>().enabled = false;

            if (Player.instance.GetComponent<PlayerLifeManager>() != null)
                Player.instance.GetComponent<PlayerLifeManager>().enabled = false;

            foreach (Collider2D collider2d in Player.playerTransform.GetComponents<Collider2D>())
            {
                collider2d.enabled = false;
            }

            if (disableWeaponOfPlayer)
            {
                if (Player.instance.GetComponent<UseWeapon>() != null)
                    Player.instance.GetComponent<UseWeapon>().enabled = false;
            }
            //--------------------------

            DismountOfPlayer.currentMount = mountParent;
            DismountOfPlayer.playerPositionInMountObj = gameObject;
            PlayerStates.SetStateValueForAllAttackCategories(false);
        }
    }

}
