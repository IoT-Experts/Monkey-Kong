using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeEnumerations;

[System.Serializable]
public class PowerToObtain
{
    public PowersEnum power;
    [Tooltip("GameObject to activates when get the power. (Can be null if you don't want activate some object)")]
    public GameObject gameObjectToActivate;
}

[RequireComponent(typeof(Collider2D))]
public class AleatoryPower : MonoBehaviour
{
    [Header("Powers that can obtain & GameObject to enable when obtain some power")]
    [SerializeField] PowerToObtain[] powers;

    [Header("Parameters")]
    [SerializeField] bool activatePower;

    [HideWhenFalse("activatePower")]
    [SerializeField] bool useTimerToDisable;
    [SerializeField] int amountToUpdate;

    [Header("Things to do when collides")]
    [SerializeField] bool disableCollider;
    [SerializeField] bool destroyThisObject;
    [HideWhenFalse("destroyThisObject")]
    [SerializeField] float delayToDestroy;


#if UNITY_EDITOR
    void OnEnable()
    {
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("The Collider2D of the GameObject " + gameObject.name + " should be of type Trigger to use the component " + GetType().Name);
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            int randomBonus = Random.Range(0, powers.Length);
            Player.playerPowers.CallPower(powers[randomBonus].power, useTimerToDisable, activatePower, false, amountToUpdate);

            if (powers[randomBonus].gameObjectToActivate != null)
                powers[randomBonus].gameObjectToActivate.SetActive(true);

            if (disableCollider)
                GetComponent<Collider2D>().enabled = false;

            if (destroyThisObject)
                Destroy(this.gameObject, delayToDestroy);
        }
    }
}
