using UnityEngine;
using System.Collections.Generic;
using EkumeEnumerations;

[RequireComponent(typeof (Collider2D))]
public class EnemyLifeReductor : MonoBehaviour
{

    public enum ActivatorOfAction { OnCollisionEnter2D, OnCollisionExit2D, OnCollisionStay2D, OnTriggerEnter2D, OnTriggerExit2D, OnTriggerStay2D }
    public bool specificEnemy;
    public GameObject enemy;
    public ActivatorOfAction actionActivator;
    public bool reduceAllHealth; 
    public float healthToReduce; //Life to reduce to the player when collides with this
    public List<string> tagsThatActivatesTheAction = new List<string>(); //Tags that it affects when action is activated

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.tag))
        {
            if (actionActivator == ActivatorOfAction.OnTriggerEnter2D)
                ReduceFixedLenthLifeToMonster(
                    (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.GetComponent<EnemyLifeManager>(),
                    (specificEnemy ? enemy.GetComponent<Enemy>() : other.GetComponent<Enemy>()));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.tag))
        {
            if (actionActivator == ActivatorOfAction.OnTriggerExit2D)
                ReduceFixedLenthLifeToMonster(
                    (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.GetComponent<EnemyLifeManager>(),
                    (specificEnemy ? enemy.GetComponent<Enemy>() : other.GetComponent<Enemy>()));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.tag))
        {
            if (actionActivator == ActivatorOfAction.OnTriggerStay2D)
            {
                EnemyLifeManager monsterLife = (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.GetComponent<EnemyLifeManager>();
                monsterLife.currentLife = monsterLife.currentLife - (healthToReduce * Time.deltaTime);
                monsterLife.RefreshLife();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.collider.tag))
        {
            if (actionActivator == ActivatorOfAction.OnCollisionEnter2D)
                ReduceFixedLenthLifeToMonster(
                  (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.collider.GetComponent<EnemyLifeManager>(),
                  (specificEnemy ? enemy.GetComponent<Enemy>() : other.collider.GetComponent<Enemy>()));
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.collider.tag))
        {
            if (actionActivator == ActivatorOfAction.OnCollisionExit2D)
                ReduceFixedLenthLifeToMonster(
                  (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.collider.GetComponent<EnemyLifeManager>(),
                  (specificEnemy ? enemy.GetComponent<Enemy>() : other.collider.GetComponent<Enemy>()));
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (tagsThatActivatesTheAction.Contains(other.collider.tag))
        {
            if (actionActivator == ActivatorOfAction.OnCollisionStay2D)
            {
                EnemyLifeManager monsterLife = (specificEnemy) ? enemy.GetComponent<EnemyLifeManager>() : other.collider.GetComponent<EnemyLifeManager>();
                monsterLife.currentLife = monsterLife.currentLife - (healthToReduce * Time.deltaTime);
                monsterLife.RefreshLife();
            }
        }
    }

    void ReduceFixedLenthLifeToMonster (EnemyLifeManager monsterLife, Enemy monsterEventDetection) //This function is called from the collider/trigger functions but NOT from OnStay functions
    {
        if (monsterLife != null)
        {
            if (reduceAllHealth)
            {
                monsterLife.currentLife = 0;
            }
            else
            {
                monsterLife.currentLife = monsterLife.currentLife - healthToReduce;
            }

            monsterEventDetection.SetStateValue(EnemyStatesEnum.EnemyLoseHealthPoint, true);

            monsterLife.RefreshLife();
        }
    }
}
