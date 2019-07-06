using UnityEngine;
using System.Collections;
using EkumeSavedData;
public class MoveObjectToCurrentLevelBox : MonoBehaviour
{
    public enum WhatToLookFor { FindCurrentLevel, FindCurrentWorld }

    [Header("This script looks for the components LevelBox in the scene.")]
    [SerializeField] WhatToLookFor whatToLookFor;
    LevelBox[] levelBoxes;
    [SerializeField] Transform objectToMove;
    [SerializeField] bool moveToExactPosition;
    [Space()]
    [SerializeField] bool dontCopyYPos;
    [SerializeField] bool dontCopyXPos;

    void Awake ()
    {
        levelBoxes = FindObjectsOfType<LevelBox>();
    }

    void Start ()
    {
        if (whatToLookFor == WhatToLookFor.FindCurrentLevel)
        {
            foreach (LevelBox levelBox in levelBoxes)
            {
                if (Levels.GetLevelIdentificationOfNextLevel() == Levels.GetLevelIdentificationOfNumberOfList(levelBox.level.Index))
                {

                    if (!moveToExactPosition)
                    {
                        objectToMove.transform.localPosition = new Vector3 (
                            ((dontCopyXPos) ? objectToMove.transform.localPosition.x : -levelBox.transform.localPosition.x),
                            ((dontCopyYPos) ? objectToMove.transform.localPosition.y : -levelBox.transform.localPosition.y),
                            objectToMove.localPosition.z);
                    }
                    else
                    {
                        objectToMove.transform.localPosition = new Vector3 (
                            ((dontCopyXPos) ? objectToMove.transform.localPosition.x : levelBox.transform.localPosition.x),
                            ((dontCopyYPos) ? objectToMove.transform.localPosition.y : levelBox.transform.localPosition.y),
                            objectToMove.localPosition.z);
                    }

                    break;
                }
            }
        }
        else if (whatToLookFor == WhatToLookFor.FindCurrentWorld)
        {
            foreach (LevelBox levelBox in levelBoxes)
            {
                if (Levels.GetCurrentWorldNumber()
                    == Levels.LevelIdentificationToWorldNumber(Levels.GetLevelIdentificationOfNumberOfList(levelBox.level.Index)))
                {

                    if (!moveToExactPosition)
                    {
                        objectToMove.transform.localPosition = new Vector3(
                            ((dontCopyXPos) ? objectToMove.transform.localPosition.x : -levelBox.transform.localPosition.x),
                            ((dontCopyYPos) ? objectToMove.transform.localPosition.y : -levelBox.transform.localPosition.y),
                            objectToMove.localPosition.z);
                    }
                    else
                    {
                        objectToMove.transform.localPosition = new Vector3(
                            ((dontCopyXPos) ? objectToMove.transform.localPosition.x : levelBox.transform.localPosition.x),
                            ((dontCopyYPos) ? objectToMove.transform.localPosition.y : levelBox.transform.localPosition.y),
                            objectToMove.localPosition.z);
                    }

                    break;
                }
            }
        }
    }
	
}
