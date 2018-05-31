using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventMenu : MonoBehaviour
{
    //Script for the eventsystem, making sure you can't click out of the main menu.

    //Gets the eventsystem and gameobject currently selected.
    EventSystem eS;
    GameObject selectedGameobject;


    void Start()
    {
        //Sets the eventsystem.
        eS = GetComponent<EventSystem>();
        
        //Invokes mentioned metod every half second.
        InvokeRepeating("MenuEventSystemChecker", 0 , 0.5f);
    }
    void MenuEventSystemChecker()
    {
        if (eS.currentSelectedGameObject == null) //If there is no selected element in the menu...
        {
            eS.SetSelectedGameObject(selectedGameobject); //Then set the selected element to the selected element set at the start.
        }
        else
        {
            selectedGameobject = eS.currentSelectedGameObject; //Setting the selected element the current selected element.
        }
    }
}//Mattias Eriksson
