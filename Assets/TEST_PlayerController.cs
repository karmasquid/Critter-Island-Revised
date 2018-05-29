using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_PlayerController : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 5f;

    //För att bestämma kamerans frammåt och höger. Dessa använder vi för att förflytta spelaren
    //relativt till kameran.
    Vector3 forward, right;

	void Start ()
    {
        //Hämtar kamerans frammåt som vi sedan använder för att räkna ut höger/vertikal riktning.
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
	}
	
	void Update ()
    {
        if (Input.anyKey)
        {
            Move();
        }
	}

    void Move()
    {
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
        
    }
}
