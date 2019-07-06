using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonActivatorOfPower : MonoBehaviour, IPointerClickHandler
{
    public PowersEnum powerToActivate;
    public bool useTimerToDisable = true;
    public bool checkAmount = true;
    [HideWhenFalse("checkAmount")]
    public int amountToUpdate = -1;
    Button thisButton;
    
    public static bool updateButton;

    void Start ()
    {
        thisButton = GetComponent<Button>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!IsUsingPower(powerToActivate))
            Player.playerPowers.CallPower(powerToActivate, true, useTimerToDisable, checkAmount, amountToUpdate);
    }

    void Update ()
    {
        if (IsUsingPower(powerToActivate))
            thisButton.interactable = false;
        else
            thisButton.interactable = true;
    }


    bool IsUsingPower(PowersEnum powerEnum)
    {
        bool usingPower = false;
        switch (powerEnum)
        {
            case PowersEnum.FlyingPower:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerToFly);
                break;

            case PowersEnum.ObjectMagnet:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerObjectMagnet);
                break;

            case PowersEnum.ScoreDuplicator:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerScoreDuplicator);
                break;

            case PowersEnum.KillerShield:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerKillerShield);
                break;

            case PowersEnum.ProtectorShield:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerProtectorShield);
                break;

            case PowersEnum.TrapsConverter:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerTrapsConverter);
                break;

            case PowersEnum.Jetpack:
                usingPower = PlayerStates.GetPlayerStateValue(PlayerStatesEnum.UsingPowerJetpack);
                break;
        }

        return usingPower;
    }
}
