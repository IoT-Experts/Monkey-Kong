using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using EkumeSavedData;
using EkumeSavedData.Player;

[RequireComponent(typeof(Collider2D))]
public class PlayerLifeReductor : MonoBehaviour
{
    public enum ActivatorOfAction { OnCollisionEnter2D, OnCollisionExit2D, OnCollisionStay2D, OnTriggerEnter2D, OnTriggerExit2D, OnTriggerStay2D }
    public ActivatorOfAction actionActivator;
    //Life to reduce to the player when collides with this
    public bool reduceAllHealth;
    //If action is OnStay will be reduced this by second
    public float healthToReduce;
    //If the player is using some power of shield
    public bool ignoreShields;
    //If the player is in immunity time
    public bool ignoreImmunityTime;
    //If the player is in a mount
    public bool sendDamagesToThePlayer;

    bool startConstantLifeReductionPlayer;
    bool startConstantLifeReductionMount;

    MountLifeManager mountLifeDetected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel) && (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime)))
        {
            if ((other.tag == "Player")
                || (other.tag == "Mount" && sendDamagesToThePlayer && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
               // || (other.tag == "Mount" && reduceAllHealth && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
               )
            {
                if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                {
                    ReduceFixedHealthToPlayer();
                }
                else if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
                {
                    startConstantLifeReductionPlayer = true;
                    if (healthToReduce > 0)
                        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, true);
                }
            }
            else if (other.tag == "Mount" && other.GetComponent<MountLifeManager>() != null
                    && other.GetComponent<MountLifeManager>().isActiveAndEnabled
                    && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                {
                    ReduceFixedHealthToMount(other.GetComponent<MountLifeManager>());
                }
                else if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
                {
                    startConstantLifeReductionMount = true;
                    mountLifeDetected = other.GetComponent<MountLifeManager>();
                    if (healthToReduce > 0)
                        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel))
        {
            if (other.tag == "Player"
            || (other.tag == "Mount" && sendDamagesToThePlayer && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            //|| (other.tag == "Mount" && reduceAllHealth && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            )
            {
                if (actionActivator == ActivatorOfAction.OnTriggerExit2D)
                {
                    ReduceFixedHealthToPlayer();
                }
                else if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
                {
                    startConstantLifeReductionPlayer = false;
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, false);
                }
            }
            else if (other.tag == "Mount" && other.GetComponent<MountLifeManager>() != null
                    && other.GetComponent<MountLifeManager>().isActiveAndEnabled
                    && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                {
                    ReduceFixedHealthToMount(other.GetComponent<MountLifeManager>());
                }
                else if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
                {
                    startConstantLifeReductionMount = false;
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, false);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel) && (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime)))
        {
            if ((other.collider.tag == "Player") ||
            (other.collider.tag == "Mount" && sendDamagesToThePlayer && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            //||
            //(other.collider.tag == "Mount" && reduceAllHealth && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)) && DismountOfPlayer.currentMount == other.gameObject
            )
            {
                if (actionActivator == ActivatorOfAction.OnCollisionEnter2D)
                {
                    ReduceFixedHealthToPlayer();
                }
                else if (actionActivator == ActivatorOfAction.OnCollisionStay2D)
                {
                    startConstantLifeReductionPlayer = true;
                    if (healthToReduce > 0)
                        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, true);
                }
            }
            else if (other.collider.tag == "Mount" && other.collider.GetComponent<MountLifeManager>() != null
                      && other.collider.GetComponent<MountLifeManager>().isActiveAndEnabled
                      && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (actionActivator == ActivatorOfAction.OnCollisionEnter2D)
                {
                    ReduceFixedHealthToMount(other.collider.GetComponent<MountLifeManager>());
                }
                else if (actionActivator == ActivatorOfAction.OnCollisionStay2D)
                {
                    startConstantLifeReductionMount = true;
                    mountLifeDetected = other.collider.GetComponent<MountLifeManager>();
                    if (healthToReduce > 0)
                        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, true);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel))
        {
            if (other.collider.tag == "Player" ||
            (other.collider.tag == "Mount" && sendDamagesToThePlayer && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            //||
            //(other.collider.tag == "Mount" && reduceAllHealth && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount) && DismountOfPlayer.currentMount == other.gameObject)
            )
            {
                if (actionActivator == ActivatorOfAction.OnCollisionExit2D)
                {
                    ReduceFixedHealthToPlayer();
                }
                else if (actionActivator == ActivatorOfAction.OnCollisionStay2D)
                {
                    startConstantLifeReductionPlayer = false;
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, false);
                }
            }
            else if (other.collider.tag == "Mount" && other.collider.GetComponent<MountLifeManager>() != null
                 && other.collider.GetComponent<MountLifeManager>().isActiveAndEnabled
                 && DismountOfPlayer.currentMount == other.gameObject)
            {
                if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                {
                    ReduceFixedHealthToMount(other.collider.GetComponent<MountLifeManager>());
                }
                else if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
                {
                    startConstantLifeReductionMount = false;
                    PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInConstantReductionOfLife, false);
                }
            }
        }
    }

    void Update()
    {
        if (startConstantLifeReductionPlayer)
        {
            PlayerStats.SetHealth(PlayerStats.GetHealth() - (healthToReduce * Time.deltaTime));
            Player.playerLifeManager.RefreshLife();
        }
        else if (startConstantLifeReductionMount)
        {
            mountLifeDetected.health = mountLifeDetected.health - (healthToReduce * Time.deltaTime);
            mountLifeDetected.RefreshLife();
        }
    }

    void ReduceFixedHealthToPlayer() //This function is called from the collider/trigger functions but NOT from OnStay functions
    {
        //If ignore the shields, OR, not ignore shields and the shields are not actived
        if (ignoreShields || (!ignoreShields && (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerProtectorShield) && !PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerKillerShield))))
        {
            if (reduceAllHealth)
            {
                if (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime))
                {
                    PlayerStats.SetHealth(-1, true);
                }

                PlayerStats._immunityTimeActivated = true;
            }
            else
            {
                if (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime))
                    PlayerStats.SetHealth(PlayerStats.GetHealth() - healthToReduce);

                PlayerStats._immunityTimeActivated = true;
            }

            if (healthToReduce > 0)
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseHealthPoint, true);

            Player.playerLifeManager.RefreshLife();
        }
    }

    void ReduceFixedHealthToMount(MountLifeManager mountLife) //This function is called from the collider/trigger functions but NOT from OnStay functions
    {
        //If ignore the shields, OR, not ignore shields and the shields is not actived
        if (ignoreShields || (!ignoreShields && (!PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerProtectorShield) && !PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerKillerShield))))
        {
            //If ignore shields and event power shield/killer shield
            if (reduceAllHealth)
            {
                if (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime))
                    mountLife.health = -1;

                PlayerStats._immunityTimeActivated = true;
            }
            else
            {
                if (!PlayerStats._immunityTimeActivated || (PlayerStats._immunityTimeActivated && ignoreImmunityTime))
                    mountLife.health = mountLife.health - healthToReduce;

                PlayerStats._immunityTimeActivated = true;
            }

            if(healthToReduce > 0)
                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseHealthPoint, true);

            mountLife.RefreshLife();
        }
    }
}