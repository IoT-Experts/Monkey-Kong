using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using EkumeLists;

public class MobileInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ListOfInputControls inputControl = new ListOfInputControls();

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonDown();
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        ButtonUp();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        ButtonDown();
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        ButtonUp();
    }

    public void ButtonDown ()
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            InputControls.inputDownMobile[inputControl.Index] = true; //It is turned false in LateUpdate in InputControls.cs
            InputControls.inputPressedMobile[inputControl.Index] = true;
        }
    }

    public void ButtonUp ()
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            InputControls.inputUpMobile[inputControl.Index] = true; //It is turned false in LateUpdate in InputControls.cs
            InputControls.inputPressedMobile[inputControl.Index] = false;
        }
    }

}
