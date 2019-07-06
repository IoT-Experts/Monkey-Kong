using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineOfTravel : MonoBehaviour
{
    public enum Orientation { Horizontal, Vertical }
    public Orientation orientationOfLine;
    public List<Transform> objectsToShowInLine = new List<Transform>();
    public List<RectTransform> travelersUI = new List<RectTransform>();
    public Transform startOfScene;
    public Transform endOfScene;
    public RectTransform aPointUI;
    public RectTransform bPointUI;
    public float delayToRefreshPositions = 0.3f;

    public bool findPlayer;
    public bool findGoal;
    public bool findSavePoints;
    public bool findStars;
    public RectTransform playerObjectInLine;
    public RectTransform goalObjectInLine;
    public RectTransform savePointObjectInLine;
    public bool autoAsignStartAndEnd;

    float gameDistance;
    float uIStartDistance;

    void Start()
    {
        if (findPlayer)
        {
            objectsToShowInLine.Add(Player.playerTransform);
            travelersUI.Add(playerObjectInLine);
        }

        if (findGoal)
        {
            objectsToShowInLine.Add(FindObjectOfType<WinLevel>().transform);
            travelersUI.Add(goalObjectInLine);
        }

        if(findSavePoints)
        {
            SavePoint[] savePoints = FindObjectsOfType<SavePoint>();

            foreach(SavePoint sp in savePoints)
            {
                RectTransform instanceSPInLine = Instantiate(savePointObjectInLine, savePointObjectInLine.transform.parent, true) as RectTransform;
                objectsToShowInLine.Add(sp.transform);
                travelersUI.Add(instanceSPInLine);
                instanceSPInLine.SetAsFirstSibling();
            }

            Destroy(savePointObjectInLine.gameObject);
        }

        if (autoAsignStartAndEnd)
        {
            endOfScene = FindObjectOfType<WinLevel>().transform;
            GameObject start = new GameObject("StartOfScene");
            start.transform.position = Player.playerTransform.position;
            startOfScene = start.transform;
        }

        gameDistance = Vector3.Distance(startOfScene.position, endOfScene.position);
        uIStartDistance = Vector2.Distance(aPointUI.localPosition, bPointUI.localPosition);

        StartCoroutine("PositionUpdate");
    }

    IEnumerator PositionUpdate()
    {
        for (;;)
        {
            RefreshPositions();
            yield return new WaitForSeconds(delayToRefreshPositions);
        }
    }

    void RefreshPositions()
    {
        float[] currentDistancesInGame = new float[objectsToShowInLine.Count];
        float[] porcentTraveled = new float[objectsToShowInLine.Count];

        for (int i = 0; i < objectsToShowInLine.Count; i++)
        {
            if (objectsToShowInLine[i] != null && travelersUI[i] != null)
            {
                currentDistancesInGame[i] = Vector2.Distance(objectsToShowInLine[i].position, endOfScene.position);
                porcentTraveled[i] = (currentDistancesInGame[i] * 100) / gameDistance;

                if (orientationOfLine == Orientation.Vertical)
                    travelersUI[i].localPosition = new Vector3(travelersUI[i].localPosition.x, ((-uIStartDistance / 100) * porcentTraveled[i]) + uIStartDistance * 0.5f, travelersUI[i].localPosition.z);
                else if (orientationOfLine == Orientation.Horizontal)
                    travelersUI[i].localPosition = new Vector3(((-uIStartDistance / 100) * porcentTraveled[i]) + uIStartDistance * 0.5f, travelersUI[i].localPosition.y, travelersUI[i].localPosition.z);
            }
            else
            {
                travelersUI[i].gameObject.SetActive(false);
            }
        }
    }
}