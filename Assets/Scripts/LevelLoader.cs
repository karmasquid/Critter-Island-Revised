using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    //Script för att ladda nivåer. Skulle initialt köra en  laddningsscen mellan nivåer men detta har kommenterats bort eftersom det inte
    //fungerade tillsammans med ChooseNextScene nedan. Planerar att ordna detta så att vi får en laddningsscen mellan scenarna senare.
    public GameObject loadingScreen;
    public Slider slider;
    private bool cameFromPrevLvl = true;
    public bool CameFromPrevLvl
    { get { return cameFromPrevLvl; } }    

    public static LevelLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    public void Loadlevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        //StartCoroutine(LoadAsynch(sceneIndex));
    }

    //Denna del skall användas senare och står därför kvar här. 
    //IEnumerator LoadAsynch(int sceneIndex)
    //{

    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    //    loadingScreen.SetActive(true);

    //    while (!operation.isDone)
    //    {
    //        //Gör om Unitys laddningsprocent till ett tal mellan 0 - 1 istället för Unitys 0 - 0.9
    //        float progress = Mathf.Clamp01(operation.progress) / 0.9f;
    //        slider.value = progress;

    //        yield return null;
    //    }
    //}

    public void ChooseNextScene(bool SceneAfterThis)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneAfterThis)
        {
            Loadlevel(currentIndex + 1);
            cameFromPrevLvl = true;
        }

        else
        {
            Loadlevel(currentIndex - 1);
            cameFromPrevLvl = false;
        }
    }
    

    }
//Daniel Laggar & Stina Hedman
