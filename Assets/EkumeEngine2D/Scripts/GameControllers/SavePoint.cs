using UnityEngine;
using EkumeEnumerations;

[RequireComponent(typeof(Collider2D))]
public class SavePoint : MonoBehaviour
{
    [HideInInspector] public int savePointNumber;
    [SerializeField] bool disableColliderOnTrigger = true;
    [SerializeField] bool destroyGameObjectOnTrigger;
    [HideWhenFalse("destroyGameObjectOnTrigger")]
    [SerializeField] float delayToDestroy;

#if UNITY_EDITOR
    void Awake ()
    {
        if(!GetComponent<Collider2D>().isTrigger)
        {
            Debug.Log("Error: The Collider2D of the GameObject " + gameObject.name + " should be of type trigger for the SavePoint.");
        }
    }
#endif

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.tag == "Mount" && PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerIsRidingAMount)))
        {
            EkumeSavedData.SavePoints.SetSavePoint(savePointNumber);

            if (FindObjectOfType<GameTimeDown>() != null)
            {
                GameTimeDown gameTimeDown = FindObjectOfType<GameTimeDown>();
                EkumeSavedData.SavePoints.SetCurrentTimeOfSavePoint(gameTimeDown.timeOfCounter);
            }
            else if(FindObjectOfType<GameTimeUpScore>() != null)
            {
                GameTimeUpScore gameTimeUp = FindObjectOfType<GameTimeUpScore>();
                EkumeSavedData.SavePoints.SetCurrentTimeOfSavePoint(gameTimeUp.timeOfCounter);
            }

            if (disableColliderOnTrigger)
                GetComponent<Collider2D>().enabled = false;

            if (destroyGameObjectOnTrigger)
                Destroy(gameObject);
        }
    }
}
