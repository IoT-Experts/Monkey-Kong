using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script should be added to the platform that transports to the player.
[RequireComponent(typeof(Collider2D))]
public class MovingPlatform : MonoBehaviour
{
    Dictionary<GameObject, Quaternion> originalRotationsOfObjects = new Dictionary<GameObject, Quaternion>();
    [SerializeField] bool keepOriginalRotations = true;

    void OnCollisionEnter2D (Collision2D other)
    {
        other.transform.SetParent(gameObject.transform);
        if(!originalRotationsOfObjects.ContainsKey(other.gameObject))
            originalRotationsOfObjects.Add(other.gameObject, other.transform.rotation);
    }

    void OnCollisionExit2D (Collision2D other)
    {
        if(other.transform.parent != null)
            other.transform.SetParent(null);

        if (originalRotationsOfObjects.ContainsKey(other.gameObject))
        {
            other.transform.rotation = originalRotationsOfObjects[other.gameObject];
            originalRotationsOfObjects.Remove(other.gameObject);
        }
    }

    void OnCollisionStay2D (Collision2D other)
    {
        if (keepOriginalRotations && originalRotationsOfObjects.ContainsKey(other.gameObject))
        {
            other.transform.eulerAngles = originalRotationsOfObjects[other.gameObject].eulerAngles;
        }
    }
}
