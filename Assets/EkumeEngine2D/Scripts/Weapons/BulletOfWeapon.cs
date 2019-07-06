using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BulletOfWeapon : MonoBehaviour
{
    enum ColliderType { Collision, Trigger }

    [SerializeField] ColliderType colliderType;
    [SerializeField] List<string> tagsToActivateDestructionOfThisObject;
    [SerializeField] float delayToDestroy;
    GameObject collisionEffectGameObject;

    DirectionsXAxisEnum weaponDirection;
    float maxDistance;
    BulletData.Type bulletType;
    float countdownToActivaterenade;
    BulletData.ActivationMode activationMode;
    GameObject explosionOfGrenadeGameObject;
    bool grenadeIsActivated;

    Vector2 startPosition;

    void Awake ()
    {
        startPosition = this.transform.position;
    }

    public void SetBulletData (GameObject _collisionEffectGameObject, DirectionsXAxisEnum _weaponDirection, float _maxDistance, BulletData.Type _bulletType, float _countdownToActivaterenade, BulletData.ActivationMode _activationMode, GameObject _explosionOfGrenadeGameObject)
    {
        collisionEffectGameObject = _collisionEffectGameObject;
        weaponDirection = _weaponDirection;
        maxDistance = _maxDistance;
        bulletType = _bulletType;
        countdownToActivaterenade = _countdownToActivaterenade;
        activationMode = _activationMode;
        explosionOfGrenadeGameObject = _explosionOfGrenadeGameObject;
    }

    void Start ()
    {
        if (bulletType == BulletData.Type.Grenade && activationMode == BulletData.ActivationMode.WhenItIsInstantiated)
        {
            StartCoroutine(CountDownToActivateGrenade());
            grenadeIsActivated = true;
        }
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (colliderType == ColliderType.Collision)
        {
            ActionsWhenCollides(tagsToActivateDestructionOfThisObject.Contains(other.collider.tag) ? true : false);

            if(bulletType == BulletData.Type.Grenade)
            {
                if(!grenadeIsActivated)
                {
                    if(activationMode == BulletData.ActivationMode.WhenCollides)
                    {
                        StartCoroutine(CountDownToActivateGrenade());
                        grenadeIsActivated = true;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (colliderType == ColliderType.Trigger)
        {
            ActionsWhenCollides(tagsToActivateDestructionOfThisObject.Contains(other.tag) ? true : false);
        }
    }

    IEnumerator CountDownToActivateGrenade ()
    {
        yield return new WaitForSeconds(countdownToActivaterenade);
        Instantiate(explosionOfGrenadeGameObject, transform.position, explosionOfGrenadeGameObject.transform.rotation);
        Destroy(this.gameObject);
    }

    void ActionsWhenCollides (bool destroy)
    {
        if (destroy)
        {
            if (collisionEffectGameObject != null)
            {
                GameObject instantiatedEffect = Instantiate(collisionEffectGameObject, transform.position, transform.rotation) as GameObject;

                if (weaponDirection == DirectionsXAxisEnum.Left)
                    instantiatedEffect.transform.localScale = new Vector3(instantiatedEffect.transform.localScale.x * -1, instantiatedEffect.transform.localScale.y, instantiatedEffect.transform.localScale.z);
            }

            Destroy(this.gameObject, delayToDestroy);
        }
    }

    void Update ()
    {
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
            Destroy(this.gameObject);
    }
}
