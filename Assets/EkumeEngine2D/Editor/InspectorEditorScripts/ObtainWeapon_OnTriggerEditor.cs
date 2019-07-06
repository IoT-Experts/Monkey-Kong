using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ObtainWeapon_OnTrigger))]

public class ObtainWeapon_OnTriggerEditor : Editor
{
    ObtainWeapon_OnTrigger obtainWeapon;

    void OnEnable()
    {
        obtainWeapon = (ObtainWeapon_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 160;
            obtainWeapon.gunToObtain = EditorGUILayout.Popup("Weapon to obain: ", obtainWeapon.gunToObtain, ConvertListStringToArray(obtainWeapon.weaponFactory.weaponNames));
            using (new GUILayout.VerticalScope("box"))
            {
                obtainWeapon.saveInWeaponInventory = EditorGUILayout.Toggle("Save in weapon inventory: ", obtainWeapon.saveInWeaponInventory);
                if (obtainWeapon.saveInWeaponInventory)
                    obtainWeapon.useOnlyForThisLevel = false;
                obtainWeapon.useOnlyForThisLevel = EditorGUILayout.Toggle("Use only for this level: ", obtainWeapon.useOnlyForThisLevel);
                if (obtainWeapon.useOnlyForThisLevel)
                    obtainWeapon.saveInWeaponInventory = false;
            }
            obtainWeapon.placeToPlayer = EditorGUILayout.Toggle("Place to player: ", obtainWeapon.placeToPlayer);
        }

        if (!obtainWeapon.weaponFactory.weaponsData[obtainWeapon.gunToObtain].infiniteBullets)
        {
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUILayout.LabelField("What to do if player have this gun", EditorStyles.boldLabel);
                using (new GUILayout.VerticalScope("box"))
                {
                    EditorGUIUtility.labelWidth = 175;
                    obtainWeapon.addBulletsIfItHaveTheGun = EditorGUILayout.Toggle("Add value of default bullets: ", obtainWeapon.addBulletsIfItHaveTheGun);
                    if (obtainWeapon.addBulletsIfItHaveTheGun)
                        obtainWeapon.restartBulletsIfItHaveTheGun = false;

                    obtainWeapon.restartBulletsIfItHaveTheGun = EditorGUILayout.Toggle("Restart bullets to default: ", obtainWeapon.restartBulletsIfItHaveTheGun);
                    if (obtainWeapon.restartBulletsIfItHaveTheGun)
                        obtainWeapon.addBulletsIfItHaveTheGun = false;
                }
            }
        }

        EditorUtility.SetDirty(obtainWeapon);
        Undo.RecordObject(obtainWeapon, "Undo obtainWeapon");
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
