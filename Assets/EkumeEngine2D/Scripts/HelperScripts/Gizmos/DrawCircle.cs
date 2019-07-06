#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
public class DrawCircle : MonoBehaviour
{
    [Header("Draw a circle in one position with X radius")]
    [SerializeField] Transform positionOfCircle;
    [SerializeField] float radius;
    [SerializeField] Color gizmosColor;


    void OnDrawGizmosSelected()
    {
        Handles.color = gizmosColor;
        Handles.DrawWireDisc(positionOfCircle.position, positionOfCircle.forward, radius);

    }
}
#endif
