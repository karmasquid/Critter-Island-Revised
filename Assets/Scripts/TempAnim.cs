using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnim : MonoBehaviour {
    bool moving, idle, dodge, dodgeB, throwing, death;

    public Animator anim;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            print("BOOP");
            anim.SetBool("isRunning", true);
        }
       if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("isRunning", false);
        }
    }
	
	void Start () {
        anim = GetComponent<Animator>();
        moving = false;

        
    }


}

