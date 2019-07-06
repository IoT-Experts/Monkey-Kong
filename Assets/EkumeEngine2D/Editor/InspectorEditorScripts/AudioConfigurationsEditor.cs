using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(AudioConfigurations))]
public class AudioConfigurationsEditor : Editor
{
    AudioConfigurations audioConfigurations;

    void OnEnable()
    {
        audioConfigurations = (AudioConfigurations)target;
    }

    public override void OnInspectorGUI()
    {
        if (audioConfigurations.GetComponents<Component>().Length > 2)
        {
            EditorGUILayout.HelpBox("Is recommendable put this component in a GameObject without more components.", MessageType.Info);
        }

        if(audioConfigurations.transform.childCount > 0)
        {
            EditorGUILayout.HelpBox("Is recommendable not add child to this GameObject while you are using this component.", MessageType.Info);
        }

        AudioConfigurations[] audioConfigs = FindObjectsOfType<AudioConfigurations>();
        if (audioConfigs.Length > 1)
        {
            EditorGUILayout.HelpBox("You have multiple AudioConfigurations in your scene. You can, and you should to use only one for all the configurations.", MessageType.Error);
        }

        GUI.contentColor = Color.cyan;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUIUtility.labelWidth = 175;
            audioConfigurations.isSceneWithConfigurations = EditorGUILayout.Toggle("It's scene with configurations: ", audioConfigurations.isSceneWithConfigurations);
        }
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            audioConfigurations.keepObjectInOtherScenes = EditorGUILayout.Toggle("Keep object in other scenes: ", audioConfigurations.keepObjectInOtherScenes);
            GUI.contentColor = Color.white;
            if(audioConfigurations.keepObjectInOtherScenes)
            {
                EditorGUILayout.HelpBox("This object will be deleted in the scenes who have the component 'AudioConfigurations' to load its corresponding configurations.", MessageType.Info);
            }
        }
        EditorGUILayout.HelpBox("Is recommendable to put the same configurations for each scene with this component to load the same Audio Configurations.", MessageType.None);
        EditorGUIUtility.labelWidth = 135;
        foreach (AudioCategory category in audioConfigurations.categories)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(category.categoryName, EditorStyles.boldLabel);

                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
                {
                    audioConfigurations.categories.Remove(category);
                    return;
                }

                EditorGUILayout.EndHorizontal();

                using (new GUILayout.VerticalScope("box"))
                {
                    category.categoryName = EditorGUILayout.TextField("Category name: ", category.categoryName);
                    if (audioConfigurations.isSceneWithConfigurations)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            category.audioConfigOptionsOfUI = (AudioCategory.AudioConfigOptionsOfUI)EditorGUILayout.EnumPopup("Configuration type: ", category.audioConfigOptionsOfUI);

                            if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar)
                            {
                                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                                {
                                    category.sliderUI = EditorGUILayout.ObjectField("Slider: ", category.sliderUI, typeof(Slider), true) as Slider;
                                }
                            }

                            if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.ButtonToTurnOnAndOff)
                            {
                                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                                {
                                    category.button = EditorGUILayout.ObjectField("Button: ", category.button, typeof(Button), true) as Button;
                                    EditorGUIUtility.labelWidth = 155;
                                    category.buttonSwitchOptions = (AudioCategory.ButtonSwitchOptions)EditorGUILayout.EnumPopup("What to change in on/off: ", category.buttonSwitchOptions);
                                    EditorGUIUtility.labelWidth = 135;

                                    if (category.buttonSwitchOptions != AudioCategory.ButtonSwitchOptions.None)
                                    {
                                        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                                        {
                                            if (category.buttonSwitchOptions == AudioCategory.ButtonSwitchOptions.ChangeSpriteOfButton)
                                            {
                                                category.spriteInOn = EditorGUILayout.ObjectField("New image in On: ", category.spriteInOn, typeof(Sprite), true) as Sprite;
                                                category.spriteInOff = EditorGUILayout.ObjectField("New image in Off: ", category.spriteInOff, typeof(Sprite), true) as Sprite;
                                            }
                                            else if (category.buttonSwitchOptions == AudioCategory.ButtonSwitchOptions.ChangeTextOfButton)
                                            {
                                                category.textOfButton = EditorGUILayout.ObjectField("Text of button: ", category.textOfButton, typeof(Text), true) as Text;
                                                category.textInOn = EditorGUILayout.TextField("New text in On: ", category.textInOn);
                                                category.textInOff = EditorGUILayout.TextField("New text in Off: ", category.textInOff);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                using (new GUILayout.VerticalScope("box"))
                {
                    using (new GUILayout.VerticalScope("Audio Sources", "Window"))
                    {
                        EditorGUIUtility.labelWidth = 150;
                        category.audioSourcesToInclude = (AudioCategory.WhatAudioSourcesInclude)EditorGUILayout.EnumPopup("Audio Sources to include: ", category.audioSourcesToInclude);

                        if(category.audioSourcesToInclude != AudioCategory.WhatAudioSourcesInclude.SpecificAudioSources)
                            EditorGUILayout.HelpBox("You can add specific AudioSources to define its specific configurations, but when the scene starts will be added the rest of AudioSources.", MessageType.Info);

                        EditorGUIUtility.labelWidth = 135;
                    }

                    if (audioConfigurations.keepObjectInOtherScenes)
                        EditorGUILayout.HelpBox("Here you should to add all the audio source names, even those who are not in this scene, in this way, the configurations will be applied when the scene changes.", MessageType.None);

                    foreach (AudioSourceConfig audioSourceData in category.audioSourcesDataConfig)
                    {
                        EditorGUILayout.BeginHorizontal();
                        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            audioSourceData.audioSourceName = EditorGUILayout.TextField("Audio Source Name: ", audioSourceData.audioSourceName);

                            if (category.audioConfigOptionsOfUI == AudioCategory.AudioConfigOptionsOfUI.SliderForSoundBar)
                            {
                                audioSourceData.defaultVolume = EditorGUILayout.Slider("Default Volume: ", audioSourceData.defaultVolume, 0, 1);
                                audioSourceData.maxVolume = EditorGUILayout.Slider("Max Volume: ", audioSourceData.maxVolume, 0, 1);
                            }
                            else
                            {
                                audioSourceData.defaultAudioValueForButton = (AudioSourceConfig.DisabledOrEnabled)EditorGUILayout.EnumPopup("Default value: ", audioSourceData.defaultAudioValueForButton);
                            }

                            if (GameObject.Find(audioSourceData.audioSourceName) == null)
                            {
                                EditorGUILayout.HelpBox("The GameObject " + audioSourceData.audioSourceName + " does not exist in this scene, it will be ignored when the scene starts" + ((audioConfigurations.keepObjectInOtherScenes) ? ", but it will be searched again in the next scenes." : "."), MessageType.Info);
                            }
                        }

                        if (GUILayout.Button("X", EditorStyles.miniButton))
                        {
                            category.audioSourcesDataConfig.Remove(audioSourceData);
                            return;
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("Add New Audio Source Name", EditorStyles.miniButton))
                    {
                        category.audioSourcesDataConfig.Add(new AudioSourceConfig("NAME", null, 1, 1, AudioSourceConfig.DisabledOrEnabled.On));

                    }
                }
            }

            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            // Your wrapped code here
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add New Category"))
        {
            audioConfigurations.categories.Add(new AudioCategory());
        }

        EditorUtility.SetDirty(audioConfigurations);
        Undo.RecordObject(audioConfigurations, "Undo audioConfigurations");
    }
}
