using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using EkumeEnumerations;
using UnityEngine.UI;

[CustomEditor(typeof(UseWeapon))]

public class UseWeaponEditor : Editor
{
    UseWeapon weaponToShoot;

    void OnEnable()
    {
        weaponToShoot = (UseWeapon)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 150;
            weaponToShoot.usedBy = (UseWeapon.UsedBy)EditorGUILayout.EnumPopup("Weapon used by: ", weaponToShoot.usedBy);
            using (new GUILayout.VerticalScope("box"))
            {
                if (weaponToShoot.usedBy == UseWeapon.UsedBy.Player)
                    weaponToShoot.weaponTypeToUse = (UseWeapon.WeaponTypeToUse)EditorGUILayout.EnumPopup("What weapon use?: ", weaponToShoot.weaponTypeToUse);
                else
                    weaponToShoot.weaponTypeToUse = UseWeapon.WeaponTypeToUse.UseSpecificWeapon;

                if (weaponToShoot.weaponTypeToUse == UseWeapon.WeaponTypeToUse.UseSpecificWeapon)
                    weaponToShoot.weaponToUse = EditorGUILayout.Popup("Specific weapon: ", weaponToShoot.weaponToUse, ConvertListStringToArray(WeaponFactory.instance.weaponNames));

                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 197;
                    weaponToShoot.automaticShootingWithTarget = EditorGUILayout.Toggle("Automatic Attack (With target): ", weaponToShoot.automaticShootingWithTarget);
                    if(weaponToShoot.automaticShootingWithTarget)
                        weaponToShoot.targetDetector = EditorGUILayout.ObjectField("Target detector: ", weaponToShoot.targetDetector, typeof(TargetDetector), true) as TargetDetector;
                }
                EditorGUIUtility.labelWidth = 150;

                if (weaponToShoot.usedBy != UseWeapon.UsedBy.Enemy && !weaponToShoot.automaticShootingWithTarget)
                    weaponToShoot.inputControl = EditorGUILayout.Popup("Input control: ", weaponToShoot.inputControl, ConvertListStringToArray(InputControlsManager.instance.inputNames));

                weaponToShoot.weaponPosition = EditorGUILayout.ObjectField("Weapon position: ", weaponToShoot.weaponPosition, typeof(Transform), true) as Transform;

            }

            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 210;
                weaponToShoot.enablePlayerStatesExceptions = EditorGUILayout.Toggle("Exceptions to attack (Player states): ", weaponToShoot.enablePlayerStatesExceptions);
                if (weaponToShoot.enablePlayerStatesExceptions)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        for (int i = 0; i < weaponToShoot.playerStatesExceptions.Count; i++)
                        {
                            using (new GUILayout.HorizontalScope("box"))
                            {
                                weaponToShoot.playerStatesExceptions[i] = (PlayerStatesEnum)EditorGUILayout.EnumPopup(weaponToShoot.playerStatesExceptions[i]);
                                if (GUILayout.Button("X", EditorStyles.miniButton))
                                {
                                    weaponToShoot.playerStatesExceptions.RemoveAt(i);
                                }
                            }
                        }
                        if (GUILayout.Button("Add exception", EditorStyles.miniButton))
                        {
                            weaponToShoot.playerStatesExceptions.Add(PlayerStatesEnum.PlayerLoseLevel);
                        }
                    }
                }
            }

            if (weaponToShoot.usedBy != UseWeapon.UsedBy.Enemy)
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField("User interface", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 155;

                    using (new GUILayout.VerticalScope("box"))
                    {
                        weaponToShoot.showGunImage = EditorGUILayout.Toggle("Show weapon image: ", weaponToShoot.showGunImage);

                        if (weaponToShoot.showGunImage)
                            weaponToShoot.uIOfGun = EditorGUILayout.ObjectField("UI to show weapon image: ", weaponToShoot.uIOfGun, typeof(Image), true) as Image;
                    }

                    using (new GUILayout.VerticalScope("box"))
                    {
                        weaponToShoot.showBulletImage = EditorGUILayout.Toggle("Show bullet image: ", weaponToShoot.showBulletImage);

                        if (weaponToShoot.showBulletImage)
                        {
                            weaponToShoot.uIOfBullet = EditorGUILayout.ObjectField("UI to show bullet image: ", weaponToShoot.uIOfBullet, typeof(Image), true) as Image;

                            bool someMeleeWeapon = false;
                            for (int i = 0; i < WeaponFactory.instance.weaponsData.Count; i++)
                            {
                                if (WeaponFactory.instance.weaponsData[i].weaponType == WeaponFactory.WeaponType.MeleeWeapon)
                                {
                                    someMeleeWeapon = true;
                                    break;
                                }
                            }
                            if(someMeleeWeapon)
                                weaponToShoot.iconIfUseMeleeWeapon = EditorGUILayout.ObjectField("Icon if use melee weapon: ", weaponToShoot.iconIfUseMeleeWeapon, typeof(Sprite), true) as Sprite;
                        }
                    }

                    using (new GUILayout.VerticalScope("box"))
                    {
                        weaponToShoot.showBulletQuantity = EditorGUILayout.Toggle("Show bullet quantity: ", weaponToShoot.showBulletQuantity);

                        if (weaponToShoot.showBulletQuantity)
                        {
                            weaponToShoot.textOfBullets = EditorGUILayout.ObjectField("Text of bullets: ", weaponToShoot.textOfBullets, typeof(Text), true) as Text;
                            weaponToShoot.formatBulletQuantity = EditorGUILayout.TextField("Format for quantity: ", weaponToShoot.formatBulletQuantity);

                            if (!weaponToShoot.formatBulletQuantity.Contains("0"))
                            {
                                EditorGUILayout.HelpBox("The format for quantity should have at least a 0.", MessageType.Warning);
                            }

                            weaponToShoot.textInfiniteBullets = EditorGUILayout.TextField("Text for infinite bullets: ", weaponToShoot.textInfiniteBullets);
                        }
                    }
                }
            }
        }

        EditorUtility.SetDirty(weaponToShoot);
        Undo.RecordObject(weaponToShoot, "Undo weaponToShoot");
    }

    string[] ConvertListStringToArray(List<string> list)
    {
        string[] arrayToReturn = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            arrayToReturn[i] = list[i];
        }

        return arrayToReturn;
    }
}
