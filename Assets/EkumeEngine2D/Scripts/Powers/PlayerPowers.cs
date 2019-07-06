using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using EkumeEnumerations;
using EkumeSavedData;
using EkumeSavedData.Player;

public class PlayerPowers : MonoBehaviour
{

#if UNITY_EDITOR
    public bool usePowerToFly;
    public bool usePowerObjectMagnet;
    public bool usePowerScoreDuplicator;
    public bool usePowerProtectorShield;
    public bool usePowerTrapsConverter;
    public bool usePowerKillerShield;
    public bool usePowerJetpack;
#endif

    public bool showCountdownOfPowerToFly;
    public Image imageCountdownToFly;

    public bool showCountdownOfPowerObjectMagnet;
    public Image imageCountdownObjectMagnet;

    public bool showCountdownOfPowerScoreDuplicator;
    public Image imageCountdownScoreDuplicator;

    public bool showCountdownOfPowerProtectorShield;
    public Image imageCountdownProtectorShield;

    public bool showCountdownOfPowerTrapsConverter;
    public Image imageCountdownTrapsConverter;

    public bool showCountdownOfPowerKillerShield;
    public Image imageCountdownKillerShield;

    public bool showCountdownOfPowerJetpack;
    public Image imageCountdownJetpack;

    //For fly
    float actualJumpForce;

    //New jump force when fly
    public float jumpForceToFly;

    //For object magnet
    public float velocityToAttract = 20;
    public List<string> tagsToAttract = new List<string>();
    public static bool objectMangetActivated;

    //For Score duplicator
    public List<int> ScoresToDuplicate = new List<int>();
    public static bool ScoreDuplicatorActivated;

    //For traps converter
    //Replacement object
    public GameObject replacementObject;
    //Tags of the objects that will be change
    public List<string> tagOfObjectsToConvert = new List<string>();

    //Killer shield
    public GameObject killerShieldToActivate;

    //Jetpack options
    public bool disableJumpWithJetpack;
    public int inputControlJetpack;
    public float maxVelocityOfJetpack;
    public float velocityToReachMaxVel;

    List<Transform> trapsAndMonstersInGame = new List<Transform>();
    List<GameObject> instantiatedReplacementObject = new List<GameObject>();

    //It is public because it is used by InsideWaterEffect.cs
    [HideInInspector] public static bool startTimerToDisablePowerToFly;
    bool startTimerToDisableObjectMagnet;
    bool startTimerToDisableScoreDuplicator;
    bool startTimerToDisableProtectorShield;
    bool startTimerToDisableTrapsConverter;
    bool startTimerToDisableKillerShield;
    bool startTimerToDisableJetpack;
    float timerPowerToFly;
    float timerObjectMagnet;
    float timerScoreDuplicator;
    float timerProtectorShield;
    float timerTrapsConverter;
    float timerKillerShield;
    float timerJetpack;

    void Awake ()
    {
        startTimerToDisablePowerToFly = false;
        objectMangetActivated = false;
        ScoreDuplicatorActivated = false;
    }

    void Start ()
    {
        timerPowerToFly = PowerStats.GetTimeOfPower(PowersEnum.FlyingPower);
        timerObjectMagnet = PowerStats.GetTimeOfPower(PowersEnum.ObjectMagnet);
        timerScoreDuplicator = PowerStats.GetTimeOfPower(PowersEnum.ScoreDuplicator);
        timerProtectorShield = PowerStats.GetTimeOfPower(PowersEnum.ProtectorShield);
        timerTrapsConverter = PowerStats.GetTimeOfPower(PowersEnum.TrapsConverter);
        timerKillerShield = PowerStats.GetTimeOfPower(PowersEnum.KillerShield);
        timerJetpack = PowerStats.GetTimeOfPower(PowersEnum.Jetpack);

        //For fly
        actualJumpForce = GetComponent<PlayerJump>().jumpForce;
    }


