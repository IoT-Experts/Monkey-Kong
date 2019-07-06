using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EkumeEnumerations;
using EkumeSavedData.WeaponInventory;
public class ObtainBullets_OnTrigger : MonoBehaviour
{
    public WeaponFactory weaponFactory;
    //Give bullets to gun that is using
    public bool bulletsToGunThatIsUsing;
    //Exceptions of give bullet to gun that is using, for example don't give the bullets if is using: bazooka
    public List<int> weaponExceptions = new List<int>();
    //If is not using weapon or the current weapon that is using is inside of the exceptions, give bullets to this:
    public int gunIfException;
    //-------------------------------
    //Give bullets to specific gun
    public bool giveBulletsToSpecificGun;
    public int specificGun;
    //Bullets to give to the corresponding gun
    public int bulletsQuantity;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            if(bulletsToGunThatIsUsing)
            {
                if (!weaponExceptions.Contains(Weapons.GetWeaponThatIsUsing()) && Weapons.GetWeaponThatIsUsing() != -1)
                {
                    AddBulletsToGun(Weapons.GetWeaponThatIsUsing());
                }
                else
                {
                    AddBulletsToGun(gunIfException);
                }
            } 
            else if (giveBulletsToSpecificGun)
            {
                AddBulletsToGun(specificGun);
            }

            Player.playerWeapon.RefreshUIWeapon();

            Destroy(this.gameObject);
        }
    }

    void AddBulletsToGun (int gunNumber)
    {
        Bullets.SetBulletsToGun(gunNumber, Bullets.GetBulletsOfGun(gunNumber) + bulletsQuantity);
    }
}
