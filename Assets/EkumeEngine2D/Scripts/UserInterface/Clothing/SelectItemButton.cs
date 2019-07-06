using UnityEngine;
using UnityEngine.EventSystems;
using EkumeSavedData;
using UnityEngine.UI;
using EkumeLists;

[RequireComponent(typeof(Button))]
public class SelectItemButton : MonoBehaviour, IPointerClickHandler
{
    public ListOfClothingItems itemToSelect;
    static int numberOfSelectorsOfItems = 0;

    int quantityOfScripts;

    ClothingItemInstantiator[] instantiators;

    void Start ()
    {
        quantityOfScripts = gameObject.GetComponents<SelectItemButton>().Length;
        instantiators = FindObjectsOfType<ClothingItemInstantiator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClothingInventory.PutItem(itemToSelect.Index);
        numberOfSelectorsOfItems++;

        if (quantityOfScripts == numberOfSelectorsOfItems) //Prevents the following code is called repeatedly unnecessarily by all the scripts of this gameObject
        {
            for (int i = 0; i < instantiators.Length; i++)
            {
                instantiators[i].RefreshClothing();
            }

            numberOfSelectorsOfItems = 0;
        }
    }
}