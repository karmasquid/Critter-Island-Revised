using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwable : MonoBehaviour {
    public int damageNum;

    void Start()
    {
        switch (gameObject.name)
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
            PlayerManager.instance.RangeAttack(collision.gameObject, damageNum);
            Attacker.hit = true;
            Destroy(gameObject);
        }
    }
}
