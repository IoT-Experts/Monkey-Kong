using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AddForceToUp : MonoBehaviour
{

    [Header("The object with rigidbody will jump")]
    [SerializeField] float jumpForceToAdd = 20;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.GetComponent<Rigidbody2D>() != null)
        {
            other.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(other.collider.GetComponent<Rigidbody2D>().velocity.x, jumpForceToAdd);
        }
    }
}
