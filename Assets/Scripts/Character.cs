using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    float Speed = 5f;
    float rawSpeed;
    bool atckn = false;
    [SerializeField]
    float speedMultiplier = 1.5f;
    [SerializeField]
    float Gravity = -9.81f;
    [SerializeField]
    float GroundDistance = 0.2f;
    [SerializeField]
    float DashDistance = 5f;
    [SerializeField]
    LayerMask Ground;
    [SerializeField]
    Vector3 Drag;
    [SerializeField]
    float rotationSpeed;

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
        set { this.speedMultiplier = value; }
    }

    private bool running;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private Vector3 targetRotation;


    void Start()
    {
        rawSpeed = Speed;
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        //____________________________ROTATION_SCRIPT________________________________________________________________
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude > 1f)
            input.Normalize();
        if (inputRaw.sqrMagnitude > 1f)
            inputRaw.Normalize();

        if (inputRaw != Vector3.zero)
            targetRotation = Quaternion.LookRotation(input).eulerAngles;

        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);
        //_____________________________________________________________________________________________________________________________________________
        if (Input.GetKeyDown(KeyCode.LeftShift)  || Input.GetKeyDown("joystick button 5") && running == false)
        {
            Speed = Speed * speedMultiplier; running = true;
            if (Speed > 1 && atckn == true)
            {
                Speed = 1;
            }
            if (Speed<rawSpeed * speedMultiplier)
            {
                Speed = rawSpeed * speedMultiplier;
            }
            //Drain stamina.
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp("joystick button 5") && running == true)
        {
            Speed = Speed / speedMultiplier; running = false;
            if (Speed < 1)
            {
                Speed = 1;
            }
            if (Speed > rawSpeed)
            {
                Speed = rawSpeed;
            }
        }
        //_____________________________________________________RUNNING______________________________________________________
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(input * Time.deltaTime * Speed);


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4")) //Check stamina here.
        {
            _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));

            //Look for dashboots
            //then dashdistance ^Up

            
        }


        _velocity.y += Gravity * Time.deltaTime;

        _velocity.x /= 1 + Drag.x * Time.deltaTime;
        _velocity.y /= 1 + Drag.y * Time.deltaTime;
        _velocity.z /= 1 + Drag.z * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

        RestricMove();
    }
    void RestricMove() //TODO Justera, mycket hårdkodning:
    {
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown("joystick button 2"))
        {
            atckn = true;
            if (running)
            {
                Speed = Speed / (speedMultiplier* rawSpeed);
            }
            else
            {
                Speed = Speed / rawSpeed;

            }
        }
        if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp("joystick button 2")) // Om slag attack eller kast attack.
        {
            atckn = false;
            if (running)
            {
                Speed = Speed * speedMultiplier * rawSpeed;
                if (Speed < 1)
                {
                    Speed = 1;
                }
            }
            else 
            {
                Speed = Speed * rawSpeed;
                if (Speed > rawSpeed)
                {
                    Speed = rawSpeed;
                }
            }
        }
    }

}
