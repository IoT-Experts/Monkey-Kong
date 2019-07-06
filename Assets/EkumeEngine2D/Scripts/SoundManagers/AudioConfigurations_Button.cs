using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This script is added to the buttons in the component AudioConfigurations.
//You should not use this script in any object of your scene. This script is added automatically.
[RequireComponent(typeof(Button))]
public class AudioConfigurations_Button : MonoBehaviour, IPointerClickHandler
{
    public AudioCategory audioCategory;
    
    void Start()
    {
        foreach (AudioSourceConfig audioSourceData in audioCategory.audioSourcesDataConfig)
        {
            if (!PlayerPrefs.HasKey("audioButtonValue" + audioCategory.categoryName))
            {
                if (audioSourceData.defaultAudioValueForButton == AudioSourceConfig.DisabledOrEnabled.On)
                {
                    audioSourceData.audioFound.enabled = true;
                    PlayerPrefs.SetInt("audioButtonValue" + audioCategory.categoryName, 1);
                    ButtonSwitchUI(true);
                }
                else
                {
                    audioSourceData.audioFound.enabled = false;
                    PlayerPrefs.SetInt("audioButtonValue" + audioCategory.categoryName, 0);
                    ButtonSwitchUI(false);
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("audioButtonValue" + audioCategory.categoryName) == 1)
                {
                    audioSourceData.audioFound.enabled = true;
                    ButtonSwitchUI(true);
                }
                else
                {
                    audioSourceData.audioFound.enabled = false;
                    ButtonSwitchUI(false);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (AudioSourceConfig audioSourceData in audioCategory.audioSourcesDataConfig)
        {
            if(audioSourceData.audioFound.enabled == true)
            {
                PlayerPrefs.SetInt("audioButtonValue" + audioCategory.categoryName, 0);
                audioSourceData.audioFound.enabled = false;
                ButtonSwitchUI(false);                
            }
            else
            {
                PlayerPrefs.SetInt("audioButtonValue" + audioCategory.categoryName, 1);
                audioSourceData.audioFound.enabled = true;
                ButtonSwitchUI(true);
            }
        }
    }

    void ButtonSwitchUI(bool buttonIsOn)
    {
        if (audioCategory.button != null && audioCategory.buttonSwitchOptions == AudioCategory.ButtonSwitchOptions.ChangeSpriteOfButton)
        {
            if(buttonIsOn)
                audioCategory.button.image.sprite = audioCategory.spriteInOn;
            else
                audioCategory.button.image.sprite = audioCategory.spriteInOff;
        }
        else if (audioCategory.textOfButton != null && audioCategory.buttonSwitchOptions == AudioCategory.ButtonSwitchOptions.ChangeTextOfButton)
        {
            if (buttonIsOn)
                audioCategory.textOfButton.text = audioCategory.textInOn;
            else
                audioCategory.textOfButton.text = audioCategory.textInOff;
        }
    }

}
