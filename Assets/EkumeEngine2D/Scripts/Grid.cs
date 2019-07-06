#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    [Range(0.05f, 100f)]
    public float width = 1.0f;
    [Range(0.05f, 100f)]
    public float height = 1.0f;

    public Color color = Color.white;
    public bool showGrid;

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            if (width > 0.05 && height > 0.05)
            {
                Vector3 pos = Camera.current.transform.position;
                Gizmos.color = color;

                for (float y = pos.y - 800.0f; y < pos.y + 800.0f; y += height)
                {
                    Gizmos.DrawLine(new Vector3(-1000000.0f, Mathf.Floor(y / height) * height, 0.0f),
                                    new Vector3(1000000.0f, Mathf.Floor(y / height) * height, 0.0f));
                }

                for (float x = pos.x - 1200.0f; x < pos.x + 1200.0f; x += width)
                {

                    Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, -1000000.0f, 0.0f),
                                    new Vector3(Mathf.Floor(x / width) * width, 1000000.0f, 0.0f));
                }
            }
        }
    }
}
#endif