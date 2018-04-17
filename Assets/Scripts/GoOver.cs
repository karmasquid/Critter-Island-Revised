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
        else
        {
            target = GameObject.Find("HolePoint B");
        }

    }
}
