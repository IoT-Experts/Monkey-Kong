using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEngine.UI;
using EkumeSavedData.Player;

[RequireComponent(typeof(Text))]
public class ShowQuantityOfPower : MonoBehaviour
{
    public PowersEnum powerToShowQuantity;
    public string format = "0";
    Text thisText;

	void Start ()
    {
        thisText = GetComponent<Text>();
        StartCoroutine("UpdateUI");
    }	

    IEnumerator UpdateUI()
    {
        for (;;)
        {
            thisText.text = PowerStats.GetQuantityOfPower(powerToShowQuantity).ToString(" " + format);
            yield return new WaitForSeconds(0.33f);
        }
    }
}
