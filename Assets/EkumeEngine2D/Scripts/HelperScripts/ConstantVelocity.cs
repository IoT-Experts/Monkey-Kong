using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantVelocity : MonoBehaviour
{
    [SerializeField] float velocityInX;
    [SerializeField] float velocityInY;
    Rigidbody2D thisRigidbody;

    void OnEnable ()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        thisRigidbody.velocity = new Vector3(velocityInX, velocityInY, 0);
    }
}
