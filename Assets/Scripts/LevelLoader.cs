using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void Loadlevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynch(sceneIndex));
    }

    IEnumerator LoadAsynch (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //Gör om Unitys laddningsprocent till ett tal mellan 0 - 1 istället för Unitys 0 - 0.9
            float progress = Mathf.Clamp01(operation.progress) / 0.9f;
            slider.value = progress;

            yield return null;
        }
    }

}
