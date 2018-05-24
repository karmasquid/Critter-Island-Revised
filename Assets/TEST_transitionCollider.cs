using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_transitionCollider : MonoBehaviour {

    TEST_LevelLoader levelLoader;

    private void Awake()
    {
        //levelLoader = GameObject.Find("").GetComponent<>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelLoader.Loadlevel(5);
        }

    }
}
