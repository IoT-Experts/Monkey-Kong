using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This component should be added to a GameObject with the component Button in the UI.
[RequireComponent(typeof(Button))]
public class SoundOnPressButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool findAnyAudioSource;
    [HideWhenTrue("findAnyAudioSource")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clipToPlay;

    private bool wasAsigned;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!wasAsigned && findAnyAudioSource)
        {
            if (FindObjectOfType<AudioSource>() != null)
                audioSource = FindObjectOfType<AudioSource>();
            else
                Debug.LogError("The component " + GetType() + " can not find an AudioSource in the scene. Please add an AudioSource to some GameObject.");

            wasAsigned = true;
        }

        if (audioSource != null)
            audioSource.PlayOneShot(clipToPlay);

    }
}
