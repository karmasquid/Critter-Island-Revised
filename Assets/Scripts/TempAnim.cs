using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnim : MonoBehaviour {
    bool moving, idle, dodge, dodgeB, throwing, death;

    public Animator anim;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            print("BOOP");
            anim.SetBool("isRunning", true);
        }
       if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))
        {
            anim.SetTrigger("fDodge");
        }
    }
	
	void Start () {
        anim = GetComponent<Animator>();
        moving = false;

        
    }


}

