using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ObtainBullets_OnTrigger))]

public class ObtainBullets_OnTriggerEditor : Editor
{
    ObtainBullets_OnTrigger obtainBullets;

    void OnEnable()
    {
        obtainBullets = (ObtainBullets_OnTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Give bullets to:", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 120;
                obtainBullets.bulletsToGunThatIsUsing = EditorGUILayout.Toggle("Gun that is using: ", obtainBullets.bulletsToGunThatIsUsing);
                if (obtainBullets.bulletsToGunThatIsUsing)
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        for (int i = 0; i < obtainBullets.weaponExceptions.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUIUtility.labelWidth = 90;
                            obtainBullets.weaponExceptions[i] = EditorGUILayout.Popup("Except if it is: ", obtainBullets.weaponExceptions[i], ConvertListStringToArray(obtainBullets.weaponFactory.weaponNames));
                            if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(30)))
                            {
                                obtainBullets.weaponExceptions.RemoveAt(i);
                            }
                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button("Add weapon exception", EditorStyles.miniButton))
                        {
                            obtainBullets.weaponExceptions.Add(0);
                        }
                    }

                    if (obtainBullets.weaponExceptions.Count > 0)
                    {
                        EditorGUIUtility.labelWidth = 110;
                        EditorGUILayout.LabelField("If the gun that is using is in the exceptions, then give bullets to:", EditorStyles.miniLabel);
                    }

                    obtainBullets.gunIfException = EditorGUILayout.Popup("Alternative gun: ", obtainBullets.gunIfException, ConvertListStringToArray(obtainBullets.weaponFactory.weaponNames));
                    EditorGUILayout.LabelField("*If player doesn't have gun currently, will give the bullets to this gun.", EditorStyles.miniLabel);

                }
            }
            using (new GUILayout.VerticalScope("box"))
            {
                EditorGUIUtility.labelWidth = 80;
                obtainBullets.giveBulletsToSpecificGun = EditorGUILayout.Toggle("Specific gun: ", obtainBullets.giveBulletsToSpecificGun);

                if (obtainBullets.giveBulletsToSpecificGun)
                {
                    obtainBullets.specificGun = EditorGUILayout.Popup("Gun: ", obtainBullets.specificGun, ConvertListStringToArray(obtainBullets.weaponFactory.weaponNames));
                }
            }
        }

        using (new GUILayout.VerticalScope("box"))
        {
            EditorGUIUtility.labelWidth = 100;
            obtainBullets.bulletsQuantity = EditorGUILayout.IntField("Bullets quantity: ", obtainBullets.bulletsQuantity);
        }

        EditorUtility.SetDirty(obtainBullets);
        Undo.RecordObject(obtainBullets, "Undo obtainBullets");
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
