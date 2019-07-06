using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]
public class SoundOnTrigger : MonoBehaviour
{
    [SerializeField] bool findAudioSourceWithName;
    [HideWhenTrue("findAudioSourceWithName")]
    [SerializeField] AudioSource audioSource;
    [HideWhenFalse("findAudioSourceWithName")]
    [SerializeField] string objectWithAudioSource;

    [SerializeField] AudioClip clipToPlay;
    [Space()]
    [SerializeField] List<string> tagsThatCanActivateTheClip;

    void Start ()
    {
        if (findAudioSourceWithName)
        {
            GameObject go = GameObject.Find(objectWithAudioSource);
            if (go != null)
            {
                if(go.GetComponent<AudioSource>() != null)
                    audioSource = go.GetComponent<AudioSource>();
                else
                    Debug.LogError("The component " + GetType() + " can not find an AudioSource in the GameObject." + go.name +" please add an AudioSource to the GameObject.");
            }
            else
            {
                Debug.LogError("The component " + GetType() + " can not find the GameObject " + objectWithAudioSource);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if(tagsThatCanActivateTheClip.Contains(other.tag))
            audioSource.PlayOneShot(clipToPlay);
    }
 
}
