using UnityEngine;

public class BringToFront : MonoBehaviour
{
    void OnEnable()
    {
        // Move the transform to the end of the local transform list.
        // Most used in Canvas UI.
        transform.SetAsLastSibling();
    }
}