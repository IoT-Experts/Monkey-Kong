using UnityEngine;
using System.Collections.Generic;
public class IgnoreCollisions : MonoBehaviour
{

    public bool ignoreTriggersToo;
    public bool ignoreTag;
    public List<string> tagsToIgnore = new List<string>();
    public bool ignoreLayer;
    public List<int> layersToIgnore = new List<int>();

    void OnEnable()
    {
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.GetComponent<Collider2D>() != null)
            {
                if (ignoreLayer)
                {
                    if (layersToIgnore.Contains(obj.layer))
                    {
                        foreach (Collider2D collider2d_1 in obj.GetComponents<Collider2D>())
                            foreach (Collider2D collider2d_2 in this.GetComponents<Collider2D>())
                                if((ignoreTriggersToo) || (!ignoreTriggersToo && (!collider2d_1.isTrigger && !collider2d_2.isTrigger)))
                                    Physics2D.IgnoreCollision(collider2d_1, collider2d_2, true);
                    }
                }

                if (ignoreTag)
                {
                    if (tagsToIgnore.Contains(obj.tag))
                    {
                        foreach (Collider2D collider2d_1 in obj.GetComponents<Collider2D>())
                            foreach (Collider2D collider2d_2 in this.GetComponents<Collider2D>())
                                if((ignoreTriggersToo) || (!ignoreTriggersToo && (!collider2d_1.isTrigger && !collider2d_2.isTrigger)))
                                    Physics2D.IgnoreCollision(collider2d_1, collider2d_2, true);
                    }
                }
            }
        }
    }
}