    void Update ()
    {
        if(startTimerToDisablePowerToFly)
        {
            timerPowerToFly -= Time.deltaTime;

            if(showCountdownOfPowerToFly && imageCountdownToFly != null)
                imageCountdownToFly.fillAmount = timerPowerToFly / PowerStats.GetTimeOfPower(PowersEnum.FlyingPower);

            if (timerPowerToFly <= 0)
            {
                startTimerToDisablePowerToFly = false;
                timerPowerToFly = PowerStats.GetTimeOfPower(PowersEnum.FlyingPower);
                CallPower(PowersEnum.FlyingPower, false, false, false, 0);

                if (showCountdownOfPowerToFly && imageCountdownToFly != null)
                    imageCountdownToFly.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableObjectMagnet)
        {
            timerObjectMagnet -= Time.deltaTime;

            if (showCountdownOfPowerObjectMagnet && imageCountdownObjectMagnet != null)
                imageCountdownObjectMagnet.fillAmount = timerObjectMagnet / PowerStats.GetTimeOfPower(PowersEnum.ObjectMagnet);

            if (timerObjectMagnet <= 0)
            {
                startTimerToDisableObjectMagnet = false;
                timerObjectMagnet = PowerStats.GetTimeOfPower(PowersEnum.ObjectMagnet);
                CallPower(PowersEnum.ObjectMagnet, false, false, false, 0);

                if (showCountdownOfPowerObjectMagnet && imageCountdownObjectMagnet != null)
                    imageCountdownObjectMagnet.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableScoreDuplicator)
        {
            timerScoreDuplicator -= Time.deltaTime;

            if (showCountdownOfPowerScoreDuplicator && imageCountdownScoreDuplicator != null)
                imageCountdownScoreDuplicator.fillAmount = timerScoreDuplicator / PowerStats.GetTimeOfPower(PowersEnum.ScoreDuplicator);

            if (timerScoreDuplicator <= 0)
            {
                startTimerToDisableScoreDuplicator = false;
                timerScoreDuplicator = PowerStats.GetTimeOfPower(PowersEnum.ScoreDuplicator);
                CallPower(PowersEnum.ScoreDuplicator, false, false, false, 0);

                if (showCountdownOfPowerScoreDuplicator && imageCountdownScoreDuplicator != null)
                    imageCountdownScoreDuplicator.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableProtectorShield)
        {
            timerProtectorShield -= Time.deltaTime;

            if (showCountdownOfPowerProtectorShield && imageCountdownProtectorShield != null)
                imageCountdownProtectorShield.fillAmount = timerProtectorShield / PowerStats.GetTimeOfPower(PowersEnum.ProtectorShield);

            if (timerProtectorShield <= 0)
            {
                startTimerToDisableProtectorShield = false;
                timerProtectorShield = PowerStats.GetTimeOfPower(PowersEnum.ProtectorShield);
                CallPower(PowersEnum.ProtectorShield, false, false, false, 0);

                if (showCountdownOfPowerProtectorShield && imageCountdownProtectorShield != null)
                    imageCountdownProtectorShield.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableTrapsConverter)
        {
            timerTrapsConverter -= Time.deltaTime;

            if (showCountdownOfPowerTrapsConverter && imageCountdownTrapsConverter != null)
                imageCountdownTrapsConverter.fillAmount = timerTrapsConverter / PowerStats.GetTimeOfPower(PowersEnum.TrapsConverter);

            if (timerTrapsConverter <= 0)
            {
                startTimerToDisableTrapsConverter = false;
                timerTrapsConverter = PowerStats.GetTimeOfPower(PowersEnum.TrapsConverter);
                CallPower(PowersEnum.TrapsConverter, false, false, false, 0);

                if (showCountdownOfPowerTrapsConverter && imageCountdownTrapsConverter != null)
                    imageCountdownTrapsConverter.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableKillerShield)
        {
            timerKillerShield -= Time.deltaTime;

            if (showCountdownOfPowerKillerShield && imageCountdownKillerShield != null)
                imageCountdownKillerShield.fillAmount = timerKillerShield / PowerStats.GetTimeOfPower(PowersEnum.KillerShield);

            if (timerKillerShield <= 0)
            {
                startTimerToDisableKillerShield = false;
                timerKillerShield = PowerStats.GetTimeOfPower(PowersEnum.KillerShield);
                CallPower(PowersEnum.KillerShield, false, false, false, 0);

                if (showCountdownOfPowerKillerShield && imageCountdownKillerShield != null)
                    imageCountdownKillerShield.gameObject.SetActive(false);
            }
        }

        if (startTimerToDisableJetpack)
        {
            timerJetpack -= Time.deltaTime;

            if (showCountdownOfPowerJetpack && imageCountdownJetpack != null)
                imageCountdownJetpack.fillAmount = timerJetpack / PowerStats.GetTimeOfPower(PowersEnum.Jetpack);

            if (timerJetpack <= 0)
            {
                startTimerToDisableJetpack = false;
                timerJetpack = PowerStats.GetTimeOfPower(PowersEnum.Jetpack);
                CallPower(PowersEnum.Jetpack, false, false, false, 0);

                if (showCountdownOfPowerJetpack && imageCountdownJetpack != null)
                    imageCountdownJetpack.gameObject.SetActive(false);
            }
        }
    }

    //----------------------------------------------------------------------------------------- /


    public void Fly (bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.FlyingPower) + amountToUpdate >= 0)
            || (activate && !checkAmountToActive))
        {
            GetComponent<PlayerJump>().jumpForce = jumpForceToFly;
            GetComponent<PlayerJump>().activateDoubleJump = false;
            GetComponent<PlayerJump>().noLimitOfJumps = true;

            if (useTimerToDisable)
            {
                startTimerToDisablePowerToFly = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerToFly && imageCountdownToFly != null)
                    imageCountdownToFly.gameObject.SetActive(true);
            }

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerToFly, true);

        }
        else if (!activate)
        {
            GetComponent<PlayerJump>().jumpForce = actualJumpForce;
            GetComponent<PlayerJump>().activateDoubleJump = GetComponent<PlayerJump>().originalActivateDoubleJump;
            GetComponent<PlayerJump>().noLimitOfJumps = GetComponent<PlayerJump>().originalNoLimitOfJumps;

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerToFly, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.FlyingPower) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.FlyingPower, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /

    public void ObjectMagnet(bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {

        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.ObjectMagnet) + amountToUpdate >= 0)
            || (activate && !checkAmountToActive))
        {
            if (useTimerToDisable)
            {
                startTimerToDisableObjectMagnet = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerObjectMagnet && imageCountdownObjectMagnet != null)
                    imageCountdownObjectMagnet.gameObject.SetActive(true);
            }

