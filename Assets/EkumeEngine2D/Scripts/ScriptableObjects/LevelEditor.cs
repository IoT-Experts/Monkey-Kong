#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

[Serializable]
public class Category
{
    public string name;
    public List<ObjectOfCategory> objects = new List<ObjectOfCategory>() { new ObjectOfCategory(null, 0) };
    public bool divideInSubcategory;
    public List<Subcategory> subCategory = new List<Subcategory>();
}

[Serializable]
public class Subcategory
{
    public string name;
    public List<ObjectOfCategory> objects = new List<ObjectOfCategory>() { new ObjectOfCategory(null, 0) };
}

[Serializable]
public class ObjectOfCategory
{
    public GameObject obj;
    public float zPos;

    public ObjectOfCategory (GameObject obj, float zPos)
    {
        this.obj = obj;
        this.zPos = zPos;
    }
}

public class LevelEditor : ScriptableObject
{
    public List<Category> categories = new List<Category>();
    public bool zPositionOfPrefab = true;
    public static Tool currentTool;

    public static void ChangeTool ()
    {
        Tools.current = Tool.Rect;
    }

    public static Tool GetCurrentTool ()
    {
        return Tools.current;
    }
}
#endif