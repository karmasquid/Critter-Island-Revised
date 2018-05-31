using UnityEngine;

public class cameraFollow : MonoBehaviour {
    private Transform playerChar;

    public Vector3 offset;
    public float smoothspeed = 0.125f;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        playerChar = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredpos = playerChar.position + offset;
        Vector3 smoothpos = Vector3.SmoothDamp(transform.position, desiredpos,  ref velocity, smoothspeed);
        transform.position = smoothpos;
    }

}
