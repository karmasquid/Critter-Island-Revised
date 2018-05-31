using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPoints : MonoBehaviour {

    LevelLoader levelLoader;

    GameObject Player;

    // Use this for initialization
    private void Awake()
    {
        levelLoader = GameObject.Find("LoadingScreen").GetComponent<LevelLoader>();
        Player = GameObject.Find("Player");

    }

    void Start () {

        //checks if player came from previous level or not then places the player at the position and rotation of the spawnpoint.
        if (levelLoader.CameFromPrevLvl)
        {
            Player.GetComponent<NavMeshAgent>().Warp(gameObject.transform.GetChild(0).transform.position);
            Player.transform.rotation = gameObject.transform.GetChild(0).transform.rotation;
        }
        else
        {
            Player.GetComponent<NavMeshAgent>().Warp(gameObject.transform.GetChild(1).transform.position);
            Player.transform.rotation = gameObject.transform.GetChild(1).transform.rotation;
        }

	}

} // Stina Hedman

