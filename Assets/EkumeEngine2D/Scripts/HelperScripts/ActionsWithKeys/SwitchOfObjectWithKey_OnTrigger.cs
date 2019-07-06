using UnityEngine;
using System.Collections;
using EkumeEnumerations;
public class SwitchOfObjectWithKey_OnTrigger : MonoBehaviour
{
    public InputControlsManager inputControlsManager;
    public int inputControl;
    public GameObject objectToSwith;
    public bool turnOn;
    public bool turnOff;
    public bool canReSwitch;

    bool enterInCollider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            enterInCollider = true;
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            enterInCollider = false;
        }
    }

    void Update ()
    {
        if(enterInCollider)
        {
            if(InputControls.GetControlDown (inputControl))
            {
                if (!canReSwitch)
                {
                    if (turnOn)
                        objectToSwith.SetActive(true);
                    else if (turnOff)
                        objectToSwith.SetActive(false);
                }
                else
                {
                    if (objectToSwith.activeSelf)
                    {
                        objectToSwith.SetActive(false);
                    }
                    else
                    {
                        objectToSwith.SetActive(true);
                    }
                }
            }
        }
    }

}
