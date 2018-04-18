using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour {

    LevelLoader levelLoader;

    GameObject Player;

    // Use this for initialization
    private void Awake()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        Player = GameObject.Find("Player");
    }

    void Start () {
		if (levelLoader.CameFromPrevLvl)
        {
            Player.transform.position = gameObject.transform.GetChild(0).transform.position;
            Player.transform.rotation = gameObject.transform.GetChild(0).transform.rotation;

        }
        else
        {
            Player.transform.position = gameObject.transform.GetChild(1).transform.position;
            Player.transform.rotation = gameObject.transform.GetChild(1).transform.rotation;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
