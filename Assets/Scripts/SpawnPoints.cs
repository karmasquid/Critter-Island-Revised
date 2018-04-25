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
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        Player = GameObject.Find("Player");

    }

    void Start () {

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

} // STina Hedman

