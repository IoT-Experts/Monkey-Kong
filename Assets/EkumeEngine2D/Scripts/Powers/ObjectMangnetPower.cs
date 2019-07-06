using UnityEngine;
using System.Collections.Generic;
using EkumeEnumerations;

public class ObjectMangnetPower : MonoBehaviour
{
    private static List<string> tagsToAtract;
    float velocity;
    List<Transform> objsToAttract = new List<Transform>();

#if UNITY_EDITOR
    void OnEnable()
    {
        if (GetComponent<Collider2D>() == null)
            Debug.LogError("The GameObject " + gameObject.name + " should have a Collider2D. Add a Collider2D to solve this problem.");

        if (GetComponent<Collider>() != null && !GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogError("The Collider2D of the GameObject " + gameObject.name + " should be of type trigger.");
        }

        if (GetComponent<Rigidbody2D>() == null)
            Debug.LogError("The GameObject " + gameObject.name + " should have a Rigidbody2D to dettects the collisions like an independent object. Add a Rigidbody2D and mark as Kinematic to solve this problem.");

        if (GetComponent<Rigidbody2D>() != null && !GetComponent<Rigidbody2D>())
            Debug.LogError("The Rigidbody2D of the GameObject " + gameObject.name + " should be marked as Kinematic to dettects the collisions like an independent object.");
 }
#endif

    void Start()
    {
        if (tagsToAtract == null)
            tagsToAtract = Player.playerPowers.tagsToAttract;

        velocity = Player.playerPowers.velocityToAttract;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerPowers.objectMangetActivated)
        {
            if (tagsToAtract.Contains(other.tag))
            {
                if (!objsToAttract.Contains(other.transform))
                {
                    objsToAttract.Add(other.transform);
                }
            }
        }
    }

    void Update()
    {
        if (objsToAttract.Count > 0)
        {
            for (int i = 0; i < objsToAttract.Count; i++)
            {
                if (objsToAttract[i] != null)
                {
                    objsToAttract[i].position = Vector3.MoveTowards(objsToAttract[i].position, transform.position, Time.deltaTime * velocity);
                    if (Vector3.Distance(transform.position, objsToAttract[i].position) < 0.05f)
                    {
                        Destroy(objsToAttract[i].gameObject);
                        objsToAttract.Remove(objsToAttract[i]);
                    }
                }
                else
                {
                    objsToAttract.Remove(objsToAttract[i]);
                }
            }
        }
    }
}
