using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
	// Use this for initialization
    Vector3 targetRotation;
    [SerializeField]
    int rotationSpeed;
    [SerializeField]
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw ("Vertical"));
        if (input.sqrMagnitude > 1f)
            input.Normalize();
        if (inputRaw.sqrMagnitude > 1f)
            inputRaw.Normalize();

        if (inputRaw != Vector3.zero)
            targetRotation = Quaternion.LookRotation(input).eulerAngles;

        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);
    }
}
