using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.Scores;
using EkumeSavedData.Player;
using UnityEngine.UI;
using EkumeEnumerations;
using System.Collections.Generic;
using EkumeLists;

[System.Serializable]
public class PowerUpgradingData
{
    public int price;
    public string descriptionText;
    public float timeToAdd;
    public GameObject objectToActivate;
}

[RequireComponent(typeof(Button))]
public class TimeUpgradingButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfScores ScoreType;
    
    public Text buttonText;
    public Text infoText;
    public PowersEnum powerToUpgrade;
    public List<PowerUpgradingData> upgradingData;
    public string buttonTextWhenMax;
    public string descriptionTextWhenMax;

    [Header("Modal window options if can buy")]
    public Sprite icon;
    public string desctiption;
    public string textOfButtonYes;
    public string textOfButtonNot;

    [Header("Modal window options if don't have coins")]
    public Sprite iconError;
    public string desctiptionError;
    public string textOfButtonYesInError;
    public string textOfButtonNotInError;

    ModalPanel modalPanel;

    void Awake()
    {
        modalPanel = ModalPanel.Instance();
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade); i++)
        {
            if(upgradingData[i].objectToActivate != null)
                upgradingData[i].objectToActivate.SetActive(true);
        }

        if (PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade) < upgradingData.Count)
        {
            infoText.text = upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].descriptionText;
            buttonText.text = upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].price.ToString();
        }
        else
        {
            infoText.text = descriptionTextWhenMax;
            buttonText.text = buttonTextWhenMax;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade) < upgradingData.Count)
        {
            ModalPanelDetails modalPanelDetails;

            if (Accumulated.GetAccumulated(ScoreType.Index) >= upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].price)
            {
                modalPanelDetails = new ModalPanelDetails(
                   desctiption, icon,
                   new EventButtonDetails(textOfButtonYes, YesFunction),
                   new EventButtonDetails(textOfButtonNot, DoNothing));
            }
            else
            {
                modalPanelDetails = new ModalPanelDetails(
                    desctiptionError, iconError,
                    new EventButtonDetails(textOfButtonYesInError, DoNothing),
                    new EventButtonDetails(textOfButtonNotInError, DoNothing));
            }

            modalPanel.NewChoice(modalPanelDetails);
        }
    }

    //  The function to call when the button is clicked
    void YesFunction()
    {
        if (Accumulated.GetAccumulated(ScoreType.Index) >= upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].price)
        {
            Accumulated.SetAccumulated(ScoreType.Index, Accumulated.GetAccumulated(ScoreType.Index) - upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].price);
            PowerStats.UpgradeTimeOfPower(powerToUpgrade, upgradingData[PowerStats.GetQuantityOfTimeUpgrades(powerToUpgrade)].timeToAdd);

            RefreshUI();

            ShowScore.refreshScores = true;
        }
    }

    void DoNothing() { }

}
