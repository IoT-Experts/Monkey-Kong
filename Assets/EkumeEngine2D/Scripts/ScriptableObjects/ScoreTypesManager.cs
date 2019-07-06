using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ScoresData
{
    public string scoreName;
    public bool accumulative;
    public bool accumulateOnlyIfWin;
    public bool saveBestByLevel;
    public bool saveBestOnlyIfWin;
    public float defaultValue;
    
    public ScoresData(string _scoreName, bool _accumulative, bool _accumulateOnlyIfWin, bool _saveBest, bool _saveBestOnlyIfWin, float _defaultValue)
    {
        scoreName = _scoreName;
        accumulative = _accumulative;
        accumulateOnlyIfWin = _accumulateOnlyIfWin;
        saveBestByLevel = _saveBest;
        saveBestOnlyIfWin = _saveBestOnlyIfWin;
        defaultValue = _defaultValue;
    }
}

public class ScoreTypesManager : ScriptableObject
{
    public List<ScoresData> ScoresData = new List<ScoresData>();

    static ScoreTypesManager reference;
    public static ScoreTypesManager instance
    {
        get
        {
            if (reference == null)
            {
                reference = (ScoreTypesManager)Resources.Load("Data/ScoreTypesManager", typeof(ScoreTypesManager));
                return reference;
            }
            else
                return reference;
        }
    }
}
