using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoor : MonoBehaviour {
    GameObject door;
    bool trigg = false;
    Vector3 startPos;
    Vector3 endPos;

    private void Awake()
    {
        door = GameObject.Find("asset_env_cave_main_gate_bars");
        startPos = transform.position;
        endPos = startPos + (Vector3.down * 6);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Throwable" && !trigg)
        {
            //Que sounds effect here.
            //Move the lever here.
            StartCoroutine(OpenDoor());
            trigg = true;
            
        }
    }

    //door opening.
    IEnumerator OpenDoor()
    {

        while (door.transform.position.y > endPos.y)
        {


            door.transform.Translate(Vector3.down / 25);


            yield return new WaitForSeconds(0.05f);

        }

        yield break;
    }
}
