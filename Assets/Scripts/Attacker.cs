using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    [SerializeField]
    Collider[] attackHitBoxes;
    [SerializeField]
    GameObject[] projectiles; //Set size once we know amount of consumables.

    GameObject Currentequipped;
    bool chargingAttack;
    bool throwing = false;
    bool hit = false;
    float chargeTimer;
    string CurrentName;

    IEnumerator Reload;

    void Start ()
    {
         Currentequipped = projectiles[0];
    }

    void Update ()
    {
        //-------------------------------------Main Attacks & Controlls------------------------------------------------
        if (Input.GetKey(KeyCode.H) || Input.GetKey("joystick button 2"))
        {
            chargingAttack = false;
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 1.0f)
            { chargingAttack = true; }
        }
        if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp("joystick button 2"))
        {
            CheckWeapon();
            Attacking();
        }
        if (Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp("joystick button 3")) // && Ammo count != 0
        {
            //GameObject Currentequipped = projectiles[0];
            //CurrentName = Currentequipped.name;
            if (!throwing)
            { 
            GameObject preo = Instantiate(Currentequipped) as GameObject; //Change index deppending on item equipped.
            preo.transform.position = transform.position + attackHitBoxes[2].transform.up;
            Rigidbody rb = preo.GetComponent<Rigidbody>();
            rb.velocity = attackHitBoxes[2].transform.forward * 20;
                throwing = true;
                Reload = ThrowCD(2.0f);
                StartCoroutine(Reload);
                if (preo != null)
            {
                attackHitBoxes[3] = preo.GetComponent<Collider>();

            }
            else
            {
                attackHitBoxes[3] = null;
            }


            Destroy(preo, Time.deltaTime + 2f);
            }

            
            /*
            switch (Currentequipped.tag)
            {
                case "Throwable":
                    GameObject preo = Instantiate(Currentequipped) as GameObject; //Change index deppending on item equipped.
                    preo.transform.position = transform.position + attackHitBoxes[2].transform.up;
                    Rigidbody rb = preo.GetComponent<Rigidbody>();
                    rb.velocity = attackHitBoxes[2].transform.forward * 20;

                    break;
                case "Eatable":
                    Debug.Log(CurrentName);
                    //Check what type of eatable (Recover hp)

                    break;
                default:
                    debug.log("Nothing equipped");
            }
            */
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4") && chargingAttack == true)
        {
            chargingAttack = false;
            chargeTimer = 0;
        }


        //----------------------------------------Invokes for throw------------------------------------------------------
        if (throwing)
        {
            InvokeRepeating("DetectedThrow", 0f, 4f);
        }
        if (hit)
        {
            Invoke("Throwdamage", 0f);
        }
    }

    void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach (Collider c in cols)
        {
            if (c.transform.root == gameObject.transform) // Slå inte dig själv...
            { continue; }
            if (c.gameObject.tag == "Enemy")
            {
                hit = true;
            }

        }

    }
    
    //------------------------------------Handles damage------------------------------------------------------------
    void CheckWeapon()
    {
            /*
            float dmg = 0;
            switch (Weapon[])
            {
                case "Hammer" :
                    dmg = 20;
                    break;
                case "Rusty Sword" :
                    dmg = 10;
                    break;
            }
            getcomponent, attack script. Assign other colliders fitting the weapon.
            */
    }
    void Throwdamage()
    {
        switch (Currentequipped.name)
        {
            case "IcaBasicChoklad": //Name Of thrown object.
                Debug.Log("Chokladskada");
                //Do damage
                break;
            case "2": //Name Of thrown object.

                //Do damage
                break;

        }
        hit = false;
    }
    void Attacking() //Checking Basic or special attack:
    {
        if (chargingAttack)
        {
            Debug.Log("*Swoosh* Special attack");
            //TODO: Add check for weapon and them assign correct animation.
            LaunchAttack(attackHitBoxes[1]);
            chargingAttack = false;
            //stamina drain
        }
        else
        {
            Debug.Log("*Swoosh* Basic attack");
            //TODO: Add check for weapon and them assign correct animation.
            LaunchAttack(attackHitBoxes[0]);
            //stamina drain
        }
        chargeTimer = 0;
    }
    void DetectedThrow() //Failsafe throw:
    {
        if (attackHitBoxes[3] != null)
        {
            LaunchAttack(attackHitBoxes[3]);
        }
    }
    IEnumerator ThrowCD (float CDTime) //Coroutine for throw:
    {
        yield return new WaitForSeconds(CDTime);
        throwing = false;
    }
}
