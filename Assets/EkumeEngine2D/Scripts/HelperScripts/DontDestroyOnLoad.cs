using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    enum HowToDestroyDuplicated { DestroyDuplicateByName, DestroyDuplicateByType, None }
    enum SelectionOptions { KeepInTheNextScenes, DestroyInTheNextScenes }

    [SerializeField] HowToDestroyDuplicated howToDestroyDuplicated;
    [SerializeField] SelectionOptions whatToDo;
    [SerializeField] List<string> scenes = new List<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += SceneChanged;

        if (howToDestroyDuplicated == HowToDestroyDuplicated.DestroyDuplicateByType)
        {
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
        }
        else if (howToDestroyDuplicated == HowToDestroyDuplicated.DestroyDuplicateByName)
        {
            if (GameObject.Find(gameObject.name)
              && GameObject.Find(gameObject.name) != this.gameObject)
            {
                Destroy(gameObject);
            }
        }

        VerifyDestruction(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // unsubscribe
    }

    void SceneChanged(Scene previousScene, Scene newScene)
    {
        VerifyDestruction(newScene.name);
    }

    void VerifyDestruction (string sceneToCompare)
    {
        if (whatToDo == SelectionOptions.DestroyInTheNextScenes && scenes.Contains(sceneToCompare))
        {
            Destroy(gameObject);
        }
        else if (whatToDo == SelectionOptions.KeepInTheNextScenes && !scenes.Contains(sceneToCompare))
        {
            Destroy(gameObject);
        }
    }
}
