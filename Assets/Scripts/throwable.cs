using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour {

    int damageNum;
    Attacker attacker;
    PlayerManager playermanager;

    void Start()
    {
        attacker = GameObject.Find("Player").GetComponent<Attacker>();
        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        switch (gameObject.name) //Deals diffrent damage depending on what object that was thrown.
        {
            case "IcaBasicChoklad" + "(Clone)": //Name Of thrown object. Make work for clone...
                damageNum = 2;
                //Do damage
                break;
            case "2" + "(Clone)": //Name Of thrown object.
                damageNum = 3;
                //Do damage
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playermanager.RangeAttack(collision.gameObject, damageNum);
            attacker.hit = true;
            Destroy(gameObject);
        }
    }
}
