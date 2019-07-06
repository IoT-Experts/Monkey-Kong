using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.Player;
using UnityEngine.UI;
using EkumeLists;

[RequireComponent(typeof(Button))]
public class SelectCharacterButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ListOfPlayers characterToSelect = new ListOfPlayers();

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerSelection.SetPlayerSelected(characterToSelect.Index);
    }
}
