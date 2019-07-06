using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData.WeaponInventory;
using EkumeLists;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectWeaponButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfWeapons weaponToUse;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectWeapon();
    }

    public void SelectWeapon ()
    {
        Weapons.SetWeaponThatIsUsing(weaponToUse.Index);

        if(Player.playerWeapon != null)
            Player.playerWeapon.ChangeWeapon(weaponToUse.Index);

        WeaponBox[] uiOfWeapons = FindObjectsOfType<WeaponBox>();
        foreach (WeaponBox uiWeapon in uiOfWeapons)
        {
            uiWeapon.RefreshSelection();
        }

        ShowUnlockedWeapons[] showUnlockedWeapons = FindObjectsOfType<ShowUnlockedWeapons>();
        foreach (ShowUnlockedWeapons obj in showUnlockedWeapons)
        {
            obj.Refresh();
        }
    }
}
