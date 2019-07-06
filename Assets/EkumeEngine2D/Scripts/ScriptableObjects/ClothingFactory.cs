using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using EkumeSavedData;

[Serializable]
public class ClothingItem
{
    public int category;
    public string name;
    public GameObject gameObjectOfItem;

    public ClothingItem (int _category, string _name, GameObject _gameObjectOfItem)
    {
        this.category = _category;
        this.name = _name;
        this.gameObjectOfItem = _gameObjectOfItem;
    }
}

public class ClothingFactory : ScriptableObject
{
    public List<string> itemCategories = new List<string>();
    public List<ClothingItem> items = new List<ClothingItem>();

    static ClothingFactory reference;

    public static ClothingFactory instance
    {
        get
        {
            if (reference == null)
            {
                reference = (ClothingFactory)Resources.Load("Data/ClothingFactory", typeof(ClothingFactory));

                if (PlayerPrefs.GetInt("WasInitializedDefaultItems", 0) == 0)
                {
                    for (int i = 0; i < reference.itemCategories.Count; i++)
                    {
                        for (int j = 0; j < reference.items.Count; j++)
                        {
                            if (reference.items[j].category == i)
                            {
                                ClothingInventory.PutItem(j);
                                break;
                            }
                        }
                    }
                    PlayerPrefs.SetInt("WasInitializedDefaultItems", 1);
                }

                return reference;
            }
            else
                return reference;
        }
    }
}
