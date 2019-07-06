using UnityEngine;
using System.Collections;
using EkumeEnumerations;
public class PowerByDefault : MonoBehaviour {

    [Header("The player will use this power by all the level")]
    [SerializeField] PowersEnum power;

	void Start ()
    {
        Player.playerPowers.CallPower(power, true, false, false, 0);
	}
	

}
