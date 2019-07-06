using UnityEngine;
using System.Collections;
using EkumeSavedData;
using UnityEngine.UI;
using EkumeEnumerations;
using EkumeSavedData.Player;

public class MountLifeManager : MonoBehaviour
{
    public RideTheMount rideTheMount;

    public GameObject mountUI;
    public bool showIconForMount;
    public Sprite mountIcon;
    public Image uIOfIcon;

    //Life of the mount
    public float health;
    public bool countHealth0;

    //Immunity time (Executed later of life reduction)
    public float immunityTime;

    public bool useUIForMount;

    //Show life filler in UI
    public bool useHealthFilling;
    public Image healthFiller;

    public float timeToDestroyWhenDie;

    float initialHealth;
    float timerOfImmunity;
    
    void Awake ()
    {
        initialHealth = health;
    }

    void Start()
    {
        if (gameObject.tag == "Mount")
        {
            this.enabled = false;
        }
    }

    void OnEnable ()
    {
        if(useUIForMount && showIconForMount && uIOfIcon != null)
            uIOfIcon.sprite = mountIcon;

        if (useUIForMount && useHealthFilling)
        {
            RefreshLife();
        }
    }

    void Update()
    {
        if (PlayerStats._immunityTimeActivated)
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInImmunityTime, true);
            timerOfImmunity += Time.deltaTime;

            if (timerOfImmunity >= immunityTime)
            {
                PlayerStats._immunityTimeActivated = false;
                timerOfImmunity = 0;

                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerIsInImmunityTime, false);
            }
        }
    }

    public void RefreshLife()
    {
        if (useUIForMount && useHealthFilling && healthFiller != null)
        {
            healthFiller.fillAmount = health / initialHealth;
        }

        //(if countLenghtLife0 is true then to the lenghtOfLife add 1, else get the normal life), later to the value obtained of this conditional compare if is less than or equal to 0
        if (((countHealth0) ? health + 1 : health) <= 0) //Mount have 0 lenght of life
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseOneLive, true);
            DismountOfPlayer.DismountPlayer();
            Destroy(gameObject, timeToDestroyWhenDie);
            Destroy(rideTheMount.gameObject);
        }
    }

}
