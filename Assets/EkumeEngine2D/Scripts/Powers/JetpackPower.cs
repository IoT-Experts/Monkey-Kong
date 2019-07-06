using UnityEngine;
using System.Collections;
using EkumeLists;

public class JetpackPower : MonoBehaviour
{

    public ListOfInputControls inputControlJetpack = new ListOfInputControls();
    public float maxVelocityOfJetpack = 5;
    public float velocity = 0.75f;
    Rigidbody2D thisRigidbody;

    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (InputControls.GetControl(inputControlJetpack.Index))
        {
            if ((this.gameObject.tag == "Mount"
                  && PlayerStates.GetPlayerStateValue(EkumeEnumerations.PlayerStatesEnum.PlayerIsRidingAMount)
                  && DismountOfPlayer.currentMount == this.gameObject)
                  || this.gameObject.tag == "Player")
            {
                if (thisRigidbody.velocity.y <= maxVelocityOfJetpack)
                {
                    thisRigidbody.velocity = new Vector2(thisRigidbody.velocity.x, thisRigidbody.velocity.y + (velocity * Time.deltaTime * 100));
                }
            }
        }
    }
}
