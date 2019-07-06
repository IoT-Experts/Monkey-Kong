using UnityEngine;
using System.Collections;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class CallerOfPowers : MonoBehaviour
{
    [SerializeField] PowersEnum power;
    [SerializeField] bool activatePower;
    [HideWhenFalse("activatePower")]
    [SerializeField] bool useTimerToDisable;
    [Header("Update the amount available of the power")]
    [SerializeField] int amountToUpdate;
    [Space]
    [SerializeField] bool destroyWhenItsObtained;
    [HideWhenFalse("destroyWhenItsObtained")]
    [SerializeField] float delayToDestroy;

#if UNITY_EDITOR
    void OnEnable ()
    {
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("The Collider2D of the GameObject " + gameObject.name + " should be of type Trigger to use the component " + GetType().Name);
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount"))
        {
            Player.playerPowers.CallPower(power, activatePower, useTimerToDisable, false, amountToUpdate);
            if (destroyWhenItsObtained)
                Destroy(gameObject, delayToDestroy);
        }
    }
}
