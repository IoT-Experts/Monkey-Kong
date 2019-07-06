using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class DefaultTimeForPower
{
    public float coinMagnet;
    public float protectorShield;
    public float coinsDuplicator;
    public float powerToFly;
    public float trapsConverter;
    public float killerShield;
    public float jetpack;
}

[Serializable]
public class DefaultQuantityForPower
{
    public int coinMagnet;
    public int protectorShield;
    public int coinsDuplicator;
    public int powerToFly;
    public int trapsConverter;
    public int killerShield;
    public int jetpack;
}

public class OptionsOfPowers : ScriptableObject
{
    public DefaultTimeForPower defaultTimeForPower = new DefaultTimeForPower();
    public DefaultQuantityForPower defaultQuantityForPower = new DefaultQuantityForPower();

    static OptionsOfPowers reference;
    public static OptionsOfPowers instance
    {
        get
        {
            if (reference == null)
            {
                reference = (OptionsOfPowers)Resources.Load("Data/OptionsOfPowers", typeof(OptionsOfPowers));
                return reference;
            }
            else
                return reference;
        }
    }
}
