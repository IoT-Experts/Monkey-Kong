using UnityEngine;
using EkumeEnumerations;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    enum ActivatorOfAction { OnCollisionEnter2D, OnCollisionExit2D, OnCollisionStay2D, OnTriggerEnter2D, OnTriggerExit2D, OnTriggerStay2D, WhenObjectStart }
    [SerializeField] ActivatorOfAction howToShowMessage;
    [SerializeField] GameObject objectToEnable;
    [SerializeField] Text textToChange;

    [Multiline]
    public string textToShow = ""; //Message to show

    [Header("If you want to show the message some seconds", order = 0)]
    [Header("(No available for OnStay actions)", order = 1)]
    [SerializeField] bool showForAMoment;

    [HideWhenFalse("showForAMoment")]
    [SerializeField] float timeToDisable;

    bool startTimer;
    float timer;

    void Start ()
    {
        if (howToShowMessage == ActivatorOfAction.WhenObjectStart)
            AppearMessage();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (howToShowMessage == ActivatorOfAction.OnTriggerEnter2D || howToShowMessage == ActivatorOfAction.OnTriggerStay2D)
                AppearMessage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (howToShowMessage == ActivatorOfAction.OnTriggerExit2D)
                AppearMessage();

            if ((howToShowMessage == ActivatorOfAction.OnTriggerStay2D || howToShowMessage == ActivatorOfAction.OnTriggerEnter2D) && !showForAMoment)
                HideMessage();
        }
            
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || (other.collider.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (howToShowMessage == ActivatorOfAction.OnCollisionEnter2D || howToShowMessage == ActivatorOfAction.OnCollisionStay2D)
                AppearMessage();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || (other.collider.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if (howToShowMessage == ActivatorOfAction.OnCollisionExit2D)
                AppearMessage();

            if ((howToShowMessage == ActivatorOfAction.OnCollisionStay2D || howToShowMessage == ActivatorOfAction.OnCollisionEnter2D) && !showForAMoment)
                HideMessage();
        }
    }

    public void AppearMessage ()
    {
         objectToEnable.SetActive(true);
         textToChange.text = textToShow;

         if (showForAMoment)
            startTimer = true;
    }

    public void HideMessage()
    {
        objectToEnable.SetActive(false);
    }

    void Update ()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;

            if (timer >= timeToDisable)
            {
                timer = 0;
                startTimer = false;
                HideMessage();
            }
        }
    }
}
