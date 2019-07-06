using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollTexture : MonoBehaviour
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
    Renderer image;
    float startTime;

    void Start()
    {
        if (positionToFollow == PositionToFollow.PlayerPosition)
            objectToFollow = Player.playerTransform;
        else if (positionToFollow == PositionToFollow.CameraPosition)
            objectToFollow = GameObject.FindObjectOfType<Camera>().transform;

        if(constantScroll)
            startTime = Time.time;

        image = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        if (!constantScroll)
        {
            image.material.mainTextureOffset = new Vector2(
                ((scrollInX) ? objectToFollow.position.x * velocityToScrollInX / 10 : image.material.mainTextureOffset.x),
                ((scrollInY) ? objectToFollow.position.y * velocityToScrollInY / 10 : image.material.mainTextureOffset.y));
        }
        else
        {
            image.material.mainTextureOffset = new Vector2(
                ((scrollInX) ? (Time.time - startTime) * (velocityToScrollInX/10) % 1 : image.material.mainTextureOffset.x),
                ((scrollInY) ? (Time.time - startTime) * (velocityToScrollInY/10) % 1 : image.material.mainTextureOffset.y));
        }
    }
}
