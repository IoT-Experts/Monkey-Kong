#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
public class DrawLineGizmosFromThis : MonoBehaviour
{

    [SerializeField] bool drawInX = true;
    [SerializeField] bool drawInY;
    [SerializeField] float sizeOfLine;
    [SerializeField] Color gizmosColor;
    [Header("Type of line")]
    [SerializeField] bool dottedLine;
    [Header("If you want to always see this gizmo")]
    [SerializeField] bool alwaysSeeing;

    void OnDrawGizmos()
    {
        if (alwaysSeeing)
        {
            DrawTheGizmo();
        }
    }

    void OnDrawGizmosSelected ()
    {
        if (!alwaysSeeing)
        {
            DrawTheGizmo();
        }
    }

    void DrawTheGizmo ()
    {
        Vector3 endPosition = Vector3.zero;

        if (drawInX)
            endPosition = new Vector3(transform.position.x + sizeOfLine, transform.position.y, transform.position.z);
        else if (drawInY)
            endPosition = new Vector3(transform.position.x, transform.position.y + sizeOfLine, transform.position.z);

        if (dottedLine)
        {
            Handles.color = gizmosColor;
            Handles.DrawDottedLine(transform.position, endPosition, 3.5f);
        }
        else
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(transform.position, endPosition);
        }
    }
}
#endif
