using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayersManager : ScriptableObject
{
    public int defaultPlayer;
    public List<string> playerNames = new List<string>();

    static PlayersManager reference;
    public static PlayersManager instance
    {
        get
        {
            if (reference == null)
            {
                reference = (PlayersManager)Resources.Load("Data/PlayersManager", typeof(PlayersManager));
                return reference;
            }
            else
                return reference;
        }
    }
}
