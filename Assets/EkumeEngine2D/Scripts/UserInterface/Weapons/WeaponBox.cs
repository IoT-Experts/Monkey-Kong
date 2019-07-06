using UnityEngine;
using System.Collections;
using EkumeSavedData.WeaponInventory;
using EkumeLists;

public class WeaponBox : MonoBehaviour
{
    public bool startUnlocked;
    public ListOfWeapons weapon;

    public GameObject objectToActivateIfIsUnlocked;
    public GameObject objectToActivateIfIsLocked;
    public GameObject objectToActivateIfIsSelected;

    void OnEnable ()
    {
        RefreshSelection();
    }

    void OnDisable()
    {
        RefreshSelection();
    }

    public void RefreshSelection()
    {
        if (startUnlocked || Weapons.IsWeaponInInventory(weapon.Index))
        {
            if (Weapons.GetWeaponThatIsUsing() == weapon.Index)
            {

                if (objectToActivateIfIsUnlocked != null)
                    objectToActivateIfIsUnlocked.SetActive(false);

                if (objectToActivateIfIsLocked != null)
                    objectToActivateIfIsLocked.SetActive(false);

                if (objectToActivateIfIsSelected != null)
                    objectToActivateIfIsSelected.SetActive(true);
            }
            else
            {
                if (objectToActivateIfIsLocked != null)
                    objectToActivateIfIsLocked.SetActive(false);

                if (objectToActivateIfIsSelected != null)
                    objectToActivateIfIsSelected.SetActive(false);

                if (objectToActivateIfIsUnlocked != null)
                    objectToActivateIfIsUnlocked.SetActive(true);
            }
        }
        else if (!Weapons.IsWeaponInInventory(weapon.Index))
        {
            if (objectToActivateIfIsUnlocked != null)
                objectToActivateIfIsUnlocked.SetActive(false);

            if (objectToActivateIfIsSelected != null)
                objectToActivateIfIsSelected.SetActive(false);

            if (objectToActivateIfIsLocked != null)
                objectToActivateIfIsLocked.SetActive(true);
        }
    }


}
