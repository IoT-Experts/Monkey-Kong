using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using EkumeSavedData.WeaponInventory;

[RequireComponent(typeof(Collider2D))]
public class ObtainWeapon_OnTrigger : MonoBehaviour
{
    public WeaponFactory weaponFactory;
    public int gunToObtain;
    public bool saveInWeaponInventory;
    public bool placeToPlayer;
    public bool useOnlyForThisLevel;
    [Header("What to do if player have this gun")]
    public bool addBulletsIfItHaveTheGun;
    public bool restartBulletsIfItHaveTheGun;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            PlayerStates.SetStateValueForAllAttackCategories(false);

            if (Weapons.IsWeaponInInventory(gunToObtain) || Weapons.GetWeaponThatIsUsing() == gunToObtain)
            {
                if (addBulletsIfItHaveTheGun)
                {
                    Bullets.SetBulletsToGun(gunToObtain,
                        Bullets.GetBulletsOfGun(gunToObtain) + WeaponFactory.instance.weaponsData[gunToObtain].defaultBulletQuantity);
                }

                if(restartBulletsIfItHaveTheGun)
                {
                    Bullets.SetBulletsToGun(gunToObtain,
                        WeaponFactory.instance.weaponsData[gunToObtain].defaultBulletQuantity);
                }
            }

            if (saveInWeaponInventory)
                Weapons.SetWeaponToInventory(gunToObtain);

            if (placeToPlayer)
            {
                Player.playerWeapon.ChangeWeapon(gunToObtain);
            }

            if (!useOnlyForThisLevel)
                Weapons.SetWeaponThatIsUsing(gunToObtain);

            Destroy(this.gameObject);
        }
    }

}
