using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.Scores;
using EkumeSavedData;
using UnityEngine.UI;
using EkumeLists;

[RequireComponent(typeof(Button))]
public class BuyItemButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfScores ScoreType;
    public int price;
    public Text buttonText;
    public ListOfClothingItems itemToBuy;

    [Header("Modal window options if can buy")]
    public Sprite icon;
    public string desctiption;
    public string textOfButtonYes;
    public string textOfButtonNot;

    [Header("Modal window options if don't have enough coins")]
    public Sprite iconError;
    public string desctiptionError;
    public string textOfButtonYesInError;
    public string textOfButtonNotInError;

    ModalPanel modalPanel;

    void Awake()
    {
        if (FindObjectOfType<ModalPanel>() != null)
        {
            modalPanel = ModalPanel.Instance();
        }
        else
        {
            Debug.LogError("Please add the component 'ModalPanel.cs' to some GameObject.");
        }

        if (buttonText != null)
            buttonText.text = price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Accumulated.GetAccumulated(ScoreType.Index) >= price)
        {
            ModalPanelDetails modalPanelDetails = new ModalPanelDetails(
               desctiption, icon,
               new EventButtonDetails(textOfButtonYes, YesFunction),
               new EventButtonDetails(textOfButtonNot, DoNothing));

            modalPanel.NewChoice(modalPanelDetails);
        }
        else
        {
            ModalPanelDetails modalPanelDetails = new ModalPanelDetails(
                desctiptionError, iconError,
                new EventButtonDetails(textOfButtonYesInError, DoNothing),
                new EventButtonDetails(textOfButtonNotInError, DoNothing));

            modalPanel.NewChoice(modalPanelDetails);
        }
    }

    //  The function to call when the button is clicked
    void YesFunction()
    {
        if (Accumulated.GetAccumulated(ScoreType.Index) >= price)
        {
            Accumulated.SetAccumulated(ScoreType.Index, Accumulated.GetAccumulated(ScoreType.Index) - price);
            ClothingInventory.SetItemToInventory(itemToBuy.Index);
            ShowScore.refreshScores = true;
        }
    }

    void DoNothing() { }
}
