using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class AudioSourceConfig
{
    public enum DisabledOrEnabled { On, Off }

    public string audioSourceName = "NAME";
    public AudioSource audioFound;
    public float defaultVolume = 1;
    public float maxVolume = 1;
    public DisabledOrEnabled defaultAudioValueForButton; 

    public AudioSourceConfig (string _audioSourceName, AudioSource _audioFound, float _defaultVolume, float _maxVolume, DisabledOrEnabled _defaultAudioValueForButton)
    {
        audioSourceName = _audioSourceName;
        audioFound = _audioFound;
        defaultVolume = _defaultVolume;
        maxVolume = _maxVolume;
        defaultAudioValueForButton = _defaultAudioValueForButton;
    }
}

[System.Serializable]
public class AudioCategory
{
    public enum AudioConfigOptionsOfUI { SliderForSoundBar, ButtonToTurnOnAndOff };
    public enum ButtonSwitchOptions { ChangeSpriteOfButton, ChangeTextOfButton, None }
    public enum WhatAudioSourcesInclude { SpecificAudioSources, includeAllAudioSourcesNotAddedInTheOtherCategories, includeAllAudioSourcesOfTheScene }

    public string categoryName = "CATEGORY NAME";
    public AudioConfigOptionsOfUI audioConfigOptionsOfUI;
    public Slider sliderUI;

    public ButtonSwitchOptions buttonSwitchOptions;
    public Button button;
    public Sprite spriteInOn;
    public Sprite spriteInOff;
    public Text textOfButton;
    public string textInOff = "Off";
    public string textInOn = "On";

    public WhatAudioSourcesInclude audioSourcesToInclude;

    public List<AudioSourceConfig> audioSourcesDataConfig = new List<AudioSourceConfig>();
}

public class AudioConfigurations : MonoBehaviour
{
#if UNITY_EDITOR
    public bool isSceneWithConfigurations = true;
#endif

    public bool keepObjectInOtherScenes = true;

    public List<AudioCategory> categories = new List<AudioCategory>();

    string originalScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        originalScene = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += SceneChanged;

