using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameConfigCategory
{
    public enum DefaultValue { Enabled, Disabled }


    public string categoryName = "NAME";
    public DefaultValue defaultValueOfThisCategory;
    public List<ObjectData> objects = new List<ObjectData>();
    public Toggle toggle;
}

[System.Serializable]
public class ObjectData
{
    public enum Type { GameObject, Component, Script }
    public enum Actions1 { NONE, Destroy, Enable, Disable }
    public enum Actions2 { NONE, Destroy }

    public Type type;
    /// <summary>
    /// Component name or gameobject name (depending on type)
    /// </summary>
    public string name;

    public GameObject objectFound;
    public Component componentFound;
    public Behaviour scriptFound;

    public string gameObjectWithTheComponent;
    public Actions1 action1OnEnable; //For GameObject and Script
    public Actions1 action1OnDisable; //For GameObject and Script

    public Actions2 action2OnEnable; //For Components
    public Actions2 action2OnDisable; //For Components
}

public class GameConfigs : MonoBehaviour
{
    public List<GameConfigCategory> configCategories = new List<GameConfigCategory>();

    public bool keepObjectInOtherScenes = true;

    string originalScene;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        originalScene = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += SceneChanged;

        GameConfigs[] gameConfigs = FindObjectsOfType<GameConfigs>();
        if (gameConfigs.Length > 1)
        {
            foreach (GameConfigs gameConfig in gameConfigs)
            {
                if (gameConfig.gameObject != this.gameObject)
                    Destroy(gameConfig.gameObject);
            }
        }
    }

    void Start()
    {
        foreach (GameConfigCategory category in configCategories)
        {
            foreach (ObjectData obj in category.objects)
            {
                obj.objectFound = null;
                obj.scriptFound = null;
                obj.componentFound = null;

                if (obj.type == ObjectData.Type.GameObject)
                {
                    GameObject objFound = GameObject.Find(obj.name);
                    if (objFound != null)
                    {
                        obj.objectFound = objFound;
                    }
                }
                else if(obj.type == ObjectData.Type.Component)
                {
                    GameObject objFound = GameObject.Find(obj.gameObjectWithTheComponent);
                    if (objFound != null && objFound.GetComponent(obj.name) != null)
                    {
                        obj.componentFound = objFound.GetComponent(obj.name);
                    }
                }
                else if (obj.type == ObjectData.Type.Script)
                {
                    GameObject objFound = GameObject.Find(obj.gameObjectWithTheComponent);
                    if(objFound != null && objFound.GetComponent(obj.name) != null)
                    {
                        obj.scriptFound = objFound.GetComponent(obj.name) as Behaviour;
                    }
                }
            }

            if (!PlayerPrefs.HasKey("toggleValue" + category.categoryName))
            {
                PlayerPrefs.SetInt("toggleValue" + category.categoryName, (category.defaultValueOfThisCategory == GameConfigCategory.DefaultValue.Enabled) ? 1 : 0);
                if (category.toggle != null)
                {
                    category.toggle.isOn = (category.defaultValueOfThisCategory == GameConfigCategory.DefaultValue.Enabled) ? true : false;
                }
            }
            else
            {
                if (category.toggle != null)
                {
                    category.toggle.isOn = (PlayerPrefs.GetInt("toggleValue" + category.categoryName) == 1) ? true : false;
                }
            }
        }

        foreach (GameConfigCategory category in configCategories)
        {
            if (category.toggle != null)
                category.toggle.onValueChanged.AddListener(delegate { ToggleChangedValue(); });
        }

        ToggleChangedValue();
    }

    void ToggleChangedValue()
    {
        foreach (GameConfigCategory category in configCategories)
        {
            if (category.toggle != null)
            {
                RefreshObjectsStates(category.toggle.isOn, category);
                PlayerPrefs.SetInt("toggleValue" + category.categoryName, (category.toggle.isOn) ? 1 : 0);
            }
            else
            {
                RefreshObjectsStates(((PlayerPrefs.GetInt("toggleValue" + category.categoryName) == 1) ? true : false), category);
            }
        }
    }

    void RefreshObjectsStates(bool toggleValue, GameConfigCategory category)
    {
        foreach (ObjectData obj in category.objects)
        {
            if (obj.type == ObjectData.Type.GameObject)
            {
                if (obj.objectFound != null)
                {
                    if (toggleValue)
                    {
                        switch (obj.action1OnEnable)
                        {
                            case ObjectData.Actions1.Destroy:
                                Destroy(obj.objectFound);
                                break;

                            case ObjectData.Actions1.Enable:
                                obj.objectFound.SetActive(true);
                                break;

                            case ObjectData.Actions1.Disable:
                                obj.objectFound.SetActive(false);
                                break;
                        }
                    }
                    else
                    {
                        switch (obj.action1OnDisable)
                        {
                            case ObjectData.Actions1.Destroy:
                                Destroy(obj.objectFound);
                                break;

                            case ObjectData.Actions1.Enable:
                                obj.objectFound.SetActive(true);
                                break;

                            case ObjectData.Actions1.Disable:
                                obj.objectFound.SetActive(false);
                                break;
                        }
                    }
                }
            }
            else if (obj.type == ObjectData.Type.Component)
            {
                if (obj.componentFound != null)
                {
                    if (toggleValue)
                    {
                        if (obj.action2OnEnable == ObjectData.Actions2.Destroy)
                        {
                            Destroy(obj.componentFound);
                        }
                    }
                    else
                    {
                        if (obj.action2OnDisable == ObjectData.Actions2.Destroy)
                        {
                            Destroy(obj.componentFound);
                        }
                    }
                }
            }
            else if (obj.type == ObjectData.Type.Script)
            {
                if (obj.scriptFound != null)
                {
                    if (toggleValue)
                    {
                        switch (obj.action1OnEnable)
                        {
                            case ObjectData.Actions1.Destroy:
                                Destroy(obj.scriptFound);
                                break;

                            case ObjectData.Actions1.Enable:
                                obj.scriptFound.enabled = true;
                                break;

                            case ObjectData.Actions1.Disable:
                                obj.scriptFound.enabled = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (obj.action1OnDisable)
                        {
                            case ObjectData.Actions1.Destroy:
                                Destroy(obj.scriptFound);
                                break;

                            case ObjectData.Actions1.Enable:
                                obj.scriptFound.enabled = true;
                                break;

                            case ObjectData.Actions1.Disable:
                                obj.scriptFound.enabled = false;
                                break;
                        }
                    }
                }
            }
        }
    }

    void SceneChanged(Scene previousScene, Scene newScene)
    {
        if (originalScene != newScene.name)
            Start();
    }
}
