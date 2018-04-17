using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelForward : MonoBehaviour {

    LevelLoader Level_Loader;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Level_Loader.Loadlevel(+1);
        }
        
    }

    // Use this for initialization
    void Start () {

        GetComponent<LevelLoader>();
        Level_Loader = Level_Loader.GetComponent<LevelLoader>();


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
