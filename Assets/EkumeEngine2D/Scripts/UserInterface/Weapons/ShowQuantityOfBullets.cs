using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEngine.UI;
using EkumeSavedData.WeaponInventory;
using EkumeLists;

[RequireComponent(typeof(Text))]
public class ShowQuantityOfBullets : MonoBehaviour
{
    public ListOfWeapons weaponToShowBullets;
    public string format = "0";
    public string textIfInfiniteBullets = "inf";
    Text thisText;

    void OnEnable()
    {
        thisText = GetComponent<Text>();
        StartCoroutine("RefreshUI");
    }

    IEnumerator RefreshUI()
    {
        for (;;)
        {
            UpdateUI();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void UpdateUI()
    {
        if (!WeaponFactory.instance.weaponsData[weaponToShowBullets.Index].infiniteBullets)
        {
            thisText.text = Bullets.GetBulletsOfGun(weaponToShowBullets.Index).ToString(" "+format);
        }
        else
        {
            thisText.text = textIfInfiniteBullets;
        }
    }
}
