using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Script för att stänga av programmet. Övriga funktioner för huvudmenyn finns på menyns komponenter i inspektorn.
    public void QuitGame()
    {
        Debug.Log("Cya");
        Application.Quit();
    }
	
}

//Daniel Laggar
