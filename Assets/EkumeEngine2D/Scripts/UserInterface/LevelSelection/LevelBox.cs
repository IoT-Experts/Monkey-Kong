using UnityEngine;
using EkumeSavedData;
using EkumeLists;

public class LevelBox : MonoBehaviour
{
    [Space()]
    public ListOfLevels level;
    public GameObject objectToActivateIfLevelUnlocked;
    public GameObject objectToActivateIfLevelLocked;
    public GameObject objectToAcivateIfLevelCleared;

	void Start ()
    {
        if (Levels.IsLevelCleared(Levels.GetLevelIdentificationOfNumberOfList(level.Index)))
        {
            if(objectToAcivateIfLevelCleared != null)
                objectToAcivateIfLevelCleared.SetActive(true);

            if(objectToActivateIfLevelLocked != null && (objectToActivateIfLevelLocked != objectToAcivateIfLevelCleared))
                objectToActivateIfLevelLocked.SetActive(false);

            if(objectToActivateIfLevelUnlocked != null && (objectToActivateIfLevelUnlocked != objectToAcivateIfLevelCleared))
                objectToActivateIfLevelUnlocked.SetActive(false);
        }
        else if (Levels.GetLevelIdentificationOfNextLevel() == Levels.GetLevelIdentificationOfNumberOfList(level.Index))
        {
            if(objectToActivateIfLevelUnlocked != null)
                objectToActivateIfLevelUnlocked.SetActive(true);

            if(objectToActivateIfLevelLocked != null && (objectToActivateIfLevelLocked != objectToActivateIfLevelUnlocked))
                objectToActivateIfLevelLocked.SetActive(false);

            if (objectToAcivateIfLevelCleared != null && (objectToAcivateIfLevelCleared != objectToActivateIfLevelUnlocked))
                objectToAcivateIfLevelCleared.SetActive(false);
        }
        else
        {
            if (objectToActivateIfLevelLocked != null)
                objectToActivateIfLevelLocked.SetActive(true);

            if(objectToAcivateIfLevelCleared != null && (objectToAcivateIfLevelCleared != objectToActivateIfLevelLocked))
                objectToAcivateIfLevelCleared.SetActive(false);

            if(objectToActivateIfLevelUnlocked != null && (objectToActivateIfLevelUnlocked != objectToActivateIfLevelLocked))
                objectToActivateIfLevelUnlocked.SetActive(false);
        }
    }
}
