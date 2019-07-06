using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EkumeEnumerations;

[RequireComponent(typeof(Button))]
public class ButtonToReappearPlayerInSavePoint : MonoBehaviour, IPointerClickHandler
{ 
    GameOverManager finishLevelController;
    [SerializeField] bool disableWindowOfContinue = true;

    void Awake()
    {
        finishLevelController = FindObjectOfType<GameOverManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel, false);
        PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerReappearInSavePoint, true);

        if (disableWindowOfContinue && finishLevelController.uIConfirmContinue != null)
            finishLevelController.uIConfirmContinue.gameObject.SetActive(false);

        //Reappear player in the save point, and reactive the components of the player to continue playing.
        //This function also fill and refresh the health of the player to continue playing
        finishLevelController.reapearPlayerInSavePoint();

        if (FindObjectOfType<GameTimeDown>() != null)
        {
            GameTimeDown gameTime = FindObjectOfType<GameTimeDown>();

            if (gameTime.restartTime)
                gameTime.timeOfCounter = gameTime.originalTime;
            else if (gameTime.continueWithLastSavedTime)
                gameTime.timeOfCounter = EkumeSavedData.SavePoints.GetLastTimeOfSavePoint();

            gameTime.finishTime = false;
        }
        else if (FindObjectOfType<GameTimeUpScore>() != null)
        {
            GameTimeUpScore gameTimeUp = FindObjectOfType<GameTimeUpScore>();

            if(gameTimeUp.restartTime)
                gameTimeUp.timeOfCounter = 0;
            if(gameTimeUp.continueWithLastSavedTime)
                gameTimeUp.timeOfCounter = EkumeSavedData.SavePoints.GetLastTimeOfSavePoint();

            gameTimeUp.finishTime = false;
        }
    }
}
