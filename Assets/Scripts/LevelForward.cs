using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelForward : MonoBehaviour {

    LevelLoader Level_Loader;
    [SerializeField]
    bool nextLevel;

    // Use this for initialization
    void Awake () {

        Level_Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Level_Loader.ChooseNextScene(nextLevel);
        } 
        
    }

}
