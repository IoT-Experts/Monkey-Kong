using UnityEngine;
using System.Collections;
using EkumeSavedData;
using EkumeLists;

public class ClothingItemInstantiator : MonoBehaviour
{
    public ListOfClothingCategories categoryOfObject;

    void Start ()
    {
        RefreshClothing();
    }

#if UNITY_EDITOR
    void Awake ()
    {
        if(transform.childCount > 0)
        {
            Debug.LogWarning("The GameObject " + this.gameObject.name + " does not should have childs.");
            Debug.LogWarning("The GameObject " + this.gameObject.name + " has objects like childs that will be deleted when the clothing item is instantiated.");
        }
    }
#endif

    public void RefreshClothing ()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int itemPlaced = ClothingInventory.GetItemPlacedInCategory(categoryOfObject.Index);
        if (ClothingFactory.instance.items[itemPlaced].gameObjectOfItem != null)
        {
            Quaternion itemRotation = ClothingFactory.instance.items[itemPlaced].gameObjectOfItem.transform.rotation * transform.rotation;
            GameObject itemCreated = Instantiate(ClothingFactory.instance.items[itemPlaced].gameObjectOfItem, this.transform.position, itemRotation) as GameObject;
            itemCreated.transform.SetParent(this.transform, true);
        }
    }
}
