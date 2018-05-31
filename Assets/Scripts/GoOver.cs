using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoOver : MonoBehaviour
{
    //Creates two public gameobjects.
    public GameObject current;
    public GameObject target;

	// Use this for initialization
	void Start()
    {
        current = this.gameObject; //Sets the current gameobject to the game object this script is being called from.


        //Checking the name of the current gameobject and based on that sets the opposite target.
        if (current.name == "HolePoint B")
        {
            target = GameObject.Find("HolePoint A");
        }
        else if (current.name == "HolePoint A")
            {
            target = GameObject.Find("HolePoint B");
        }
        else if (current.name == "HolePoint D")
        {
            target = GameObject.Find("HolePoint C");
        }
        else if (current.name == "HolePoint C")
        {
            target = GameObject.Find("HolePoint D");
        }

    }
}//Mattias Eriksson
