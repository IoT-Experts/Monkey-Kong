using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEnableOrDisableObject : MonoBehaviour, IPointerClickHandler
{
    [Header("If object is enable, will be disable")]
    [Space()]
    [Header("If object is disable, will be enable")]
    public GameObject gameObjectToSwitch;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObjectToSwitch.activeSelf)
        {
            gameObjectToSwitch.SetActive(true);
        }
        else
        {
            gameObjectToSwitch.SetActive(false);
        }
    }
}
