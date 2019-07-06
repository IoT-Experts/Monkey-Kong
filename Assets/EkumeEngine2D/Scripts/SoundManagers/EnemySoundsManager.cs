using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;
using System;
using EkumeSavedData.WeaponInventory;

[Serializable]
public class ConditionsForSoundsOfEnemy
{
    public List<EnemyStatesEnum> enemyStates = new List<EnemyStatesEnum>();
    public List<bool> playWhenIs = new List<bool>();

#if UNITY_EDITOR
    public List<BooleanType> playWhenIs_Enum = new List<BooleanType>();
#endif
}

public class EnemySoundsManager : MonoBehaviour
{
    public Enemy enemyToGetStates;
    public List<AudioSource> audioSource = new List<AudioSource>();

    public List<ConditionsForSoundsOfEnemy> conditionsOfStates = new List<ConditionsForSoundsOfEnemy>();

    public List<int> attackCategory = new List<int>(); //For state EnemyWeaponAttack

    public List<AudioClip> audioClip = new List<AudioClip>();
    public List<bool> playLikeLoop = new List<bool>();

    List<bool> canPlay = new List<bool>();

    void Start()
    {
        enemyToGetStates = this.GetComponent<Enemy>();

        for (int i = 0; i < audioClip.Count; i++)
        {
            canPlay.Add(true);
        }
    }

    void Update()
    {
        for (int i = 0; i < audioClip.Count; i++)
        {
            bool allConditionsAreTrue = false;
            int numberOfConditionsTrue = 0;

            for (int j = 0; j < conditionsOfStates[i].enemyStates.Count; j++)
            {
                if (enemyToGetStates.GetStateValue(conditionsOfStates[i].enemyStates[j]) == conditionsOfStates[i].playWhenIs[j])
                {
                    numberOfConditionsTrue++;
                }
            }

            if ((conditionsOfStates[i].enemyStates.Count == numberOfConditionsTrue))
            {
                allConditionsAreTrue = true;
            }

            if (canPlay[i] && allConditionsAreTrue)
            {
                if (!playLikeLoop[i])
                {
                    audioSource[i].PlayOneShot(audioClip[i]);
                }
                else
                {
                    audioSource[i].clip = audioClip[i];
                    audioSource[i].loop = true;
                    audioSource[i].Play();
                }

                canPlay[i] = false;

            }
            else if (!allConditionsAreTrue)
            {
                canPlay[i] = true;

                if (playLikeLoop[i])
                {
                    audioSource[i].Stop();
                }
            }
        }
    }
}
