﻿using UnityEngine;
using System.Collections;

public class InstantiateWithKey_OnCollision : MonoBehaviour
{
    public InputControlsManager inputControlsManager;
    public string tagThatActivates;
    public Transform positionToInstantiate;
    public GameObject gameObjectToInstantiate;
    public int inputControl;

    public bool destroyObjectWhenKeyIsRaised;
    public bool instantiateWhilePress;
    public float timeToReinstantiate;

    float timer;
    bool startTimer;

    GameObject lastInstantiatedObject;

    bool isInCollider;

    void OnCollisionEnter2D (Collision2D other)
    {
        if(other.collider.tag == tagThatActivates)
            isInCollider = true;      
    }

    void OnCollisionExit2D (Collision2D other)
    {
        if (other.collider.tag == tagThatActivates)
            isInCollider = false;
    }

	void Update ()
    {
        if (isInCollider)
        {
            if (InputControls.GetControlDown(inputControl))
            {
                InstantiateObject();

                if (instantiateWhilePress)
                    startTimer = true;
            }

            if (InputControls.GetControlUp(inputControl))
            {
                if (instantiateWhilePress)
                    startTimer = false;

                if (destroyObjectWhenKeyIsRaised && lastInstantiatedObject != null)
                    Destroy(lastInstantiatedObject);

            }

            if (instantiateWhilePress && startTimer)
            {
                timer += Time.deltaTime;

                if (timer >= timeToReinstantiate)
                {
                    InstantiateObject();
                    timer = 0;
                }
            }
        }
	}

    void InstantiateObject ()
    {
        lastInstantiatedObject = Instantiate(gameObjectToInstantiate, positionToInstantiate.transform.position, positionToInstantiate.transform.rotation) as GameObject;
    }
}
