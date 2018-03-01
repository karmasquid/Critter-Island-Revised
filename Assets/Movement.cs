using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField]
    float speed;
    [SerializeField]
    float runMultiplier;
    [SerializeField]
    float cooldownTimer;
    [SerializeField]
    bool evading;
    [SerializeField]
    float evadeTimer;
    bool running;

    public float evadeTime; // this tells us how long the evade takes
    public float evadeDistance; // this tells us how far player will evade
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W)) { this.transform.Translate(0, 0, speed); };
        if (Input.GetKey(KeyCode.S)) { this.transform.Translate(0, 0, -speed); };
        if (Input.GetKey(KeyCode.A)) { this.transform.Translate(-speed, 0, 0); };
        if (Input.GetKey(KeyCode.D)) { this.transform.Translate(speed, 0, 0); };
        if (Input.GetKeyDown(KeyCode.LeftShift) && !evading) { speed = speed * runMultiplier; running = true; };
        if (Input.GetKeyUp(KeyCode.LeftShift) && !evading) { speed = speed / runMultiplier; running = false; };

        if (Input.GetKeyDown(KeyCode.Q)) { ProcessEvasion(); }
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);
        if (evading)
        {
            evadeTimer = Mathf.Max(0f, evadeTimer - Time.deltaTime);

            // Evasion overrides speed and direction
            //moveDirection = this.transform.forward; // evasion = full speed forward


                speed = evadeDistance / evadeTime;
           


            if (evadeTimer == 0 )//&& stamina >= 30)
            {
                //anim.SetBool("Evading", false);
                evading = false;
                if (running)
                {
                    speed = 0.1f * runMultiplier;
                }
                else
                {
                    speed = 0.1f;
                }
            }
        }
    }
    void ProcessEvasion()
    {
        
        if (!evading && cooldownTimer == 0)
        {
            evading = true;
            evadeTimer = evadeTime;
            cooldownTimer = 1f;
            //anim.SetTrigger("Evading");
        }
    }
}