        AudioConfigurations[] audioConfigs = FindObjectsOfType<AudioConfigurations>();
        if (audioConfigs.Length > 1)
        {
            foreach(AudioConfigurations audioConfig in audioConfigs)
            {
                if(audioConfig.gameObject != this.gameObject)
                  Destroy(audioConfig.gameObject);
            }            
        }
    }

    bool AudioSourceConfigContainsString (List<AudioSourceConfig> audioSourceConfig, string textToSearch)
    {
        bool valueToReturn = false;
        foreach(AudioSourceConfig audioConf in audioSourceConfig)
        {
            if (audioConf.audioSourceName == textToSearch)
            {
                valueToReturn = true;
                break;
            }
        }
        return valueToReturn;
    }

    bool AudioSourceNameIsInAnyCategory(string audioSourceName)
    {
        bool valueToReturn = false;
        foreach(AudioCategory categ in categories)
        {
            if(AudioSourceConfigContainsString(categ.audioSourcesDataConfig, audioSourceName))
            {
                valueToReturn = true;
                break;
            }
        }
        return valueToReturn;
    }

    void Start()
    {
        AudioSource[] audioSourcesOfScene = FindObjectsOfType<AudioSource>();

        //START includeAllAudioSourcesNotAddedInTheOtherCategories <----------------------------------------------------------------------------
        foreach (AudioCategory category in categories)
        {
            if (category.audioSourcesToInclude == AudioCategory.WhatAudioSourcesInclude.includeAllAudioSourcesNotAddedInTheOtherCategories)
            {
                foreach (AudioSource audioSourceOfScene in audioSourcesOfScene)
                {
                    if (!AudioSourceNameIsInAnyCategory(audioSourceOfScene.gameObject.name))
                    {
                        category.audioSourcesDataConfig.Add(new AudioSourceConfig(audioSourceOfScene.gameObject.name, audioSourceOfScene, 1, 1, AudioSourceConfig.DisabledOrEnabled.On));
                    }
                }
            }
        }
        //FINISH includeAllAudioSourcesNotAddedInTheOtherCategories <----------------------------------------------------------------------------

        foreach (AudioCategory category in categories)
        {
            if (category.audioSourcesToInclude == AudioCategory.WhatAudioSourcesInclude.includeAllAudioSourcesOfTheScene)
            {
                foreach (AudioSource audioSource in audioSourcesOfScene)
                {
                    if(!AudioSourceConfigContainsString(category.audioSourcesDataConfig, audioSource.gameObject.name))
                        category.audioSourcesDataConfig.Add(new AudioSourceConfig(audioSource.gameObject.name, audioSource, 1, 1, AudioSourceConfig.DisabledOrEnabled.On));
                }
            }

            foreach (AudioSourceConfig audioSourceData in category.audioSourcesDataConfig) //Foreach audioSourceData in the array in category.audioSourcesDataConfig
            {
                if (category.audioSourcesToInclude == AudioCategory.WhatAudioSourcesInclude.SpecificAudioSources)
                {
                    GameObject gameObjectWithAudio = GameObject.Find(audioSourceData.audioSourceName);
                    audioSourceData.audioFound = null;
                    if (gameObjectWithAudio != null && gameObjectWithAudio.GetComponent<AudioSource>() != null)
                    {
                        audioSourceData.audioFound = gameObjectWithAudio.GetComponent<AudioSource>();
                    }
                }

                if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar)
                {
                    if (audioSourceData.audioFound != null)
                    {
                        if (!PlayerPrefs.HasKey("sliderValue" + category.categoryName))
                        {
                            audioSourceData.audioFound.volume = audioSourceData.defaultVolume;
                        }
                        else
                        {
                            audioSourceData.audioFound.volume = PlayerPrefs.GetFloat("sliderValue" + category.categoryName);
                        }
                    }
                }
            }

            if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar)
            {
                if (!PlayerPrefs.HasKey("sliderValue" + category.categoryName))
                {
                    PlayerPrefs.SetFloat("sliderValue" + category.categoryName, 1);
                    if (category.sliderUI != null)
                        category.sliderUI.value = 1;
                }
                else
                {
                    if (category.sliderUI != null)
                    {
                        category.sliderUI.value = PlayerPrefs.GetFloat("sliderValue" + category.categoryName);
                    }
                }
            }

            if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar) //If the category has the configuration by Slider
            {
                if (category.sliderUI != null)
                    category.sliderUI.onValueChanged.AddListener(delegate { SliderValueChangedCheck(); });
            }
            else if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.ButtonToTurnOnAndOff) //If the category has the configuration by Button
            {
                if (category.button != null)
                {
                    AudioConfigurations_Button audConf_Button = category.button.gameObject.AddComponent<AudioConfigurations_Button>(); //The button will do every action about the sounds and initialization.
                    audConf_Button.audioCategory = category;
                }
            }
        }

        SliderValueChangedCheck();

        if (FindObjectOfType<AudioConfigurations_Button>() == null)
            CheckValuesOfButtons();
    }

    void SliderValueChangedCheck()
    {
        foreach (AudioCategory category in categories)
        {
            if (category.sliderUI != null
                && category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar)
            {
                foreach (AudioSourceConfig audioSourceData in category.audioSourcesDataConfig)
                {
                    if (audioSourceData.audioFound != null)
                    {
                        audioSourceData.audioFound.volume = audioSourceData.maxVolume * category.sliderUI.value;
                    }
                }
                PlayerPrefs.SetFloat("sliderValue" + category.categoryName, category.sliderUI.value);
            }
        }
    }

    void CheckValuesOfButtons()
    {
        foreach (AudioCategory category in categories) //Foreach category in the List of categories
        {
            foreach (AudioSourceConfig audioSourceData in category.audioSourcesDataConfig)
            {
                if (audioSourceData.audioFound != null)
                {
                    if (!PlayerPrefs.HasKey("audioButtonValue" + category.categoryName))
                    {
                        if (audioSourceData.defaultAudioValueForButton == AudioSourceConfig.DisabledOrEnabled.On)
                        {
                            audioSourceData.audioFound.enabled = true;
                            PlayerPrefs.SetInt("audioButtonValue" + category.categoryName, 1);
                        }
                        else
                        {
                            audioSourceData.audioFound.enabled = false;
                            PlayerPrefs.SetInt("audioButtonValue" + category.categoryName, 0);
                        }
                    }
                    else
                    {
                        if (PlayerPrefs.GetInt("audioButtonValue" + category.categoryName) == 1)
                        {
                            audioSourceData.audioFound.enabled = true;
                        }
                        else
                        {
                            audioSourceData.audioFound.enabled = false;
                        }
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // unsubscribe
    }

    void SceneChanged(Scene previousScene, Scene newScene)
    {
        if(originalScene != newScene.name)
            Start();
    }   
}
