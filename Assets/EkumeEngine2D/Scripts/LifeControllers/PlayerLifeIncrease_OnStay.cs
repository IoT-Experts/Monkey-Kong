using UnityEngine;
using System.Collections;
using EkumeSavedData;
using EkumeSavedData.Player;

[RequireComponent(typeof(Collider2D))]
public class PlayerLifeIncrease_OnStay : MonoBehaviour {

    [SerializeField] enum ActionActivator { OnTriggerStay2D, OnCollisionStay2D }
    [SerializeField] ActionActivator actionToIncrease;
    [Header("This value will be increased per second")]
    [SerializeField] float howManyIncrease;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            if (actionToIncrease == ActionActivator.OnTriggerStay2D)
            {
                if (PlayerStats.GetHealth() < PlayerStats._defaultHealth)
                {
                    PlayerStats.SetHealth(PlayerStats.GetHealth() + (howManyIncrease * Time.deltaTime));
                    other.GetComponent<PlayerLifeManager>().RefreshLife();
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            if (actionToIncrease == ActionActivator.OnCollisionStay2D)
            {
                if (PlayerStats.GetHealth() < PlayerStats._defaultHealth)
                {
                    PlayerStats.SetHealth(PlayerStats.GetHealth() + (howManyIncrease * Time.deltaTime));
                    other.collider.GetComponent<PlayerLifeManager>().RefreshLife();
                }
            }
        }
    }

}
