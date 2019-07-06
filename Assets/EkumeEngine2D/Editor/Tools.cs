#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using EkumeSavedData.Player;
using EkumeEnumerations;

public class Tools
{
    [MenuItem("Tools/Clear all saved data")]
    private static void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif