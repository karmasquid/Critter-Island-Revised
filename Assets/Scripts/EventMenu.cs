using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventMenu : MonoBehaviour
{
    //Scriptet för att alltid ha något element i UI:et 
    EventSystem eS;
    GameObject selectedGameobject;
    void Start()
    {
        eS = GetComponent<EventSystem>();
        InvokeRepeating("MenuEventSystemChecker", 0 , 0.5f);
    }
    void MenuEventSystemChecker()
    {
        if (eS.currentSelectedGameObject == null)
        {
            eS.SetSelectedGameObject(selectedGameobject);
        }
        else
        {
            selectedGameobject = eS.currentSelectedGameObject;
        }
    }
}
