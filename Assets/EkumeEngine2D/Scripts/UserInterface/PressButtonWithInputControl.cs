using UnityEngine;
using EkumeLists;
using UnityEngine.EventSystems;

public class PressButtonWithInputControl : MonoBehaviour
{
    public ListOfInputControls inputControl;
    
    void Update()
    {
        if (InputControls.GetControlDown(inputControl.Index))
        {
            ExecuteEvents.Execute<IPointerClickHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}
