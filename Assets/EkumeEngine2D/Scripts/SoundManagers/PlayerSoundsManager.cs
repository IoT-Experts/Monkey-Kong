using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;
using System;
using EkumeSavedData.WeaponInventory;

[Serializable]
public class ConditionsForSoundsOfPlayer
{
    public List<PlayerStatesEnum> playerStates = new List<PlayerStatesEnum>();
    public List<bool> playWhenIs = new List<bool>();

#if UNITY_EDITOR
    public List<BooleanType> playWhenIs_Enum = new List<BooleanType>();
#endif
}

public class PlayerSoundsManager : MonoBehaviour
{
    public List<AudioSource> audioSource = new List<AudioSource>();

    public List<ConditionsForSoundsOfPlayer> conditionsOfStates = new List<ConditionsForSoundsOfPlayer>();

    public List<int> attackCategory = new List<int>(); //For state PlayerWeaponAttack

    public List<bool> useInputControl = new List<bool>();
    public List<int> inputControl = new List<int>();

    public List<AudioClip> audioClip = new List<AudioClip>();
    public List<bool> playLikeLoop = new List<bool>();

    List<bool> canPlay = new List<bool>();

    void Start()
    {
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

            for (int j = 0; j < conditionsOfStates[i].playerStates.Count; j++)
            {
                if (conditionsOfStates[i].playerStates[j] != PlayerStatesEnum.PlayerAttackWithWeapon)
                {
                    if (PlayerStates.GetPlayerStateValue(conditionsOfStates[i].playerStates[j]) == conditionsOfStates[i].playWhenIs[j])
                    {
                        numberOfConditionsTrue++;
                    }
                }
                else
                {
                    if (Weapons.GetWeaponThatIsUsing() != -1 && PlayerStates.GetAttackCategoryStateValue(WeaponFactory.instance.weaponsData[Weapons.GetWeaponThatIsUsing()].weaponCategory))
                    {
                        numberOfConditionsTrue++;
                    }
                }
            }

            if ((conditionsOfStates[i].playerStates.Count == numberOfConditionsTrue) && ((useInputControl[i]) ? InputControls.GetControlDown(inputControl[i]) : true))
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