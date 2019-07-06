using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EkumeEnumerations;

[RequireComponent(typeof(Enemy))]
public class EnemyLifeManager : MonoBehaviour
{
    //Life of the monster
    public float health;
    public bool showUIOfHealth;
    //If you want to show the UI of life since start
    public bool showUIOfHealthSinceStart;
    public GameObject uIHealthContainer;
    public bool useHealthFilling;
    public Image healthFiller;
    public bool showHealthPoints;
    public bool showCurrentDivTotal;
    public Text healthPointsText;
    //Time to destroy monster when die
    public float timeToDestroyObjWhenDie;
    
    /* Hide in inspector */ public float currentLife;

    Enemy enemyStates;
    AIEnemyMovement enemyMovement;

    bool uiStartDirectionIsNegative;

    void Awake()
    {
        currentLife = health;

        enemyStates = GetComponent<Enemy>();
        if(GetComponent<AIEnemyMovement>() != null)
        {
            enemyMovement = GetComponent<AIEnemyMovement>();
        }

        if (!useHealthFilling && healthFiller != null)
        {
            healthFiller.transform.parent.gameObject.SetActive(false);
        }

        RefreshLife();

        if(uIHealthContainer != null)
        {
            if (uIHealthContainer.transform.localScale.x < 0)
            {
                uiStartDirectionIsNegative = true;
            }
            else
            {
                uiStartDirectionIsNegative = false;
            }
        }

        if (!showUIOfHealthSinceStart && uIHealthContainer != null)
        {
            uIHealthContainer.SetActive(false);
        }
    }

    bool flip_A;
    bool flip_B;
    void LateUpdate ()
    {
        if (showUIOfHealth && enemyMovement != null)
        {
            if (!flip_A && enemyMovement.currentDirection == DirectionsXAxisEnum.Left && enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight)
                || enemyMovement.currentDirection == DirectionsXAxisEnum.Right && enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
            {
                uIHealthContainer.transform.localScale = new Vector3(
                    ((!uiStartDirectionIsNegative) ? -Mathf.Abs(uIHealthContainer.transform.localScale.x) : Mathf.Abs(uIHealthContainer.transform.localScale.x)),
                    uIHealthContainer.transform.localScale.y,
                    uIHealthContainer.transform.localScale.z);

                flip_A = true;
                flip_B = false;
            }
            else if (!flip_B && enemyMovement.currentDirection == DirectionsXAxisEnum.Left && enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft)
                   || enemyMovement.currentDirection == DirectionsXAxisEnum.Right && enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
            {
                uIHealthContainer.transform.localScale = new Vector3(
                    ((!uiStartDirectionIsNegative) ? Mathf.Abs(uIHealthContainer.transform.localScale.x) : -Mathf.Abs(uIHealthContainer.transform.localScale.x)),
                    uIHealthContainer.transform.localScale.y,
                    uIHealthContainer.transform.localScale.z);

                flip_A = false;
                flip_B = true;
            }
        }
    }

    public void RefreshLife()
    {
        if (showUIOfHealth)
        {
            if (!showUIOfHealthSinceStart && uIHealthContainer != null)
                uIHealthContainer.SetActive(true);

            if (currentLife >= 0) //if current life is greater or equal to 0
            {
                if (useHealthFilling)
                    healthFiller.fillAmount = currentLife / health;

                if (showCurrentDivTotal)
                    healthPointsText.text = currentLife + "/" + health;
                else if (showHealthPoints)
                    healthPointsText.text = currentLife.ToString();
                else if (!showHealthPoints && !showCurrentDivTotal && healthPointsText != null)
                    healthPointsText.text = "";
            }
        }

        if (currentLife <= 0)
        {
            if (showUIOfHealth && uIHealthContainer != null)
                uIHealthContainer.SetActive(false);

            enemyStates.SetStateValue(EnemyStatesEnum.EnemyDie, true);
            Destroy(this.gameObject, timeToDestroyObjWhenDie);
        }
    }
}
