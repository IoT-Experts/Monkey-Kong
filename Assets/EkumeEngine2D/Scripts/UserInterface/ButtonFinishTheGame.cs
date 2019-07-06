using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFinishTheGame : MonoBehaviour, IPointerClickHandler
{

    [Header("If the value is unchecked the player will lose")]
    [SerializeField] bool win;
    
    [Header("If continue window is enabled")]
    [SerializeField] bool disableWindowOfContinue = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!win)
            FindObjectOfType<GameOverManager>().delayToShowLevelFinished = 0;

        FindObjectOfType<GameOverManager>().FinishTheGame((win) ? "win" : "lose", false);

        if (disableWindowOfContinue && FindObjectOfType<GameOverManager>().uIConfirmContinue != null)
            FindObjectOfType<GameOverManager>().uIConfirmContinue.gameObject.SetActive(false);

        Time.timeScale = 1;
    }
}
