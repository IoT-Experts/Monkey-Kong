#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
public class ComponentInfo
{
    public MonoScript script;
    public string description;
    public int category;

    public ComponentInfo (MonoScript script, string littleDescription, int category)
    {
        this.script = script;
        this.description = littleDescription;
        this.category = category;
    }
}

public class ComponentsManager : ScriptableObject
{
    public List<ComponentInfo> components = new List<ComponentInfo>();
    public List<string> categories = new List<string>();

    static ComponentsManager reference;

    public static ComponentsManager instance
    {
        get
        {
            if (reference == null)
            {
                reference = (ComponentsManager)Resources.Load("Data/ComponentsManager", typeof(ComponentsManager));
                return reference;
            }
            else
                return reference;
        }
    }
}
#endif