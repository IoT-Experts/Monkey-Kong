using UnityEngine;
using System.Collections;
using EkumeSavedData;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EkumeSavedData.Player;

public class PlayerLifeManager : MonoBehaviour
{
    //If ends the total of lives, don't will do this actions
    //Actions to do when the length of life ends
    public bool killPlayerWhenHealth0;
    public bool deleteAllLevelsClearedWhenHealth0;
    public bool deleteAllLevelsClearedOfCurrentWorldWhenHealth0;
    public bool deleteSavePointsOfCurrentLevelWhenHealth0;
    //Actions to do when the total of lives ends
    public bool killPlayerWhenLives0;
    public bool deleteAllLevelsClearedWhenLives0;
    public bool deleteAllLevelsClearedOfCurrentWorldWhenLives0;
    public bool deleteSavePointsOfCurrentLevelWhenLives0;
    //Default values for life (This values don't change in runtime
    public int defaultTotalLives;
    public float defaultHealth;
    //Immunity time (Executed later of life reduction
    public float immunityTime;
    //Count 0 values in the counters
    public bool countLive0;
    public bool countHealth0;
    //Type of life
    public bool useHealthFilling;
    public Image healthFillerImage;
    public bool useHealthCounter;
    public RectTransform healthCounterParent;
    public GameObject healthIconForLifeCounter;
    //Fill lenght of life with default value when start
    public bool fillLenghtOfLife;
    //UI of totalLives
    public bool showTotalLives;
    public Text totalLivesText;
    public string formatTotalLives;
    //Shows the icon of the player in the UI
    public bool usePlayerIcon;
    public Image uIOfPlayerIcon;
    public Sprite playerIcon;

    bool showContinueIfHealth0;
    bool showContinueIfLives0;
    float initialLife;

    float timerOfImmunity;

    GameOverManager finishLevelController;

    void Start()
    {
        if (defaultTotalLives == 0 && !countLive0)
        {
            Debug.LogError("Error: The player should have at less one live point or should count the live 0");
        }

        PlayerStats._defaultTotalLives = defaultTotalLives;
        PlayerStats._defaultHealth = defaultHealth;

        finishLevelController = FindObjectOfType<GameOverManager>();

        if (fillLenghtOfLife)
            PlayerStats.SetHealth(defaultHealth);

        if (useHealthFilling)
        {
            initialLife = PlayerStats.GetHealth();
            RefreshLife();
        }
        else if (useHealthCounter)
        {
            RefreshLife();
        }

        if (usePlayerIcon && uIOfPlayerIcon != null)
        {
            uIOfPlayerIcon.sprite = playerIcon;
        }
    }

    void Update()
    {
        if (PlayerStats._immunityTimeActivated)
        {
            PlayerStates.SetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsInImmunityTime, true);
            timerOfImmunity += Time.deltaTime;

            if (timerOfImmunity >= immunityTime)
            {
                PlayerStats._immunityTimeActivated = false;
                timerOfImmunity = 0;

                PlayerStates.SetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsInImmunityTime, false);
            }
        }
    }

    public void RefreshLife()
    {
        if (useHealthFilling && healthFillerImage != null)
        {
            healthFillerImage.fillAmount = PlayerStats.GetHealth() / initialLife;
        }
        else if (useHealthCounter)
        {
            int numberOfCurrentLifeCounters = healthCounterParent.childCount;
            for (int i = 0; i < numberOfCurrentLifeCounters; i++)
            {
                GameObject lifeIconGameObj = healthCounterParent.GetChild(0).gameObject;
                lifeIconGameObj.transform.SetParent(null);
                Destroy(lifeIconGameObj.gameObject); //Destroy all icons of lifes
            }
            for (int i = 0; i < PlayerStats.GetHealth(); i++)
            {
                GameObject instantiatedIcon = Instantiate(healthIconForLifeCounter); //Instantiate the respective life icons
                instantiatedIcon.transform.SetParent(healthCounterParent, false);
            }
        }

        if(showTotalLives && totalLivesText != null)
            totalLivesText.text = PlayerStats.GetTotalLives().ToString(" "+formatTotalLives);

        //(if countHealth0 is true then to the current health add 1, else get the current health), later to the value obtained of this conditional compare if is less than or equal to 0
        if (((countHealth0) ? PlayerStats.GetHealth() + 1 : PlayerStats.GetHealth()) <= 0) //Verify if player have 0 of health
        {
            if (deleteSavePointsOfCurrentLevelWhenHealth0)
                showContinueIfHealth0 = false;
            else
                showContinueIfHealth0 = true;

            PlayerStats.SetTotalLives(PlayerStats.GetTotalLives() - 1);
            PlayerStates.SetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerLoseOneLive, true);
            
            PlayerStats.SetHealth(PlayerStats._defaultHealth);

            if (((countLive0) ? PlayerStats.GetTotalLives() + 1 : PlayerStats.GetTotalLives()) > 0)
            {
                if (transform.parent != null) //If player is in mount (if is child of some transform)
                {
                    if(Player.instance.GetComponent<DismountOfPlayer>() != null)
                       DismountOfPlayer.DismountPlayer(); //Dismount the player
                }
                ActivateAction("health0");
            }
        }

        //(if countLive0 is true then to the total lives add 1, else get the current normal lives), later to the value obtained of this conditional compare if is less than or equal to 0
        if (((countLive0) ? PlayerStats.GetTotalLives() + 1 : PlayerStats.GetTotalLives()) <= 0) //Player have 0 lives
        {
            if (/*killPlayerWhenLives0 || */ deleteSavePointsOfCurrentLevelWhenLives0)
                showContinueIfLives0 = false;
            else
                showContinueIfLives0 = true;

            PlayerStates.SetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerLoseAllLives, true);

            if (transform.parent != null) //If player is in mount (if is child of some transform)
            {
                if(Player.instance.GetComponent<DismountOfPlayer>() != null)
                    DismountOfPlayer.DismountPlayer(); //Dismount the player
            }

            PlayerStats.SetTotalLives(defaultTotalLives);
            ActivateAction("lives0");
        }
    }

    /// <param name="actionFor">Write "health0" if ends the health. Write "lives0" if ends the lives</param>
    void ActivateAction(string actionFor)
    {
        if (actionFor == "health0")
        {
            if (killPlayerWhenHealth0)
            {
                finishLevelController.FinishTheGame("lose", showContinueIfHealth0);
            }

            if (deleteAllLevelsClearedWhenHealth0)
                Levels.DisableAllLevelsCleared();

            if (deleteAllLevelsClearedOfCurrentWorldWhenHealth0)
                Levels.DisableAllLevelsClearedOfAWorld(Levels.SceneNameToWorldNumber(SceneManager.GetActiveScene().name)); //Disable all levels of the actual world

            if (deleteSavePointsOfCurrentLevelWhenHealth0)
                EkumeSavedData.SavePoints.DeleteSavePoint();

        }
        else if (actionFor == "lives0")
        {
            if (killPlayerWhenLives0)
            {
                finishLevelController.FinishTheGame("lose", showContinueIfLives0);
            }

            if (deleteAllLevelsClearedWhenLives0)
                Levels.DisableAllLevelsCleared();

            if (deleteAllLevelsClearedOfCurrentWorldWhenLives0)
                Levels.DisableAllLevelsClearedOfAWorld(Levels.SceneNameToWorldNumber(SceneManager.GetActiveScene().name)); //Disable all levels of the actual world

            if (deleteSavePointsOfCurrentLevelWhenLives0)
                EkumeSavedData.SavePoints.DeleteSavePoint();

        }
    }
}