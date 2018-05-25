using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TEST_LevelLoader : MonoBehaviour
{
    //Script för att ladda nivåer. Skulle initialt köra en  laddningsscen mellan nivåer men detta har kommenterats bort eftersom det inte
    //fungerade tillsammans med ChooseNextScene nedan. Planerar att ordna detta så att vi får en laddningsscen mellan scenarna senare.
    public GameObject loadingScreen;
    public Slider slider;
    public Animator animator;
    private int levelToLoad;

    public static TEST_LevelLoader instance;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Loadlevel(5);
        }
    }



    public void Loadlevel(int sceneIndex)
    {
        levelToLoad = sceneIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeComplete()
    {
        StartCoroutine(LoadAsynch(levelToLoad));
    }

    IEnumerator LoadAsynch(int sceneIndex)
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
//Daniel Laggar & Stina Hedman
