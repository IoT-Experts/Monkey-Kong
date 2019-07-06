using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using EkumeSavedData.WeaponInventory;
using EkumeLists;

[Serializable]
public class ItemToUnlock
{
    public ListOfWeapons weaponToCheck;
    public GameObject objectToEnable;
}

public class ShowUnlockedWeapons : MonoBehaviour
{
    enum OptionsToShowWeapons { EnableTheWeaponsOfTheInventory, EnableTheWeaponThatIsUsing }
    [SerializeField] OptionsToShowWeapons whatToShow;
    [SerializeField] List<ItemToUnlock> weaponsToShow = new List<ItemToUnlock>();
    [SerializeField] bool enableObjectInException;
    [HideWhenFalse("enableObjectInException")]
    [SerializeField] GameObject objectToEnable;

    bool foundSomeone = false;

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (ItemToUnlock item in weaponsToShow)
        {
            if (whatToShow == OptionsToShowWeapons.EnableTheWeaponThatIsUsing)
            {
                if (Weapons.GetWeaponThatIsUsing() == item.weaponToCheck.Index)
                {
                    item.objectToEnable.SetActive(true);
                    foundSomeone = true;
                }
                else
                {
                    item.objectToEnable.SetActive(false);
                }
            }
            else
            {
                if (Weapons.IsWeaponInInventory(item.weaponToCheck.Index))
                {
                    item.objectToEnable.SetActive(true);
                    foundSomeone = true;
                }
                else
                {
                    item.objectToEnable.SetActive(false);
                }
            }
        }

        if(!foundSomeone)
        {
            if(enableObjectInException && objectToEnable != null)
                objectToEnable.SetActive(true);
        }
        else
        {
            if (objectToEnable != null)
                objectToEnable.SetActive(false);
        }
    }
}
