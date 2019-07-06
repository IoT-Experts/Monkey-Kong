using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using EkumeSavedData;

[Serializable]
public class Level
{
    public bool startUnlocked;
    public string sceneName;

    public UnityEngine.Object _levelScene;
    public UnityEngine.Object levelScene
    {
        get { return _levelScene; }
        set
        {
            //Only set when the value is changed
            if (_levelScene != value && value != null)
            {
                string name = value.ToString();
                if (name.Contains(" (UnityEngine.SceneAsset)"))
                {
                    _levelScene = value;
                    sceneName = name.Substring(0, name.IndexOf(" (UnityEngine.SceneAsset)"));
                }
            }
        }
    }

    public Level (bool levelCleared, UnityEngine.Object levelScene)
    {
        this.startUnlocked = levelCleared;
        this.levelScene = levelScene;
    }
}

[Serializable]
public class World
{
    public List<Level> level = new List<Level>()
    {
        new Level(false, null) //Creates the level 0 for each world
    };
}

public class LevelsManager : ScriptableObject
{
    public List<World> world = new List<World>()
    {
        new World() // Creates the world 0
    }; //This list is only used to save the levels, and save the default values of each level. (Does not store the levels cleared by the player)

    static LevelsManager reference;

    public static LevelsManager instance
    {
        get
        {
            if (reference == null)
            {
                reference = (LevelsManager)Resources.Load("Data/LevelsManager", typeof(LevelsManager));

                if (PlayerPrefs.GetInt("levelsInitializedLikeCleared", 0) == 0)
                {
                    for (int i = 0; i < reference.world.Count; i++)
                    {
                        for (int j = 0; j < reference.world[i].level.Count; j++)
                        {
                            if (reference.world[i].level[j].startUnlocked)
                            {
                                Levels.SetLevelCleared("World " + i + " - Level " + j, true);
                            }
                        }
                    }

                    PlayerPrefs.SetInt("levelsInitializedLikeCleared", 1);
                }

                return reference;
            }
            else
                return reference;
        }
    }
}