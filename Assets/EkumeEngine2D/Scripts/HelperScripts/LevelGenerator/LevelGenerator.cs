using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour
{
    enum Order { Aleatory, OrderOfList }
    enum DirectionToGenerate { Right, Left, Up, Down }
    [SerializeField] Transform limitToMove;
    Transform player;
    [Space]
    [SerializeField]  DirectionToGenerate directionToGenerate;
    [Space]
    [SerializeField] List<GameObject> objectsToInstantiate = new List<GameObject>();
    [Space]
    [SerializeField] Order orderToInstantiate;
    
    [Header("Aleatory distance to instantiate")]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    float randomDistance;
    GameObject lastInstantiatedObject;
    int instantiatedPart;

    void Start ()
    {
        player = FindObjectOfType<Player>().transform;
        randomDistance = 0;
        lastInstantiatedObject = this.gameObject;
        instantiatedPart = 0;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (limitToMove != null)
        {
            Gizmos.color = Color.magenta;
            if (directionToGenerate == DirectionToGenerate.Right || directionToGenerate == DirectionToGenerate.Left)
                Gizmos.DrawLine(new Vector3(limitToMove.position.x, limitToMove.position.y + 1, limitToMove.position.z), new Vector3(limitToMove.position.x, limitToMove.position.y - 1, limitToMove.position.z));
            else
                Gizmos.DrawLine(new Vector3(limitToMove.position.x + 1, limitToMove.position.y, limitToMove.position.z), new Vector3(limitToMove.position.x - 1, limitToMove.position.y, limitToMove.position.z));

            if (directionToGenerate == DirectionToGenerate.Right)
            {
                Gizmos.DrawLine(new Vector3(limitToMove.position.x + 0.3f, limitToMove.position.y, limitToMove.position.z), new Vector3(limitToMove.position.x + 0.1f, limitToMove.position.y + 0.1f, limitToMove.position.z));
                Gizmos.DrawLine(new Vector3(limitToMove.position.x + 0.3f, limitToMove.position.y, limitToMove.position.z), new Vector3(limitToMove.position.x + 0.1f, limitToMove.position.y - 0.1f, limitToMove.position.z));
            }
            else if (directionToGenerate == DirectionToGenerate.Left)
            {
                Gizmos.DrawLine(new Vector3(limitToMove.position.x - 0.3f, limitToMove.position.y, limitToMove.position.z), new Vector3(limitToMove.position.x - 0.1f, limitToMove.position.y + 0.1f, limitToMove.position.z));
                Gizmos.DrawLine(new Vector3(limitToMove.position.x - 0.3f, limitToMove.position.y, limitToMove.position.z), new Vector3(limitToMove.position.x - 0.1f, limitToMove.position.y - 0.1f, limitToMove.position.z));
            }
            else if (directionToGenerate == DirectionToGenerate.Up)
            {
                Gizmos.DrawLine(new Vector3(limitToMove.position.x, limitToMove.position.y + 0.3f, limitToMove.position.z), new Vector3(limitToMove.position.x + 0.1f, limitToMove.position.y + 0.1f, limitToMove.position.z));
                Gizmos.DrawLine(new Vector3(limitToMove.position.x, limitToMove.position.y + 0.3f, limitToMove.position.z), new Vector3(limitToMove.position.x - 0.1f, limitToMove.position.y + 0.1f, limitToMove.position.z));
            }
            else if (directionToGenerate == DirectionToGenerate.Down)
            {
                Gizmos.DrawLine(new Vector3(limitToMove.position.x, limitToMove.position.y - 0.3f, limitToMove.position.z), new Vector3(limitToMove.position.x + 0.1f, limitToMove.position.y - 0.1f, limitToMove.position.z));
                Gizmos.DrawLine(new Vector3(limitToMove.position.x, limitToMove.position.y - 0.3f, limitToMove.position.z), new Vector3(limitToMove.position.x - 0.1f, limitToMove.position.y - 0.1f, limitToMove.position.z));
            }
        }
    }
#endif

    void Update ()
    {
        if((directionToGenerate == DirectionToGenerate.Right && player.position.x > limitToMove.position.x)
            || (directionToGenerate == DirectionToGenerate.Left && player.position.x < limitToMove.position.x)
            || (directionToGenerate == DirectionToGenerate.Up && player.position.y > limitToMove.position.y)
            || (directionToGenerate == DirectionToGenerate.Down && player.position.y < limitToMove.position.y))
        {
            float lastDistance = Vector2.Distance(transform.position, lastInstantiatedObject.transform.position);

            if (directionToGenerate == DirectionToGenerate.Right || directionToGenerate == DirectionToGenerate.Left)
                transform.position = new Vector3(player.position.x - ((limitToMove.position.x - transform.position.x)), transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, player.position.y - ((limitToMove.position.y - transform.position.y)), transform.position.z);

            if (lastDistance >= randomDistance)
            {
                int randomPart = Random.Range(0, objectsToInstantiate.Count);
                lastInstantiatedObject =  Instantiate(objectsToInstantiate[(orderToInstantiate == Order.Aleatory) ? randomPart : instantiatedPart], transform.position, objectsToInstantiate[(orderToInstantiate == Order.Aleatory) ? randomPart : instantiatedPart].transform.rotation) as GameObject;
                randomDistance = Random.Range(minDistance, maxDistance);
                instantiatedPart++;

                if (instantiatedPart >= objectsToInstantiate.Count)
                    instantiatedPart = 0;
            }
        }
    }
}
