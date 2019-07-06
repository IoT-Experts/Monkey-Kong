using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (Collider2D))]
public class AnimatorManagerOnTrigger : MonoBehaviour
{
    public List<string> parameterNames = new List<string>();
    public List<string> tags = new List<string>();

    public Animator currentAnimator;
    
    void Awake ()
    {
        currentAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(tags.Contains(other.tag))
        {
            currentAnimator.SetBool(parameterNames[tags.IndexOf(other.tag)], true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (tags.Contains(other.tag))
        {
            currentAnimator.SetBool(parameterNames[tags.IndexOf(other.tag)], false);
        }
    }
}