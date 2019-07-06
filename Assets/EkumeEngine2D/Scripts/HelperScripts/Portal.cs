using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{

    [Header("Position to appear when enter in this portal")]
    public Transform positionToReapear;

    [SerializeField] List<string> tagsThatUsePortal;
    [SerializeField] bool keepPositionX;
    [SerializeField] bool keepPositionY;

    void OnTriggerEnter2D (Collider2D other)
    {
	    if (tagsThatUsePortal.Contains(other.tag))
        {
            other.transform.position = new Vector3 (
                (keepPositionX) ? other.transform.position.x : positionToReapear.position.x,
                (keepPositionY) ? other.transform.position.y : positionToReapear.position.y,
                other.transform.position.z);
        }
	}
}
