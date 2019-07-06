using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using EkumeSavedData.WeaponInventory;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class BulletData
{
    public enum Type { NormalBullet, Grenade }
    public enum ActivationMode { WhenCollides, WhenItIsInstantiated }

    public GameObject bulletGameObject;
    public GameObject collisionEffectGameObject;
    public float bulletVelocity;
    public float maxDistance;
    public Type bulletType;
    public float countdownToActivateGranade;
    public GameObject explosionOfGrenadeGameObject;
    public ActivationMode activationMode;

    public BulletData(GameObject _bulletGameObject, GameObject _collisionEffectGameObject, float _bulletVelocity, float _maxDistance, Type _bulletType, float _countdownToActivateGranade, ActivationMode _activationMode, GameObject _explosionOfGrenadeGameObject)
    {
        this.bulletGameObject = _bulletGameObject;
        this.collisionEffectGameObject = _collisionEffectGameObject;
        this.bulletVelocity = _bulletVelocity;
        this.maxDistance = _maxDistance;
        this.bulletType = _bulletType;
        this.countdownToActivateGranade = _countdownToActivateGranade;
        this.activationMode = _activationMode;
        this.explosionOfGrenadeGameObject = _explosionOfGrenadeGameObject;
    }
}

[Serializable]
public class WeaponData
{
    public GameObject weaponGameObject;
    public GameObject shootingEffectGameObject;
    public int defaultBulletQuantity;
    public bool infiniteBullets;
    public float timeToNextShot; //Time to next shot: the time to wait to attack again
    public int bulletToShoot;
    public bool startInWeaponInventory;
    public WeaponFactory.WeaponType weaponType;
    public float attackDuration; //Attack duration: time that the collider of the weapon will be enabled
    public int weaponCategory;
    public bool stopMovementWhenAttack; //Stop movement when attack: if you want to stop the movement of the enemy/player when press the input to attack
    public bool iUseACustomMovementScript;
    public string movementScript;
    public float stoppingTime; //Time to keep stopped when attack
    public float delayTimeBeforeAttack; //Delay time before attack: the delay before activate the collider to attack
    public float delayTimeBeforeShotActivation; //Delay time before shot activation. This is a delay to wait before to send the shot, for example to wait the animation to up the weapon.

    public WeaponData(GameObject _weaponGameObject, GameObject _shotEffectGameObject, int _defaultBulletQuantity, bool _noLimitsOfBullet, float _timeToNextShot, int _bulletToShoot, WeaponFactory.WeaponType _weaponType, float _attackDuration, int _weaponCategory, bool _stopMovementWhenAttack, bool _iUseACustomMovementScript, string _movementScript,  float _delayTimeBeforeAttack, float _delayTimeBeforeShotActivation)
    {
        this.weaponGameObject = _weaponGameObject;
        this.shootingEffectGameObject = _shotEffectGameObject;
        this.defaultBulletQuantity = _defaultBulletQuantity;
        this.infiniteBullets = _noLimitsOfBullet;
        this.timeToNextShot = _timeToNextShot;
        this.bulletToShoot = _bulletToShoot;
        this.weaponType = _weaponType;
        this.attackDuration = _attackDuration;
        this.weaponCategory = _weaponCategory;
        this.stopMovementWhenAttack = _stopMovementWhenAttack;
        this.iUseACustomMovementScript = _iUseACustomMovementScript;
        this.movementScript = _movementScript;
        this.delayTimeBeforeAttack = _delayTimeBeforeAttack;
        this.delayTimeBeforeShotActivation = _delayTimeBeforeShotActivation;
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class WeaponFactory : ScriptableObject
{
    public enum WeaponType { Gun, MeleeWeapon };
    public enum TypeSave { RestartBulletsOfCurrentGunWhenRestartLevel, RestartBulletsOfAllGunsWhenRestartLevel, SaveTheQuantityOfBullets }

    public List<string> weaponCategories = new List<string>() { "Default" };
    public List<BulletData> bulletData = new List<BulletData>();
    public List<WeaponData> weaponsData = new List<WeaponData>();
    public List<string> bulletNames = new List<string>();
    public List<string> weaponNames = new List<string>();
    public int gunToStartByFirstTime;
    public bool startWithGunTheFirsTime;
    public TypeSave savingTypeOfBullets;

    static WeaponFactory reference;
    public static WeaponFactory instance
    {
        get
        {
            if (reference == null)
            {
                reference = (WeaponFactory)Resources.Load("Data/WeaponFactory", typeof(WeaponFactory));

                if (PlayerPrefs.GetInt("WasInitializedDefaultWeapons", 0) == 0)
                {
                    for (int i = 0; i < reference.weaponNames.Count; i++)
                    {
                        if (reference.weaponsData[i].startInWeaponInventory)
                        {
                            Weapons.SetWeaponToInventory(i);
                        }
                    }

                    PlayerPrefs.SetInt("WasInitializedDefaultWeapons", 1);
                }

                return reference;
            }
            else
                return reference;
        }
    }
}
