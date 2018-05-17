using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    //Script för att
    public static bool GamePaused = false;
    public GameObject pauseMenu;
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))//TODO: Behöver få stöd för Input från kontroll.
        {
            if (GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        }

    //Återaktiverar spelet och inaktiverar pausmenyn.
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    //Pausar spelet och aktiverar pausmenyn.
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Cya");
        Application.Quit();
    }

}
//Daniel Laggar