using UnityEngine;
using System.Collections;
using EkumeSavedData;
using UnityEngine.SceneManagement;

public class DestroyIfWasObtainedBefore : MonoBehaviour {

    [SerializeField] enum ActionToActivate { OnTriggerEnter2D, OnCollisionEnter2D }
    [SerializeField] ActionToActivate howIsObtained;

    void Awake ()
    {
        if (ObjectsInGame.IsThisObjectObtainedBefore(SceneManager.GetActiveScene().name
                                             + "-" + transform.position.x + "-" + transform.position.y
                                             + "-" + gameObject.name))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (howIsObtained == ActionToActivate.OnTriggerEnter2D)
        {
            if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsRidingAMount)))
            {
                SetObject();

            }
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (howIsObtained == ActionToActivate.OnCollisionEnter2D)
        {
            if (other.collider.tag == "Player" || (other.collider.tag == "Mount" && PlayerStates.GetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsRidingAMount)))
            {
                SetObject();
            }
        }
    }

    void SetObject ()
    {
        ObjectsInGame.SetObjectObtained(SceneManager.GetActiveScene().name
                                 + "-" + transform.position.x + "-" + transform.position.y
                                 + "-" + gameObject.name);
    }
}
