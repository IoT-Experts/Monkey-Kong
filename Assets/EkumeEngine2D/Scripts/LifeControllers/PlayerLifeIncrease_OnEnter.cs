using UnityEngine;
using System.Collections;
using EkumeSavedData;
using EkumeSavedData.Player;

[RequireComponent(typeof(Collider2D))]
public class PlayerLifeIncrease_OnEnter : MonoBehaviour {

    [SerializeField] enum ActionActivator { OnTriggerEnter2D, OnCollisionEnter2D }
    [SerializeField] enum WhatIncrease { Health, Lives }
    [SerializeField] ActionActivator actionToIncrease;
    [SerializeField] WhatIncrease whatIncrease;
    [SerializeField] float howManyIncrease;
    [SerializeField] bool destroyItWhenGetIt;
    [HideWhenFalse("destroyItWhenGetIt")]
    [SerializeField] float delayToDestroy;

    PlayerLifeManager playerLife;

    void Start ()
    {
        playerLife = Player.instance.GetComponent<PlayerLifeManager>();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            if (actionToIncrease == ActionActivator.OnTriggerEnter2D)
            {
                  Increase();
            }
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            if (actionToIncrease == ActionActivator.OnCollisionEnter2D)
            {
                Increase();
            }
        }
    }

    void Increase ()
    {
        if (whatIncrease == WhatIncrease.Health)
        {
            if (PlayerStats.GetHealth() < PlayerStats._defaultHealth)
            {
                PlayerStats.SetHealth(PlayerStats.GetHealth() + howManyIncrease);
            }
        }
        else if (whatIncrease == WhatIncrease.Lives)
        {
            PlayerStats.SetTotalLives(PlayerStats.GetTotalLives() + (int)howManyIncrease);
        }

        playerLife.RefreshLife();

        if(destroyItWhenGetIt)
            Destroy(gameObject, delayToDestroy);
    }


}
