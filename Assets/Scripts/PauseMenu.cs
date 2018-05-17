using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

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
    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    //Pausar spelet och aktiverar pausmenyn.
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

	}