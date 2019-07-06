using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ScrollRawImage : MonoBehaviour
{

    public enum PositionToFollow { CameraPosition, PlayerPosition }
    [Space()]

    [SerializeField] bool constantScroll;
    [HideWhenTrue("constantScroll")]
    [SerializeField] PositionToFollow positionToFollow;
        
    [SerializeField] bool scrollInX;
    [HideWhenFalse("scrollInX")]
    [SerializeField] float velocityToScrollInX;

    [SerializeField] bool scrollInY;
    [HideWhenFalse("scrollInY")]
    [SerializeField] float velocityToScrollInY;

    Transform objectToFollow;
    RawImage image;
    float startTime;

    void Start()
    {
        if (positionToFollow == PositionToFollow.PlayerPosition)
            objectToFollow = Player.playerTransform;
        else if (positionToFollow == PositionToFollow.CameraPosition)
            objectToFollow = GameObject.FindObjectOfType<Camera>().transform;

        if(constantScroll)
            startTime = Time.time;

        image = GetComponent<RawImage>();
    }

    void LateUpdate()
    {
        if (!constantScroll)
        {
            image.uvRect = new Rect(
                ((scrollInX) ? objectToFollow.position.x * velocityToScrollInX / 10 : image.uvRect.x),
                ((scrollInY) ? objectToFollow.position.y * velocityToScrollInY / 10 : image.uvRect.y),
                image.uvRect.width, image.uvRect.height);
        }
        else
        {
            image.uvRect = new Rect(
                ((scrollInX) ? (Time.time - startTime) * (velocityToScrollInX/10) % 1 : image.uvRect.x),
                ((scrollInY) ? (Time.time - startTime) * (velocityToScrollInY/10) % 1 : image.uvRect.y),
                image.uvRect.width, image.uvRect.height);
        }
    }
}
