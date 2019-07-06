using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleDetectorForAIEnemy : MonoBehaviour
{
    [Header("This component requires a collider of type trigger")]
    [SerializeField] List<string> layersToDetect = new List<string>();
    [SerializeField] List<string> tagsToDetect = new List<string>();

    AIEnemyMovement iAEnemy;

    void Start ()
    {
        iAEnemy = transform.root.GetComponent<AIEnemyMovement>();
    }

#if UNITY_EDITOR
    void Awake ()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            if (!GetComponent<Rigidbody2D>())
            {
                if (transform.parent == null)
                {
                    Debug.LogWarning("The Rigidbody2D with the component TargetDetector should be marked as Kinematic. Please mark as kinematic the Rigidbody2D of the GameObject " + gameObject.name);
                }
                else
                {
                    Debug.LogWarning("The Rigidbody2D with the component TargetDetector should be marked as Kinematic. Please mark as kinematic the Rigidbody2D of the GameObject " + gameObject.name + " (His parent is: " + transform.parent.name + ")");
                }
            }
        }

        if(!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogError("The Collider2D of the object " + gameObject.name + " is not of type trigger for the component " + GetType().Name);
        }
    }
#endif

    void OnTriggerStay2D(Collider2D other)
    {
        if (layersToDetect.Contains(LayerMask.LayerToName(other.gameObject.layer)) || tagsToDetect.Contains(other.gameObject.tag))
        {
            iAEnemy.ObstacleDetection(other);
        }
    }
}
