using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

    LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = this.transform.parent.GetComponent<LevelLoader>();
    }

    public void FadeComplete()
    {
        levelLoader.FadeIsComplete();
    }
}
