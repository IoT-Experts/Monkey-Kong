using UnityEngine;
using EkumeSavedData;
using EkumeEnumerations;
using System.Collections;
using System.Collections.Generic;
using EkumeSavedData.Scores;
using EkumeSavedData.Player;

public class GameOverManager : MonoBehaviour
{
    [Space()]
    [SerializeField] Transform uIOfLevelFinished;
    public float delayToShowLevelFinished;
    [Space()]
    [SerializeField] bool enableSavePoints;
    [HideWhenFalse("enableSavePoints")]
    public Transform uIConfirmContinue;
    [HideWhenFalse("enableSavePoints")]
    [SerializeField] float delayToShowContinueWindow;

    [Header("When the player lose or win")]
    [SerializeField] List<string> componentsOfPlayerToDisable = new List<string>();
    [SerializeField] bool disableRigidbody2DOfPlayer;
    [SerializeField] bool removePlayerVelocity;

    SavePoint[] savePoints;

    bool finishLevel = false;

    void Awake()
    {
        Score.scoresInLevel = new Dictionary<int, float>(); //Restart the scores of the level to start with 0

        if (enableSavePoints)
        {
            savePoints = FindObjectsOfType<SavePoint>();
        if (savePoints != null && savePoints.Length > 0)
            {
                for (int i = 0; i < savePoints.Length; i++)
                {
                    if (savePoints[i] != null)
                        savePoints[i].savePointNumber = i;
                }
            }
        }
    }

    IEnumerator ShowContinueWindow()
    {
        yield return new WaitForSeconds(delayToShowContinueWindow);
        uIConfirmContinue.gameObject.SetActive(true);
    }

    //This is called from the component "ButtonReappearPlayerInSavePoint".
    //Reappear player in the save point, and reactive the components of the player to continue playing.
    //This function also fill and refresh the health of the player to continue playing
    public void reapearPlayerInSavePoint()
    {
        Player.playerTransform.position = new Vector3(
            savePoints[EkumeSavedData.SavePoints.GetSavePointNumber()].transform.position.x,
            savePoints[EkumeSavedData.SavePoints.GetSavePointNumber()].transform.position.y, 
            Player.playerTransform.position.z);

        foreach (string component in componentsOfPlayerToDisable)
        {
            if(Player.instance.GetComponent(component) != null)
                (Player.instance.GetComponent(component) as MonoBehaviour).enabled = true;
        }

        if (disableRigidbody2DOfPlayer)
        {
            Player.instance.GetComponent<Rigidbody2D>().isKinematic = false;
        }

        Player.playerLifeManager.RefreshLife();
    }

    /// <param name="winOrLose">Write "lose" to lose, and write "win" to win.</param>
    public void FinishTheGame(string winOrLose, bool showWindowOfContinue)
    {
        if (winOrLose == "lose" && showWindowOfContinue && SavePoints.ExistSavePoint())
        {
            StartCoroutine("ShowContinueWindow");
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel, true);
        }
        else if (winOrLose == "lose")
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerLoseLevel, true);
            if (uIOfLevelFinished != null)
                StartCoroutine("ShowUIOfLevelFinished");
            else
                Debug.LogWarning("The player is died, but the component \"FinishLevelController\" don't have anything to show when the level is finished.");

            finishLevel = true;

        }
        else if(winOrLose == "win")
        {
            if (!finishLevel)
            {
                for (int i = 0; i < ScoreTypesManager.instance.ScoresData.Count; i++)
                {
                    if (ScoreTypesManager.instance.ScoresData[i].saveBestByLevel
                        && ScoreTypesManager.instance.ScoresData[i].saveBestOnlyIfWin)
                    {
                        if (Score.GetScoreOfLevel(i) > Score.GetBestScoreOfLevel(i, Levels.GetLevelIdentificationOfCurrentScene()))
                            Score.SetBestScoreOfLevel(i, Levels.GetLevelIdentificationOfCurrentScene(), Score.GetScoreOfLevel(i));
                    }

                    if (ScoreTypesManager.instance.ScoresData[i].accumulateOnlyIfWin)
                    {
                        Accumulated.SetAccumulated(i, Accumulated.GetAccumulated(i) + Score.GetScoreOfLevel(i));
                    }
                }

                if (uIOfLevelFinished != null)
                    StartCoroutine("ShowUIOfLevelFinished");
                else
                    Debug.LogWarning("The player win, but the component \"FinishLevelController\" don't have anything to show when the level is finished.");

                PlayerStates.SetPlayerStateValue(PlayerStatesEnum.PlayerWinLevel, true);
                SavePoints.DeleteSavePoint();
                finishLevel = true;
                Levels.SetLevelCleared(Levels.GetLevelIdentificationOfCurrentScene(), true);
            }
        }

        DisableComponentsOfPlayer();
    }

    public void DisableComponentsOfPlayer()
    {
        if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount))
        {
            DismountOfPlayer.DismountPlayer();
        }

        foreach (string component in componentsOfPlayerToDisable)
        {
			if(Player.instance.GetComponent(component) != null)
				(Player.instance.GetComponent(component) as MonoBehaviour).enabled = false;
        }

        if (disableRigidbody2DOfPlayer)
        {
            Player.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Player.instance.GetComponent<Rigidbody2D>().isKinematic = true;
        }

        if(removePlayerVelocity)
            Player.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }


    IEnumerator ShowUIOfLevelFinished()
    {
         yield return new WaitForSeconds(delayToShowLevelFinished);
         uIOfLevelFinished.gameObject.SetActive(true);
    }

}
