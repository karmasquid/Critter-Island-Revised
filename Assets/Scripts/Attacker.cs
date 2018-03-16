using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour {

    [SerializeField]
    Collider[] attackHitBoxes;

    bool chargingAttack; 
    float chargeTimer;

    void Start ()
    {

	}

    void Update ()
    {
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
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4") && chargingAttack == true)
        {
            chargingAttack = false;
            chargeTimer = 0;
        }


    }

    private void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (c.transform.root == gameObject.transform) // Slå inte dig själv...
            { continue;}
            //c.name = Fienden eller objektet som blir träffat.

        }
    }
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
    void Attacking()
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









}