            objectMangetActivated = true;
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerObjectMagnet, true);

        }
        else if (!activate)
        {

            objectMangetActivated = false;
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerObjectMagnet, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.ObjectMagnet) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.ObjectMagnet, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /

    public void CoinsDuplicator(bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.ScoreDuplicator) + amountToUpdate >= 0)
           || (activate && !checkAmountToActive))
        {

            ScoreDuplicatorActivated = true;

            if (useTimerToDisable)
            {
                startTimerToDisableScoreDuplicator = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerScoreDuplicator && imageCountdownScoreDuplicator != null)
                    imageCountdownScoreDuplicator.gameObject.SetActive(true);
            }
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerScoreDuplicator, true);
        }
        else if (!activate)
        {
            ScoreDuplicatorActivated = false;

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerScoreDuplicator, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.ScoreDuplicator) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.ScoreDuplicator, amountToUpdate);
      
    }

    //----------------------------------------------------------------------------------------- /

    public void ProtectorShield(bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.ProtectorShield) + amountToUpdate >= 0)
        || (activate && !checkAmountToActive))
        {
            if (useTimerToDisable)
            {
                startTimerToDisableProtectorShield = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerProtectorShield && imageCountdownProtectorShield != null)
                    imageCountdownProtectorShield.gameObject.SetActive(true);
            }
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerProtectorShield, true);
        }
        else if (!activate)
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerProtectorShield, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.ProtectorShield) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.ProtectorShield, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /

    public void TrapsConverter(bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.TrapsConverter) + amountToUpdate >= 0)
            || (activate && !checkAmountToActive))
        {

            //Save all gameObjects of the scene in the array
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            //For each object in the scene
            foreach (GameObject objInGame in allObjects)
            {
                //for each tag in the list tagsOfObjsToConvert
                foreach (string objTag in tagOfObjectsToConvert)
                {
                    //if the tag of the actual iteration is the same that the object tag of the first "foreach"
                    if (objTag == objInGame.tag)
                    {
                        //add the trap to the array of traps/monsters
                        trapsAndMonstersInGame.Add(objInGame.transform);
                    }
                }
            }

            foreach (Transform obj in trapsAndMonstersInGame)
            {
                if (obj != null)
                {
                    instantiatedReplacementObject.Add(Instantiate(replacementObject, obj.transform.position, Quaternion.identity) as GameObject);
                    obj.gameObject.SetActive(false);
                }
            }

            if (useTimerToDisable)
            {
                startTimerToDisableTrapsConverter = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerTrapsConverter && imageCountdownTrapsConverter != null)
                    imageCountdownTrapsConverter.gameObject.SetActive(true);
            }

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerTrapsConverter, true);
        }
        else if (!activate)
        {
            for (int i = 0; i < trapsAndMonstersInGame.Count; i++)
            {
                if (instantiatedReplacementObject.Count != 0)
                    Destroy(instantiatedReplacementObject[i].gameObject);

                if (trapsAndMonstersInGame[i] != null)
                    trapsAndMonstersInGame[i].gameObject.SetActive(true);

            }

            instantiatedReplacementObject.Clear();
            trapsAndMonstersInGame.Clear();
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerTrapsConverter, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.TrapsConverter) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.TrapsConverter, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /

    public void KillerShield(bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.KillerShield) + amountToUpdate >= 0)
            || (activate && !checkAmountToActive))
        {
            if (useTimerToDisable)
            {
                startTimerToDisableKillerShield = true; //This start the timer in the Update function to disable the power after some seconds

                killerShieldToActivate.SetActive(true);

                if (showCountdownOfPowerKillerShield && imageCountdownKillerShield != null)
                    imageCountdownKillerShield.gameObject.SetActive(true);
            }

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerKillerShield, true);
        }
        else if (!activate)
        {
            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerKillerShield, false);

            killerShieldToActivate.SetActive(false);
        }

        if(PowerStats.GetQuantityOfPower(PowersEnum.KillerShield) + amountToUpdate >= 0)
           PowerStats.AddQuantityOfPower(PowersEnum.KillerShield, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /

    public void Jetpack (bool activate, bool useTimerToDisable, bool checkAmountToActive, int amountToUpdate)
    {
        if ((activate && PowerStats.GetQuantityOfPower(PowersEnum.Jetpack) + amountToUpdate >= 0)
            || (activate && !checkAmountToActive))
        {

            JetpackPower jetPackPowerScript = gameObject.AddComponent<JetpackPower>();

            if (disableJumpWithJetpack && GetComponent<PlayerJump>() != null)
                GetComponent<PlayerJump>().enabled = false;

            //Set the variables of Jetpack (options) to the script
            jetPackPowerScript.inputControlJetpack.Index = inputControlJetpack;
            jetPackPowerScript.maxVelocityOfJetpack = maxVelocityOfJetpack;
            jetPackPowerScript.velocity = velocityToReachMaxVel;
            //---------------------------------------------------- /

            if (useTimerToDisable)
            {
                startTimerToDisableJetpack = true; //This start the timer in the Update function to disable the power after some seconds

                if (showCountdownOfPowerJetpack && imageCountdownJetpack != null)
                    imageCountdownJetpack.gameObject.SetActive(true);
            }

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerJetpack, true);
        }
        else if (!activate)
        {

            if (disableJumpWithJetpack && GetComponent<PlayerJump>() != null)
            {
                GetComponent<PlayerJump>().enabled = true;
            }

            Destroy(GetComponent<JetpackPower>());

            PlayerStates.SetPlayerStateValue(PlayerStatesEnum.UsingPowerJetpack, false);
        }

        if (PowerStats.GetQuantityOfPower(PowersEnum.Jetpack) + amountToUpdate >= 0)
            PowerStats.AddQuantityOfPower(PowersEnum.Jetpack, amountToUpdate);
    }

    //----------------------------------------------------------------------------------------- /
    public void CallPower(PowersEnum power, bool activatePower, bool useTimerToDisable, bool checkAmount, int amountToUpdate)
    {
        switch (power)
        {
            case PowersEnum.FlyingPower:
                Fly(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.ObjectMagnet:
                ObjectMagnet(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.ScoreDuplicator:
                CoinsDuplicator(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.ProtectorShield:
                ProtectorShield(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.TrapsConverter:
                TrapsConverter(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.KillerShield:
                KillerShield(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;

            case PowersEnum.Jetpack:
                Jetpack(activatePower, useTimerToDisable, checkAmount, amountToUpdate);
                break;
        }
    }

}
