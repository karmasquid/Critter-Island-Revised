﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    public bool hit = false;

    [SerializeField]
    float atkSpeed;
    [SerializeField]
    float reloadSpeed;
    [SerializeField]
    Collider[] attackHitBoxes;
    [SerializeField]
    GameObject[] projectiles; //Set size once we know amount of consumables.

    GameObject names;
    GameObject Currentequipped;

    bool chargingAttack;
    bool throwing = false;
    
    float chargeTimer;
    string CurrentName;
    bool canAtk = true;

    PlayerManager playermanager;
    IEnumerator Reload;

    Animator anim;

    GameObject[] enemies;
    List<GameObject> listOfEffect = new List<GameObject>();

    void Start ()
    {
        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        anim = GetComponent<Animator>();
        Currentequipped = projectiles[0];
        
    }
    void AtkControl()
    {
        if (canAtk)
        {
            if (Input.GetKey(KeyCode.H) || Input.GetKey("joystick button 2"))
            {

                chargeTimer += Time.deltaTime;
                if (chargeTimer > 1.0f)
                {
                    chargingAttack = true;
                    anim.SetBool("isCharging", true);
                }
            }
            if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp("joystick button 2"))
            {
                CheckWeapon();
                Attacking();
                canAtk = false;
            }
        }
    }
    void Update ()
    {
        //-------------------------------------Main Attacks & Controls------------------------------------------------
        AtkControl();

        if (playermanager.hasAmmo && Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp("joystick button 1")) 
        {
            if (!throwing && playermanager.hasAmmo)
            {
                anim.SetTrigger("throw");
                GameObject preo = Instantiate(Currentequipped) as GameObject; //Change index deppending on item equipped.
                preo.transform.position = transform.position + attackHitBoxes[2].transform.up;
                Rigidbody rb = preo.GetComponent<Rigidbody>();
                rb.velocity = attackHitBoxes[2].transform.forward * 20;
                throwing = true;

                playermanager.AmmoCounter(1); //-1 ammo
                Reload = ThrowCD(reloadSpeed);
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
            anim.SetBool("isCharging", false);
        }


        //----------------------------------------Invokes for throw------------------------------------------------------
        if (throwing)
        {
            InvokeRepeating("DetectedThrow", 0f, 4f);
        }

        if (hit)
        {
            Invoke("Throwdamage", 0f); //Metod for hits.
        }

    }

    void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach (Collider c in cols)
        {
            if (c.transform.root == gameObject.transform) // Slå inte dig själv...
            {
                continue;
            }
            listOfEffect.Add(c.gameObject);
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
        hit = false;
    }
    void Attacking() //Checking Basic or special attack:
    {
        playermanager.LooseStamina(0);
        if (!playermanager.outOfstamina)
        {
            //---------Check if charging up an attack or not------------
            if (chargingAttack)
            {
                anim.SetTrigger("chargeAttack");
                //TODO: Add check for weapon and them assign correct animation.
                LaunchAttack(attackHitBoxes[1]);
                chargingAttack = false;
                anim.SetBool("isCharging", false);
                playermanager.LooseStamina(20); //Stamina drain

                if (!listOfEffect.Count.Equals(0)) //Om listan inte tom...
                {
                    enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
                }
                if (enemies != null)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                        enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                        enemy.GetComponent<BasicAI>().TakeDMG(playermanager.MeleeSpecDamage); //Takes basic melee damage + potetial special attack damage.

                        //enemy.GetComponent<ElderBrute>().TakeDMG(meleeDamage);
                    }
                    //playermanager.MeleeAttack(enemies); //Added special attack damage.
                }
            }
            else
            {
                anim.SetTrigger("attack");
                //TODO: Add check for weapon and them assign correct animation.
                LaunchAttack(attackHitBoxes[0]);
                playermanager.LooseStamina(2); //Stamina drain

                if (!listOfEffect.Count.Equals(0)) //Om listan inte tom...
                {
                    enemies = listOfEffect.ToArray(); //Convert the list of gameobject enemies inside the collider into an array.
                }
                if (enemies != null)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();

                        enemyRB.AddForce(-enemy.transform.forward * 2f * 4f, ForceMode.Impulse);

                        enemy.GetComponent<BasicAI>().TakeDMG(playermanager.MeleeDamage); //Takes basic melee damage + potetial special attack damage.

                        //enemy.GetComponent<ElderBrute>().TakeDMG(meleeDamage);
                    }
                }

                chargingAttack = false;
                anim.SetBool("isCharging", false);
            }
            listOfEffect.Clear();
            if (listOfEffect.Count.Equals(0))
            {
                enemies = null;
            }
            chargeTimer = 0;

            Reload = AttackCD(atkSpeed);
            StartCoroutine(Reload);
        }
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
    IEnumerator AttackCD(float AtkCDTime)
    {
        yield return new WaitForSeconds(AtkCDTime);
        canAtk = true;
    }
}
