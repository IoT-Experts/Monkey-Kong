using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEngine.UI;
using EkumeSavedData.Player;

public class ShowMaxTimeOfPower : MonoBehaviour
{

    public PowersEnum powerToShowTime;
    public string format = "0";

    Text thisText;

    void Start()
    {
        thisText = GetComponent<Text>();

        StartCoroutine("UpdateUI");
    }

    IEnumerator UpdateUI()
    {
        for (;;)
        {
            thisText.text = PowerStats.GetTimeOfPower(powerToShowTime).ToString(" " + format);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
