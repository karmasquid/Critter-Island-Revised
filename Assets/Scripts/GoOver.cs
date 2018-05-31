using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoOver : MonoBehaviour
{
    public GameObject current;
    public GameObject target;

	// Use this for initialization
	void Start()
    {
        current = this.gameObject;
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
}
