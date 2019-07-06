using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class ColorChanger_PlayerState : MonoBehaviour
{

    [SerializeField] PlayerStatesEnum stateActivator;
    [SerializeField] Color color1;
    [SerializeField] Color color2;
    [SerializeField] List<SpriteRenderer> spritesToChange = new List<SpriteRenderer>();
	[SerializeField] float lerpVelocity;

    List<Color> originalColors = new List<Color>();

    float timerOfLerp;

    bool equalToColor1 = true;
    bool equalToColor2;

    void Awake ()
    {
        for (int i = 0; i < spritesToChange.Count; i++)
        {
            originalColors.Add(spritesToChange[i].color);
        }
        
    }

    void Update ()
    {
        if (PlayerStates.GetPlayerStateValue(stateActivator))
        {
            timerOfLerp += Time.deltaTime * lerpVelocity;

            for (int i = 0; i < spritesToChange.Count; i++)
            {

                if (equalToColor1)
                    spritesToChange[i].color = Color.Lerp(color1, color2, timerOfLerp);
                else if (equalToColor2)
                    spritesToChange[i].color = Color.Lerp(color2, color1, timerOfLerp);

                if (spritesToChange[i].color == color1)
                {
                    equalToColor1 = true;
                    equalToColor2 = false;
                    timerOfLerp = 0;
                }
                else if (spritesToChange[i].color == color2)
                {
                    equalToColor2 = true;
                    equalToColor1 = false;
                    timerOfLerp = 0;
                }
            }
        }
        else
        {
            timerOfLerp = 0;
            for (int i = 0; i < spritesToChange.Count; i++)
            {
                spritesToChange[i].color = originalColors[i];
            }
        }
    }

}
