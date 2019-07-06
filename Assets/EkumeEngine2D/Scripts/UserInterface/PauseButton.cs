using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    [Header("This button pause/resume the game.")]
    [SerializeField] bool enableUIOnPause;
    [HideWhenFalse("enableUIOnPause")]
    [SerializeField] GameObject uIOfPause;
    [Space()]
    [SerializeField] bool pauseAllAudioSources;
    [HideWhenFalse("pauseAllAudioSources")]
    [SerializeField] List<AudioSource> audioExceptions = new List<AudioSource>();

    public static bool isPaused;

    void Awake ()
    {
        isPaused = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPaused)
        {
            if (uIOfPause != null)
                uIOfPause.SetActive(true);

            Time.timeScale = 0;
            isPaused = true;

            if (pauseAllAudioSources)
            {
                AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                if (pauseAllAudioSources)
                {
                    foreach (AudioSource audio in audioSources)
                    {
                        if(!audioExceptions.Contains(audio))
                            audio.Pause();
                    }
                }
            }
        }
        else
        {
            if(uIOfPause != null)
                uIOfPause.SetActive(false);

            Time.timeScale = 1;
            isPaused = false;

            if (pauseAllAudioSources)
            {
                AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                foreach (AudioSource audio in audioSources)
                {
                    if (!audioExceptions.Contains(audio))
                        audio.UnPause();
                }
            }
        }
    }
}
