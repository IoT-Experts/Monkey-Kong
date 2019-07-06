using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour, IPointerClickHandler
{
    public string sceneName;

    [Space]
    [SerializeField] bool showLoadingScreen;
    [HideWhenFalse("showLoadingScreen")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] bool fillProgressBar;
    [HideWhenFalse("fillProgressBar")]
    [SerializeField] Image progressBar;

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(LoadNewScene(sceneName));
        Time.timeScale = 1;
    }

    IEnumerator LoadNewScene(string levelToLoad)
    {
        if (showLoadingScreen)
            loadingScreen.SetActive(true);

        if (fillProgressBar)
            progressBar.fillAmount = 0;

        if (showLoadingScreen || fillProgressBar)
            yield return new WaitForSeconds(0.1f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(levelToLoad);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.

        while (!loadingScene.isDone)
        {
            if (fillProgressBar)
                progressBar.fillAmount = loadingScene.progress;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
