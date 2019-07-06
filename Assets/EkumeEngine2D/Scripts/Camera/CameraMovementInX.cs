using UnityEngine;
using System.Collections;
using EkumeEnumerations;

/// <sumary>
/// Moves the camera around axis X, also, this script limits the camera for avoid going off the scene
/// </sumary>
public class CameraMovementInX : MonoBehaviour
{
    Transform player;
    [Header("Limits for the camera move. (No childrens of cam)")]
    [SerializeField] Transform limitCamPositionRight;
    [SerializeField] Transform limitCamPositionLeft;

    [Header("If the player reach this positions in X, then move camera (Childs of cam)")]
    [SerializeField] Transform positionToGoRight;
    [SerializeField] Transform positionToGoLeft;

    float positionToGoLeftX;
    float positionToGoRightX;
    Camera thisCamera;

    Vector3 playerVelocity;
    Vector3 prevPos;

    void Awake ()
    {
        // Obtains the positions in X to move the camera to left and right
        positionToGoLeftX = positionToGoLeft.position.x; 
        positionToGoRightX = positionToGoRight.position.x;

		//----------------------------------------------------------------
		
        thisCamera = GetComponent<Camera>(); //Get the component 'Camera'
    }

    void Start()
    {
        // Obtains the positions in X to move the camera to left and right
        positionToGoLeftX = positionToGoLeft.position.x;
        positionToGoRightX = positionToGoRight.position.x;

        player = FindObjectOfType<Player>().transform; // This Searche the object of type Player in the scene
    }

    void LateUpdate () 
    {
        playerVelocity = (player.position - prevPos) / Time.deltaTime;
        prevPos = player.position;

        // Moves the camera depending of the positions of the limits to go right
        if (player != null && player.position.x >= positionToGoRight.position.x &&
            limitCamPositionRight.position.x >= (transform.position.x + thisCamera.orthographicSize * Screen.width / Screen.height)
            && playerVelocity.x > 0)
        {
            float positionX = player.position.x - positionToGoRightX;
            Vector3 originalPos = transform.localPosition;
            transform.localPosition = new Vector3(positionX, transform.localPosition.y, transform.localPosition.z);

            if (limitCamPositionRight.position.x <= (transform.position.x + thisCamera.orthographicSize * Screen.width / Screen.height))
            {
                transform.localPosition = originalPos;

            }
        }

        //-----------------------------------------------------------

        // Moves the camera depending of the positions of the limits to go left
        if (player != null && player.position.x <= positionToGoLeft.position.x &&
            limitCamPositionLeft.position.x <= (transform.position.x - thisCamera.orthographicSize * Screen.width / Screen.height)
            && playerVelocity.x < 0)
        {
            float positionX = player.position.x - positionToGoLeftX;
            Vector3 originalPos = transform.localPosition;

            transform.localPosition = new Vector3(positionX, transform.localPosition.y, transform.localPosition.z);

            if (limitCamPositionLeft.position.x >= (transform.position.x - thisCamera.orthographicSize * Screen.width / Screen.height))
            {
                transform.localPosition = originalPos;
            }
        }

        //-------------------------------------------------------------------------------
    }
}
