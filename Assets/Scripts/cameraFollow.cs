using UnityEngine;

public class cameraFollow : MonoBehaviour {
    public Transform playerChar;

    public Vector3 offset;
    public float smoothspeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
    void LateUpdate()
    {
        Vector3 desiredpos = playerChar.position + offset;
        Vector3 smoothpos = Vector3.SmoothDamp(transform.position, desiredpos,  ref velocity, smoothspeed);
        transform.position = smoothpos;
    }

}
