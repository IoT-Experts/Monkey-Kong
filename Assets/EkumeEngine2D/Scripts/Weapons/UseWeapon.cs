using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using UnityEngine.UI;
using EkumeSavedData.WeaponInventory;
using System.Collections.Generic;

public class UseWeapon : MonoBehaviour
{
    public enum WeaponTypeToUse { UseTheSavedWeapon, UseSpecificWeapon }
    public enum UsedBy { Player, Enemy, Mount }

    public UsedBy usedBy;
    public Transform weaponPosition; //Where will be instantiated the weapon
    public WeaponTypeToUse weaponTypeToUse;
    public int weaponToUse;
    public bool automaticShootingWithTarget;
    public TargetDetector targetDetector;
    public int inputControl; //input control to shoot

    //UI
    public bool showGunImage;
    public Image uIOfGun;
    public bool showBulletImage;
    public Image uIOfBullet;
    public Sprite iconIfUseMeleeWeapon;
    public bool showBulletQuantity;
    public Text textOfBullets;
    public string formatBulletQuantity = "";
    public string textInfiniteBullets;

    public bool enablePlayerStatesExceptions;
    public List<PlayerStatesEnum> playerStatesExceptions = new List<PlayerStatesEnum>();
    bool someExceptionEnabled;

    Enemy enemyStates;
    DirectionsXAxisEnum weaponDirection;
    Vector3 originalRotationOfWeapon;
    float timerToNextShot;
    GameObject instantiatedWeapon;
    float timerToDisableCollider;
    bool startTimerToDisableCollider;

    bool startTimerToStopMovement;
    float timerForStoppingTime;

    bool startTimerForDelayOfAttak;
    float timerForDelayOfAttack;

    public void ChangeWeapon(int weaponNumber)
    {
        weaponToUse = weaponNumber;

        if (usedBy == UsedBy.Player)
        {
            Weapons.SetWeaponThatIsUsing(weaponToUse);
            PlayerStates.SetStateValueForAllAttackCategories(false);
            PlayerStates.SetStateOfUseOfWeaponCategory(WeaponFactory.instance.weaponsData[weaponNumber].weaponCategory, true);
        }

        if (weaponNumber != -1)
        {
            if (instantiatedWeapon != null)
                Destroy(instantiatedWeapon);

            Quaternion weaponRotation = new Quaternion();

            if ((usedBy == UsedBy.Enemy) ? enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft) : PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft))
            {
                weaponRotation = WeaponFactory.instance.weaponsData[weaponNumber].weaponGameObject.transform.rotation *
                    (new Quaternion(weaponPosition.rotation.x, weaponPosition.rotation.y, weaponPosition.rotation.z * -1, weaponPosition.rotation.w));
            }
            else
            {
                weaponRotation = WeaponFactory.instance.weaponsData[weaponNumber].weaponGameObject.transform.rotation * weaponPosition.rotation;
            }

            instantiatedWeapon = Instantiate(WeaponFactory.instance.weaponsData[weaponNumber].weaponGameObject, weaponPosition.position, weaponRotation) as GameObject;

