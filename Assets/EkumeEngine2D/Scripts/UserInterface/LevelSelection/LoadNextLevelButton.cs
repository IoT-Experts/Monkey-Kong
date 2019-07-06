using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using EkumeSavedData;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadNextLevelButton : MonoBehaviour, IPointerClickHandler 
{
    [Header("If this scene is a level, and it is not won...")]
    [SerializeField] bool disableButton;
    [SerializeField] bool disableGameObject;

    [Space]
    [SerializeField] bool showLoadingScreen;
    [HideWhenFalse("showLoadingScreen")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] bool fillProgressBar;
    [HideWhenFalse("fillProgressBar")]
    [SerializeField] Image progressBar;

    bool buttonDisabled;

    void OnEnable()
    {
        if (disableButton || disableGameObject)
        {
#if UNITY_EDITOR
            if (Levels.GetLevelIdentificationOfCurrentScene() == "")
            {
                Debug.LogError("Error in the GameObject " + gameObject.name + " with the script " + this.GetType().Name + ". The current scene is not a level or it is not added to the Levels Manager.");
                return;
            }
#endif

            if (!Levels.IsLevelCleared(Levels.GetLevelIdentificationOfCurrentScene()))
            {
                if (disableButton)
                {
                    GetComponent<Button>().interactable = false;
                    buttonDisabled = true;
                }

                if (disableGameObject)
                {
                    gameObject.SetActive(false);
                    buttonDisabled = true;
                }
            }
        }
        StartCoroutine(CheckIfCanLoadNextLevel());
    }

    IEnumerator CheckIfCanLoadNextLevel ()
    {
        yield return new WaitForSeconds(0.1f);
        if (Levels.GetSceneNameOfNextLevel() == "")
        {
            Debug.Log("The component " + GetType().Name + " will not load the next level beacuse it does not exist. (Maybe all the levels have been won, or the next level is null in the Levels Manager) - (GameObject: " + gameObject.name + ")");
            GetComponent<Button>().interactable = false;
            buttonDisabled = true;
        }
    }

    //This script loads the next level. Searches what is the last level that cleared and if the next level is not clear will load that level.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!buttonDisabled)
        {
            StartCoroutine(LoadNewScene(Levels.GetSceneNameOfNextLevel()));

            Time.timeScale = 1;
        }
    }

    IEnumerator LoadNewScene(string levelToLoad)
    {
        if (showLoadingScreen)
            loadingScreen.SetActive(true);

        if (fillProgressBar)
            progressBar.fillAmount = 0;

        if(showLoadingScreen || fillProgressBar)
            yield return new WaitForSeconds(0.1f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(levelToLoad);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!loadingScene.isDone)
        {
            if(fillProgressBar)
                progressBar.fillAmount = loadingScene.progress;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
