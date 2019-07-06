using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimatorManagerOfEnemy : MonoBehaviour
{

    public List<EnemyStatesEnum> enemyStates = new List<EnemyStatesEnum>();
    public List<string> parameterNames = new List<string>();

    public Enemy enemy;
    public Animator currentAnimator;

    int framesCounter; //Used for optimization

    void Awake()
    {
        currentAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        enemy = this.GetComponent<Enemy>();
        StartCoroutine("StatusUpdate");
    }

    IEnumerator StatusUpdate()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.075f);
            RefreshAnimations();
        }
    }

    void OnDisable()
    {
        RefreshAnimations();
    }

    void RefreshAnimations()
    {
#if UNITY_EDITOR
        if (currentAnimator.isInitialized)
        {
#endif
            for (int i = 0; i < enemyStates.Count; i++)
            {
                currentAnimator.SetBool(parameterNames[i], enemy.GetStateValue(enemyStates[i]));
            }
#if UNITY_EDITOR
        }
#endif
    }
}