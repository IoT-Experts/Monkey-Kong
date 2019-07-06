using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.Scores;
using EkumeSavedData.WeaponInventory;
using UnityEngine.UI;
using EkumeLists;

[RequireComponent(typeof(Button))]
public class BuyWeaponButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfScores ScoreType;
    public int price;
    public Text buttonText;
    public ListOfWeapons weaponToBuy;

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

        if(buttonText != null)
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

            Weapons.SetWeaponToInventory(weaponToBuy.Index);
            Weapons.SetWeaponThatIsUsing(weaponToBuy.Index);

            UseWeapon[] weponsInGame = FindObjectsOfType<UseWeapon>();
            foreach (UseWeapon weapon in weponsInGame)
            {
                if (weapon.tag == "Player" || weapon.GetComponent<Player>() != null)
                {
                    weapon.ChangeWeapon(weaponToBuy.Index);
                }
            }

            WeaponBox[] uiOfWeapons = FindObjectsOfType<WeaponBox>();
            foreach (WeaponBox uiWeapon in uiOfWeapons)
            {
                uiWeapon.RefreshSelection();
            }

            ShowUnlockedWeapons[] showUnlockedWeapons = FindObjectsOfType<ShowUnlockedWeapons>();
            foreach (ShowUnlockedWeapons obj in showUnlockedWeapons)
            {
                obj.Refresh();
            }

            ShowScore.refreshScores = true;
        }
    }

    void DoNothing() { }
}
