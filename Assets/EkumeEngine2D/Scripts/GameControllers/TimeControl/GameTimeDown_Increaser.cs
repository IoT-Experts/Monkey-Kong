using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Collider2D))]
public class GameTimeDown_Increaser : MonoBehaviour {

    [SerializeField] enum ActionToActive { OnTriggerEnter2D, OnCollisionEnter2D }
    [SerializeField] ActionToActive actionToActive;
    [SerializeField] float timeToIncrease;
    [SerializeField] bool destroyItWhenItIsObtained;
    [HideWhenFalse("destroyItWhenItIsObtained")]
    [SerializeField] float delayToDestroy;
    GameTimeDown gameTime;

    void Awake ()
    {
        gameTime = FindObjectOfType<GameTimeDown>();
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            if (actionToActive == ActionToActive.OnTriggerEnter2D)
            {
                gameTime.timeOfCounter += timeToIncrease;
                if (destroyItWhenItIsObtained)
                    Destroy(this.gameObject, delayToDestroy);
            }
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "Mount")
        {
            if (actionToActive == ActionToActive.OnCollisionEnter2D)
            {
                gameTime.timeOfCounter += timeToIncrease;
                if (destroyItWhenItIsObtained)
                    Destroy(this.gameObject, delayToDestroy);
            }
        }
    }
}
