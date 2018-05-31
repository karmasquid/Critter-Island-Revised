using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDoor : MonoBehaviour {
    //Script that handles the door(s) with levers.

    //Private variables.
    GameObject door;
    bool trigg = false;
    Vector3 startPos;
    Vector3 endPos;

    private void Awake()
    {
        //Sets the door, the startposition of that door and the end position where that door should go.
        door = GameObject.Find("asset_env_cave_main_gate_bars");
        startPos = transform.position;
        endPos = startPos + (Vector3.down * 6);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Throwable" && !trigg) //If the colliding gameobject is a throwable object, that way stones/knifes can be used to open the door.
        {
            //Add sound effect and animation to the lever when activated.
            StartCoroutine(OpenDoor());
            trigg = true; //Since it's only used once this bool is used to lock the ontrigger check for future purposes.
            
        }
    }

    //door opening.
    IEnumerator OpenDoor()
    {

        while (door.transform.position.y > endPos.y) //While loop that opens the door by transform.translating it.
        {


            door.transform.Translate(Vector3.down / 25);


            yield return new WaitForSeconds(0.05f);

        }

        yield break;
    }
}//Mattias Eriksson
