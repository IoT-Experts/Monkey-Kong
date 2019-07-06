using UnityEngine;
using System.Collections;
using EkumeEnumerations;

/// <sumary>
/// Moves the camera around axis Y, also, this script limits the camera for avoid going off the scene
/// </sumary>

public class CameraMovementInY : MonoBehaviour {

    Transform player;
    [Header("Limits for the camera move. (No childrens of cam)")]
    [SerializeField] Transform limitCamPositionUp;
    [SerializeField] Transform limitCamPositionDown;

    [Header("If the player reach this positions in Y, then move camera (Childs of cam)")]
    [SerializeField] Transform positionToGoUp;
    [SerializeField] Transform positionToGoDown;

    float positionToGoDownY;
    float positionToGoUpY;
    Camera thisCamera;

    Vector3 playerVelocity;
    Vector3 prevPos;

    void Awake ()
    {
		// Obtains the positions in X to move the camera to down and up
        positionToGoDownY = positionToGoDown.position.y;
        positionToGoUpY = positionToGoUp.position.y;
		//----------------------------------------------------------------
        thisCamera = GetComponent<Camera>(); //Get the component 'Camera'
    }

    void Start()
    {
        player = FindObjectOfType<Player>().transform; // It Searches the object of type Player in the scene
    }

    void LateUpdate ()
    {
        playerVelocity = (player.position - prevPos) / Time.deltaTime;
        prevPos = player.position;

        // Moves the camera depending of the positions of the limits to go up

        if (player != null && player.position.y >= positionToGoUp.position.y
            && limitCamPositionUp.position.y >= (transform.position.y + thisCamera.orthographicSize)
            && playerVelocity.y > 0)
        {
            float positionY = player.position.y - positionToGoUpY;
            transform.localPosition = new Vector3(transform.localPosition.x, positionY, transform.localPosition.z);
        }

		//-----------------------------------------------------------

		// Moves the camera depending of the positions of the limits to go down
        if (player != null && player.position.y <= positionToGoDown.position.y 
            && limitCamPositionDown.position.y <= (transform.position.y - thisCamera.orthographicSize)
            && playerVelocity.y < 0)
        {
            float positionY = player.position.y - positionToGoDownY;
            transform.localPosition = new Vector3(transform.localPosition.x, positionY, transform.localPosition.z);
        }
		//-------------------------------------------------------------------------------
    }
}