            if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft))
            {
                instantiatedWeapon.transform.localScale = new Vector3(instantiatedWeapon.transform.localScale.x * -1, instantiatedWeapon.transform.localScale.y, instantiatedWeapon.transform.localScale.z);
            }

            instantiatedWeapon.transform.SetParent(weaponPosition, true);

            RefreshUIWeapon();
        }
    }

    void Start()
    {
        if (usedBy == UsedBy.Enemy)
            enemyStates = GetComponent<Enemy>();

        if (WeaponFactory.instance.savingTypeOfBullets == WeaponFactory.TypeSave.RestartBulletsOfCurrentGunWhenRestartLevel)
        {
            if (Weapons.GetWeaponThatIsUsing() != -1)
                Bullets.SetBulletsToGun(Weapons.GetWeaponThatIsUsing(), WeaponFactory.instance.weaponsData[Weapons.GetWeaponThatIsUsing()].defaultBulletQuantity);
        }
        else if (WeaponFactory.instance.savingTypeOfBullets == WeaponFactory.TypeSave.RestartBulletsOfAllGunsWhenRestartLevel)
        {
            for (int i = 0; i < WeaponFactory.instance.weaponsData.Count; i++)
            {
                Bullets.SetBulletsToGun(i, WeaponFactory.instance.weaponsData[i].defaultBulletQuantity);
            }
        }

        originalRotationOfWeapon = weaponPosition.localEulerAngles;

        if (weaponTypeToUse == WeaponTypeToUse.UseSpecificWeapon)
        {
            ChangeWeapon(weaponToUse);
        }
        else
        {
            ChangeWeapon(Weapons.GetWeaponThatIsUsing());
        }

        if (gameObject.tag == "Mount")
        {
            this.enabled = false;
        }

        if(enablePlayerStatesExceptions)
        {
            StartCoroutine("CheckStatesExceptions");
        }
    }

    IEnumerator CheckStatesExceptions()
    {
        for (;;)
        {
            for (int i = 0; i < playerStatesExceptions.Count; i++)
            {
                if(PlayerStates.GetPlayerStateValue(playerStatesExceptions[i]))
                {
                    someExceptionEnabled = true;
                    break;
                }
                else if(i == playerStatesExceptions.Count - 1)
                {
                    someExceptionEnabled = false;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnEnable()
    {
        if (weaponToUse != -1)
        {
            if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
            {
                RefreshUIWeapon();
            }
        }
    }

    public void RefreshUIWeapon()
    {
        if (showGunImage)
        {
            if(uIOfGun != null)
            {
                uIOfGun.sprite = WeaponFactory.instance.weaponsData[weaponToUse].weaponGameObject.GetComponent<SpriteRenderer>().sprite;
                uIOfGun.color = WeaponFactory.instance.weaponsData[weaponToUse].weaponGameObject.GetComponent<SpriteRenderer>().color;
            }
        }

        if (showBulletImage)
        {
            if (WeaponFactory.instance.weaponsData[weaponToUse].weaponType == WeaponFactory.WeaponType.Gun)
            {
                if (uIOfBullet != null)
                {
                    uIOfBullet.sprite = WeaponFactory.instance.bulletData[WeaponFactory.instance.weaponsData[weaponToUse].bulletToShoot].bulletGameObject.GetComponent<SpriteRenderer>().sprite;
                    uIOfBullet.color = WeaponFactory.instance.bulletData[WeaponFactory.instance.weaponsData[weaponToUse].bulletToShoot].bulletGameObject.GetComponent<SpriteRenderer>().color;
                }
            }
            else
            {
                if (uIOfBullet != null)
                {
                    uIOfBullet.sprite = iconIfUseMeleeWeapon;
                    uIOfBullet.color = Color.white;
                }
            }
        }

        if (showBulletQuantity)
        {
            if (textOfBullets != null)
            {
                if (WeaponFactory.instance.weaponsData[weaponToUse].infiniteBullets)
                {
                    textOfBullets.text = textInfiniteBullets;
                }
                else
                {
                    textOfBullets.text = Bullets.GetBulletsOfGun(weaponToUse).ToString(" " + formatBulletQuantity);
                }
            }
        }
    }
    
    void ShootBullet()
    {
        if (timerToNextShot >= WeaponFactory.instance.weaponsData[weaponToUse].timeToNextShot)
        {
            if (Bullets.GetBulletsOfGun(weaponToUse) > 0 || WeaponFactory.instance.weaponsData[weaponToUse].infiniteBullets)
            {
                if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
                    PlayerStates.SetAttackCategoryStateValue(WeaponFactory.instance.weaponsData[weaponToUse].weaponCategory, true);
                else if (usedBy == UsedBy.Enemy)
                    enemyStates.SetStateValue(EnemyStatesEnum.EnemyAttack, true);

                timerToNextShot = 0;
                StartCoroutine(DelayBeforeShotActivation()); //Here is fired the bullet
            }
        }
    }

    void Update()
    {
        if (weaponToUse != -1)
        {
            timerToNextShot += Time.deltaTime; //Increase timer to next shot

            if (!enablePlayerStatesExceptions || (enablePlayerStatesExceptions && !someExceptionEnabled))
            {
                if (WeaponFactory.instance.weaponsData[weaponToUse].weaponType == WeaponFactory.WeaponType.Gun)
                {
                    if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
                    {
                        if (timerToNextShot > 0.1f)
                        {
                            PlayerStates.SetAttackCategoryStateValue(WeaponFactory.instance.weaponsData[weaponToUse].weaponCategory, false);
                        }
                    }
                    else if (usedBy == UsedBy.Enemy)
                    {
                        if (timerToNextShot > 0.1f)
                        {
                            enemyStates.SetStateValue(EnemyStatesEnum.EnemyAttack, false);
                        }
                    }

                    if ((usedBy != UsedBy.Enemy && InputControls.GetControl(inputControl) && !automaticShootingWithTarget)
                        || (automaticShootingWithTarget && targetDetector.locatedTarget != null))
                    {
                        if (WeaponFactory.instance.weaponsData[weaponToUse].stopMovementWhenAttack)
                        {
                            if (!WeaponFactory.instance.weaponsData[weaponToUse].iUseACustomMovementScript)
                            {
                                if (usedBy == UsedBy.Enemy)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                                    if (GetComponent<AIEnemyMovement>() != null)
                                    {
                                        GetComponent<AIEnemyMovement>().movementSpeed = 0;
                                        GetComponent<AIEnemyMovement>().movementSpeedToFollow = 0;
                                    }

                                    if (GetComponent<AIFlyingEnemy>() != null)
                                        GetComponent<AIFlyingEnemy>().velocityOfMovement = 0;
                                }
                                else
                                {
                                    if (GetComponent<PlayerHorizontalMovement>() != null)
                                    {
                                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                        GetComponent<PlayerHorizontalMovement>().enabled = false;
                                    }
                                }
                            }
                            else
                            {
                                if (GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) != null)
                                {
                                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                    MonoBehaviour script = GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) as MonoBehaviour;
                                    script.enabled = false;
                                }
#if UNITY_EDITOR
                                else
                                {
                                    Debug.LogError("The script " + WeaponFactory.instance.weaponsData[weaponToUse].movementScript + " does not exist. Go to the Weapon Factory and correct it at the weapon " + WeaponFactory.instance.weaponNames[weaponToUse]);
                                }
#endif
                            }

                            startTimerToStopMovement = true;
                            timerForStoppingTime = 0;
                        }
                        ShootBullet();
                    }

                    if (instantiatedWeapon != null)
                    {
                        if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
                        {
                            if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsLeft))
                                ChangeBulletDirection(DirectionsXAxisEnum.Left);
                            else if (PlayerStates.GetPlayerStateValue(PlayerStatesEnum.PlayerDirectionIsRight))
                                ChangeBulletDirection(DirectionsXAxisEnum.Right);
                        }
                        else if (usedBy == UsedBy.Enemy)
                        {
                            if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsLeft))
                                ChangeBulletDirection(DirectionsXAxisEnum.Left);
                            else if (enemyStates.GetStateValue(EnemyStatesEnum.EnemyDirectionIsRight))
                                ChangeBulletDirection(DirectionsXAxisEnum.Right);
                        }
                    }
                }
                else
                {
                    if (timerToNextShot >= WeaponFactory.instance.weaponsData[weaponToUse].timeToNextShot)
                    {
                        if ((InputControls.GetControlDown(inputControl) && usedBy != UsedBy.Enemy) ||
                            (automaticShootingWithTarget && targetDetector.locatedTarget != null))
                        {
                            if (WeaponFactory.instance.weaponsData[weaponToUse].stopMovementWhenAttack)
                            {
                                if (!WeaponFactory.instance.weaponsData[weaponToUse].iUseACustomMovementScript)
                                {
                                    if (usedBy == UsedBy.Enemy)
                                    {
                                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                                        if (GetComponent<AIEnemyMovement>() != null)
                                        {
                                            GetComponent<AIEnemyMovement>().movementSpeed = 0;
                                            GetComponent<AIEnemyMovement>().movementSpeedToFollow = 0;
                                        }

                                        if (GetComponent<AIFlyingEnemy>() != null)
                                            GetComponent<AIFlyingEnemy>().velocityOfMovement = 0;
                                    }
                                    else
                                    {
                                        if (GetComponent<PlayerHorizontalMovement>() != null)
                                        {
                                            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                            GetComponent<PlayerHorizontalMovement>().enabled = false;
                                        }
                                    }   
                                }
                                else
                                {
                                    if (GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) != null)
                                    {
                                        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                                        MonoBehaviour script = GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) as MonoBehaviour;
                                        script.enabled = false;
                                    }
#if UNITY_EDITOR
                                    else
                                    {
                                        Debug.LogError("The script " + WeaponFactory.instance.weaponsData[weaponToUse].movementScript + " does not exist. Go to the Weapon Factory and correct it at the weapon " + WeaponFactory.instance.weaponNames[weaponToUse]);
                                    }
#endif
                                }

                                startTimerToStopMovement = true;
                                timerForStoppingTime = 0;
                            }

                            if (!startTimerForDelayOfAttak)
                            {
                                startTimerForDelayOfAttak = true;
                                timerForDelayOfAttack = 0;
                            }

                            if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
                            {
                                PlayerStates.SetAttackCategoryStateValue(WeaponFactory.instance.weaponsData[weaponToUse].weaponCategory, true);
                            }
                            else if (usedBy == UsedBy.Enemy)
                            {
                                enemyStates.SetStateValue(EnemyStatesEnum.EnemyAttack, true);
                            }

                            startTimerToDisableCollider = true;
                            timerToNextShot = 0;
                        }
                    }

                    if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
                    {
                        if (timerToNextShot > 0.1f)
                            PlayerStates.SetAttackCategoryStateValue(WeaponFactory.instance.weaponsData[weaponToUse].weaponCategory, false);
                    }
                    else if (usedBy == UsedBy.Enemy)
                    {
                        if (timerToNextShot > 0.1f)
                            enemyStates.SetStateValue(EnemyStatesEnum.EnemyAttack, false);
                    }

                    if (startTimerToDisableCollider)
                    {
                        timerToDisableCollider += Time.deltaTime;

                        if (timerToDisableCollider >= WeaponFactory.instance.weaponsData[weaponToUse].attackDuration)
                        {
                            if (instantiatedWeapon.GetComponent<Collider2D>() != null)
                                instantiatedWeapon.GetComponent<Collider2D>().enabled = false;
                            else
                                Debug.LogError("The current weapon of the player should have a Collider2D.");

                            startTimerToDisableCollider = false;
                            timerToDisableCollider = 0;
                        }
                    }
                }
            }

            if(startTimerToStopMovement)
            {
                ReturnMovementTimer();
            }

            if(startTimerForDelayOfAttak)
            {
                DelayToAttack();
            }
        }
    }

    IEnumerator DelayBeforeShotActivation ()
    {
        yield return new WaitForSeconds(WeaponFactory.instance.weaponsData[weaponToUse].delayTimeBeforeShotActivation);

        if (!WeaponFactory.instance.weaponsData[weaponToUse].infiniteBullets)
            Bullets.SetBulletsToGun(weaponToUse, Bullets.GetBulletsOfGun(weaponToUse) - 1);

        if (usedBy == UsedBy.Player || usedBy == UsedBy.Mount)
        {
            RefreshUIWeapon();
        }

        WeaponData gun = WeaponFactory.instance.weaponsData[weaponToUse];
        BulletData bullet = WeaponFactory.instance.bulletData[WeaponFactory.instance.weaponsData[weaponToUse].bulletToShoot];
        GameObject instantiatedBullet = Instantiate(bullet.bulletGameObject, instantiatedWeapon.transform.FindChild("BulletInstantiator").position, instantiatedWeapon.transform.FindChild("BulletInstantiator").rotation) as GameObject;

        GameObject instantiatedShootingEffect = null;

        if (gun.shootingEffectGameObject != null)
        {
            instantiatedShootingEffect = Instantiate(gun.shootingEffectGameObject, instantiatedWeapon.transform.FindChild("BulletInstantiator").position, instantiatedWeapon.transform.rotation) as GameObject;
            instantiatedShootingEffect.transform.SetParent(instantiatedWeapon.transform, true);
        }

        if (weaponDirection == DirectionsXAxisEnum.Left)
        {
            if (instantiatedShootingEffect != null)
                instantiatedShootingEffect.transform.localScale = new Vector3(instantiatedShootingEffect.transform.localScale.x * -1, instantiatedShootingEffect.transform.localScale.y, instantiatedShootingEffect.transform.localScale.z);

            instantiatedBullet.GetComponent<Rigidbody2D>().velocity = instantiatedBullet.transform.right * bullet.bulletVelocity;
        }
        else if (weaponDirection == DirectionsXAxisEnum.Right)
        {
            instantiatedBullet.GetComponent<Rigidbody2D>().velocity = instantiatedBullet.transform.right * bullet.bulletVelocity;
        }

        instantiatedBullet.GetComponent<BulletOfWeapon>().SetBulletData(bullet.collisionEffectGameObject, weaponDirection, bullet.maxDistance, bullet.bulletType, bullet.countdownToActivateGranade, bullet.activationMode, bullet.explosionOfGrenadeGameObject);
    }

    void DelayToAttack ()
    {
        timerForDelayOfAttack += Time.deltaTime;

        if (timerForDelayOfAttack > WeaponFactory.instance.weaponsData[weaponToUse].delayTimeBeforeAttack)
        {
            if (instantiatedWeapon.GetComponent<Collider2D>() != null)
                instantiatedWeapon.GetComponent<Collider2D>().enabled = true;
#if UNITY_EDITOR
            else
                Debug.LogError("The current weapon of the GameObject " + this.gameObject.name + " should have a Collider2D.");
#endif
            startTimerForDelayOfAttak = false;
        }
    }

    void ReturnMovementTimer()
    {
        timerForStoppingTime += Time.deltaTime;

        if (timerForStoppingTime > WeaponFactory.instance.weaponsData[weaponToUse].stoppingTime)
        {
            if (!WeaponFactory.instance.weaponsData[weaponToUse].iUseACustomMovementScript)
            {
                if ((usedBy == UsedBy.Player || usedBy == UsedBy.Mount) && GetComponent<PlayerHorizontalMovement>() != null)
                {
                    GetComponent<PlayerHorizontalMovement>().enabled = true;
                }

                if (usedBy == UsedBy.Enemy)
                {
                    if (GetComponent<AIEnemyMovement>() != null)
                    {
                        GetComponent<AIEnemyMovement>().movementSpeed = GetComponent<AIEnemyMovement>().originalVelocityOfMovement;
                        GetComponent<AIEnemyMovement>().movementSpeedToFollow = GetComponent<AIEnemyMovement>().originalSpeedToFollow;
                    }

                    if (GetComponent<AIFlyingEnemy>() != null)
                        GetComponent<AIFlyingEnemy>().velocityOfMovement = GetComponent<AIFlyingEnemy>().originalVelocity;
                }
            }
            else
            {
                if (GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) != null)
                {
                    MonoBehaviour script = GetComponent(WeaponFactory.instance.weaponsData[weaponToUse].movementScript) as MonoBehaviour;
                    script.enabled = true;
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError("The script " + WeaponFactory.instance.weaponsData[weaponToUse].movementScript + " does not exist. Go to the Weapon Factory and correct it at the weapon " + WeaponFactory.instance.weaponNames[weaponToUse]);
                }
#endif
            }

            startTimerToStopMovement = false;
        }
    }

    void ChangeBulletDirection(DirectionsXAxisEnum direction)
    {
        if (direction == DirectionsXAxisEnum.Left)
        {
            instantiatedWeapon.transform.FindChild("BulletInstantiator").localEulerAngles = new Vector3(instantiatedWeapon.transform.rotation.x, instantiatedWeapon.transform.rotation.y,
            (((instantiatedWeapon.transform.eulerAngles.z) * -1) * 2f) + 180);
            weaponDirection = DirectionsXAxisEnum.Left;
        }
        else if (direction == DirectionsXAxisEnum.Right)
        {
            instantiatedWeapon.transform.FindChild("BulletInstantiator").localEulerAngles = originalRotationOfWeapon;
            weaponDirection = DirectionsXAxisEnum.Right;
        }
    }
}
