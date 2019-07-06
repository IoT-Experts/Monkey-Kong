using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.WeaponInventory;

public class SelectorButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject objecToEnable;
    public Transform parentOfObjects;

    void Start ()
    {
        if(GetComponent<SelectWeaponButton>() != null)
        {
            if(Weapons.GetWeaponThatIsUsing() == GetComponent<SelectWeaponButton>().weaponToUse.Index)
            {
                Actions();
            }
        }
        else if (GetComponent<WeaponBox>() != null)
        {
            if (Weapons.GetWeaponThatIsUsing() == GetComponent<WeaponBox>().weapon.Index)
            {
                Actions();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Actions();
    }

    void Actions ()
    {
        foreach (Transform child in parentOfObjects)
        {
            child.gameObject.SetActive(false);
        }

        objecToEnable.SetActive(true);
    }
}
