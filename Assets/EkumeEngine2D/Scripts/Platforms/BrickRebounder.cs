using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Collider2D))]
public class BrickRebounder : MonoBehaviour
{
    
    enum AxisToMove { X, Y}

    [SerializeField] Transform objectToMove;
    [SerializeField] float quantityToMove;
    [SerializeField] AxisToMove axisToMove;
    [SerializeField] float velocityToMove;

    bool startMovement;
    Vector3 originalPos;

    bool isGoingToDown;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Mount")
        {
            startMovement = true;
            originalPos = objectToMove.position;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void FixedUpdate ()
    {
        if (startMovement)
        {
            if (((axisToMove == AxisToMove.X) ? objectToMove.position.x : objectToMove.position.y) <
                ((axisToMove == AxisToMove.X) ? originalPos.x + quantityToMove : originalPos.y + quantityToMove) && !isGoingToDown)
            {
                objectToMove.position = new Vector3((axisToMove == AxisToMove.X) ? objectToMove.position.x + 0.1f * velocityToMove : objectToMove.position.x, (axisToMove == AxisToMove.Y) ? objectToMove.position.y + 0.1f * velocityToMove : objectToMove.position.y, objectToMove.position.z);
            }
            else
            {
                isGoingToDown = true;
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, originalPos, Time.deltaTime * velocityToMove);

                if (Vector2.Distance(objectToMove.position, originalPos) < 0.05)
                {
                    objectToMove.position = originalPos;
                    isGoingToDown = false;
                    startMovement = false;
                    GetComponent<Collider2D>().enabled = true;
                }
            }
        }
    }
}
