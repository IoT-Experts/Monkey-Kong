using UnityEngine;
using EkumeSavedData.Player;
using EkumeLists;
using System.Collections;

public class CharacterBox : MonoBehaviour
{
    [SerializeField] bool startUnlocked;
    [SerializeField] ListOfPlayers character;

    [SerializeField] GameObject objectToActivateIfIsUnlocked;
    [SerializeField] GameObject objectToActivateIfIsLocked;
    [SerializeField] GameObject objectToActivateIfIsSelected;

    int frames;

    void OnEnable()
    {
        RefreshSelection();
        StartCoroutine("UpdateSelection");
    }

    void RefreshSelection ()
    {
        if (startUnlocked || PlayerSelection.PlayerIsUnlocked(character.Index))
        {
            if (PlayerSelection.GetPlayerSelected() == character.Index)
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
        else if (!PlayerSelection.PlayerIsUnlocked(character.Index))
        {
            if (objectToActivateIfIsUnlocked != null)
                objectToActivateIfIsUnlocked.SetActive(false);

            if (objectToActivateIfIsSelected != null)
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
