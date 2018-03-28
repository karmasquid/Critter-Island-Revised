using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnim : MonoBehaviour {
    bool moving, idle, dodge, dodgeB, throwing, death;
    float chargeTimer;


    public Animator anim;

    void Update ()
    {
       /* chargeTimer += Time.deltaTime;
        if(chargeTimer >= 1.0f)
            { anim.SetBool("isCharging", true);
            if (Input.GetKeyUp(KeyCode.H))
            {
                anim.SetTrigger("chargeAttack");
            }

         } */

        if (Input.GetKeyDown(KeyCode.O))
        {
            anim.SetBool("isCharging", true);
            if (Input.GetKeyUp(KeyCode.O))
            {
                anim.SetTrigger("chargeAttack");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("attack");

        }
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetTrigger("throw");
        }
    }
	
	void Start () {
        anim = GetComponent<Animator>();
        moving = false;

        
    }


}

