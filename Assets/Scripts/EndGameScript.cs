using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour {
    IEnumerator coroutine;
    Animator anim;
    [SerializeField]
    GameObject EndOfGame;

    bool trigEnd = false;

    // Use this for initialization
    void Start () {
        anim = GameObject.Find("asset_prfblevel_cave_treasure_orb").GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            anim.SetTrigger("close");
            if (InputManager.Interact() && trigEnd == false)
            {
                trigEnd = true;
                coroutine = EndTheGame(5f);
                StartCoroutine(coroutine);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            //anim.SetTrigger("rotation");
        }
    }
    IEnumerator EndTheGame(float waitTime)
    {
        Instantiate(EndOfGame);
        yield return new WaitForSeconds(waitTime);
        EndOfGame.SetActive(false);
        LevelLoader.instance.Loadlevel(0);
        Destroy(GameObject.Find("ItemDatabase"));
        Destroy(GameObject.Find("Inventory"));
        Destroy(GameObject.Find("PlayerManager"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("Canvas"));
        trigEnd = false;

    }
}
