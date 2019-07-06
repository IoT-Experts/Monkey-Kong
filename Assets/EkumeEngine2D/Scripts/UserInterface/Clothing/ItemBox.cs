using UnityEngine;
using EkumeSavedData;
using EkumeLists;
using System.Collections;

public class ItemBox : MonoBehaviour
{
    [SerializeField] bool startUnlocked;
    [SerializeField] ListOfClothingItems item;

    [SerializeField] GameObject objectToActivateIfIsUnlocked;
    [SerializeField] GameObject objectToActivateIfIsLocked;
    [SerializeField] GameObject objectToActivateIfIsSelected;

    int frames;

    void OnEnable()
    {
        RefreshSelection();
        StartCoroutine("UpdateSelection");
    }

    void RefreshSelection()
    {
        if (startUnlocked || ClothingInventory.IsItemInInventory(item.Index))
        {
            if (ClothingInventory.GetItemPlacedInCategory(ClothingFactory.instance.items[item.Index].category) == item.Index)
            {
                if (objectToActivateIfIsUnlocked != null)
                    objectToActivateIfIsUnlocked.SetActive(false);

                if (objectToActivateIfIsLocked != null)
                    objectToActivateIfIsLocked.SetActive(false);

                if (objectToActivateIfIsSelected != null)
                    objectToActivateIfIsSelected.SetActive(true);
            }
            else
            {
                if (objectToActivateIfIsLocked != null)
                    objectToActivateIfIsLocked.SetActive(false);

                if (objectToActivateIfIsSelected != null)
                    objectToActivateIfIsSelected.SetActive(false);

                if (objectToActivateIfIsUnlocked != null)
                    objectToActivateIfIsUnlocked.SetActive(true);
            }
        }
        else if (!ClothingInventory.IsItemInInventory(item.Index))
        {
            if(objectToActivateIfIsUnlocked != null)
                objectToActivateIfIsUnlocked.SetActive(false);

            if(objectToActivateIfIsSelected != null)
                objectToActivateIfIsSelected.SetActive(false);

            if (objectToActivateIfIsLocked != null)
                objectToActivateIfIsLocked.SetActive(true);
        }
    }

    IEnumerator UpdateSelection()
    {
        for (;;)
        {
            RefreshSelection();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
