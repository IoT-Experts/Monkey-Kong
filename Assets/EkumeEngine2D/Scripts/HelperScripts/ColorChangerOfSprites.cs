using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorChangerOfSprites : MonoBehaviour {

    [SerializeField] enum ActivatorOfAction { OnTriggerEnter2D, OnCollisionEnter2D, WhenTheObjectStart }
    [SerializeField] ActivatorOfAction activatorOfAction;
    
    [SerializeField] Color newColor = Color.white;
    [SerializeField] List<SpriteRenderer> spritesToChange = new List<SpriteRenderer>();
	[SerializeField] bool lerpColor;

    [HideWhenFalse("lerpColor")]
    [SerializeField] float velocityToChangeColor;

    bool startToLerp;

    void Start ()
    {
        if (activatorOfAction == ActivatorOfAction.WhenTheObjectStart)
            ColorChange();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activatorOfAction == ActivatorOfAction.OnTriggerEnter2D)
                ColorChange();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            if (activatorOfAction == ActivatorOfAction.OnCollisionEnter2D)
                ColorChange();
        }
    }

   public void ColorChange ()
    {
        if (!lerpColor)
        {
            for (int i = 0; i < spritesToChange.Count; i++)
            {
                spritesToChange[i].color = newColor;
            }
        }
        else
        {
            startToLerp = true;
        }
    }

    void Update ()
    {
        if (startToLerp)
        {
            for (int i = 0; i < spritesToChange.Count; i++)
            {
                spritesToChange[i].color = Color.Lerp(spritesToChange[i].color, newColor, Time.deltaTime * velocityToChangeColor);
            }

            if (spritesToChange[spritesToChange.Count-1].color == newColor)
            {
                startToLerp = false;
            }
        }
    }
}
