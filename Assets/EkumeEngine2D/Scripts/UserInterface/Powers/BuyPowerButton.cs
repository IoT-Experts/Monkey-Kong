using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.Scores;
using EkumeSavedData.Player;
using UnityEngine.UI;
using EkumeEnumerations;
using EkumeLists;

[RequireComponent(typeof(Button))]
public class BuyPowerButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfScores ScoreType;
    public int price;
    public Text buttonText;
    public PowersEnum powerToBuy;
    public int quantityToAdd;

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

        if (buttonText != null)
            buttonText.text = price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ModalPanelDetails modalPanelDetails;

        if (Accumulated.GetAccumulated(ScoreType.Index) >= price)
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

    //  The function to call when the button is clicked
    void YesFunction()
    {
        if (Accumulated.GetAccumulated(ScoreType.Index) >= price)
        {
            Accumulated.SetAccumulated(ScoreType.Index, Accumulated.GetAccumulated(ScoreType.Index) - price);
            PowerStats.AddQuantityOfPower(powerToBuy, quantityToAdd);

            ShowScore.refreshScores = true;
        }
    }

    void DoNothing() { }

}
