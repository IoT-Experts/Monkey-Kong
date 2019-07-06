using UnityEngine;
using UnityEngine.EventSystems;

public class CallModalExample : MonoBehaviour, IPointerClickHandler
{
    public Sprite icon;
    public string desctiption;
    public string textOfButtonYes;
    public string textOfButtonNot;
    ModalPanel modalPanel;

    void Awake()
    {
        modalPanel = ModalPanel.Instance();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ModalPanelDetails modalPanelDetails = new ModalPanelDetails(
            desctiption, icon,
            new EventButtonDetails(textOfButtonYes, YesFunction),
            new EventButtonDetails(textOfButtonNot, NoFunction));

        modalPanel.NewChoice(modalPanelDetails);
    }

    //  The function to call when the button is clicked
    void YesFunction()
    {
        Debug.Log("Example if do click in yes");
    }

    void NoFunction()
    {
        Debug.Log("Example if do click in not");
    }
}