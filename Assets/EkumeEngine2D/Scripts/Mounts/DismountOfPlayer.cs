using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DismountOfPlayer : MonoBehaviour
{
    //How to dismount the player of the mount
    public int inputToDismount;
    public float timeToReactivateMount;

    public static GameObject currentMount;
    public static GameObject playerPositionInMountObj;

    float timerToReactivateMount;
    static bool startTimerToReactivate;
    public static bool ridingActivated;
    
    void Start ()
    {
        ridingActivated = true;
    }

    public static void DismountPlayer()
    {
        PlayerStates.SetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsRidingAMount, false);

        if (currentMount.GetComponent<MountLifeManager>() != null && currentMount.GetComponent<MountLifeManager>().mountUI != null)
            currentMount.GetComponent<MountLifeManager>().mountUI.SetActive(false);

        //Disable the mount scripts
        if (currentMount.GetComponent<PlayerJump>() != null)
            currentMount.GetComponent<PlayerJump>().enabled = false;

        if(currentMount.GetComponent<PlayerHorizontalMovement>() != null)
            currentMount.GetComponent<PlayerHorizontalMovement>().enabled = false;
        
        if (currentMount.GetComponent<MountLifeManager>() != null)
            currentMount.GetComponent<MountLifeManager>().enabled = false;

        if (currentMount.GetComponent<UseWeapon>() != null)
            currentMount.GetComponent<UseWeapon>().enabled = false;

        if (currentMount.GetComponent<JetpackPower>() != null)
            currentMount.GetComponent<JetpackPower>().enabled = false;

        if (currentMount.GetComponent<AnimatorManagerOfPlayer>() != null)
            currentMount.GetComponent<AnimatorManagerOfPlayer>().enabled = false;
        //--------------------------

        //Enable the player scripts
        Player.playerTransform.GetComponent<Rigidbody2D>().isKinematic = false;

        if (Player.playerTransform.GetComponent<PlayerHorizontalMovement>() != null)
            Player.playerTransform.GetComponent<PlayerHorizontalMovement>().enabled = true;

        if (Player.playerTransform.GetComponent<PlayerJump>() != null)
            Player.playerTransform.GetComponent<PlayerJump>().enabled = true;

        if (Player.playerTransform.GetComponent<PlayerLifeManager>() != null)
            Player.playerTransform.GetComponent<PlayerLifeManager>().enabled = true;

        foreach (Collider2D collider2d in Player.playerTransform.GetComponents<Collider2D>())
        {
            collider2d.enabled = true;
        }

        if (playerPositionInMountObj.GetComponent<RideTheMount>().disableWeaponOfPlayer)
        {
            if (Player.playerTransform.GetComponent<UseWeapon>() != null)
                Player.playerTransform.GetComponent<UseWeapon>().enabled = true;
        }
        //------------------------

        currentMount = null;

        //This is made because sometimes the mount could have an animation that rotatates the player, and when dismount the mount could have errors. With this it's solved
        Player.playerTransform.localRotation = new Quaternion(0,0,0,1);

        Player.playerTransform.SetParent(null);
        ridingActivated = false;
        startTimerToReactivate = true;
    }

    void Update()
    {
        if (startTimerToReactivate)
        {
            timerToReactivateMount += Time.deltaTime;

            if (timerToReactivateMount > timeToReactivateMount)
            {
                timerToReactivateMount = 0;
                startTimerToReactivate = false;
                ridingActivated = true;
            }
        }

        if (InputControls.GetControlDown(inputToDismount))
        {
            if (currentMount != null)
                DismountPlayer();
        }
    }
}
