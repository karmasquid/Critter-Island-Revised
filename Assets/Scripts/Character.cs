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
    float DashDistance = 3f;
    [SerializeField]
    float stamReCharge; //Stamina recharge per frame.
    [SerializeField]
    float dodgeCost;
    [SerializeField]
    LayerMask Ground;
    [SerializeField]
    Vector3 Drag;
    [SerializeField]
    float rotationSpeed;

    Animator anim;
    PlayerManager playermanager;

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
        set { this.speedMultiplier = value; }
    }
    //Controls and rotation.
    private bool running;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private Vector3 targetRotation;

    //Movement and stamina:
    private Vector3 curPos;
    private Vector3 pastPos;
    private bool moving;
    private float rawStamRe;

    //Hole dodge:
    private bool dodgeronies = false;
    private bool inHole = false;
    private GameObject pitHole;
    IEnumerator DelayGravity;
    Vector3 PosBeforeDodge;
    float overHole;
    float normalDash;

    void Start()
    {
        //Modular variables. Saves start values set in inspector:
        rawStamRe = stamReCharge;
        rawSpeed = Speed;
        normalDash = DashDistance;

        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);

        InvokeRepeating("LastPosition", 0f, 0.1f); //Invokes and checks last position of player.

        anim = GetComponent<Animator>();
        playermanager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    void Update()
    {
        mover(); //Handles Movement and Rotation.
        runner(); //Handles Running.
        dodger(); //Handles Dodge.
        RestricMove(); //Restricts Movement when attacking.
        playermanager.RechargeStamina(stamReCharge);
        
    }
    void RestricMove()
    {
        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown("joystick button 2"))
        {
            atckn = true;
            if (running)
            {
                Speed = Speed / (speedMultiplier * rawSpeed);
            }
            else if (!running)
            {
                Speed = Speed / rawSpeed;

            }
        }
        if (Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp("joystick button 2")) // Om slag attack.
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
            else if (!running)
            {
                Speed = Speed * rawSpeed;
                if (Speed > rawSpeed)
                {
                    Speed = rawSpeed;
                }
            }
        }
    }
    void LastPosition() // Check for movement.
    {
        curPos = this.transform.position;
        if (curPos == pastPos)
        {
            moving = false;
            
        }
        if (curPos != pastPos)
        {
            moving = true;
         
        }
        pastPos = curPos;
    }
    void runner()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 5"))
        {
            if (running)
            {
                Speed = Speed * speedMultiplier;
            }
            else
            {
                if (Speed > 1 && atckn == true)
                {
                    Speed = 1;
                }

                if (Speed >= rawSpeed)
                {
                    Speed = rawSpeed * speedMultiplier;
                }
                running = true;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 5"))
        {
            if (!playermanager.outOfstamina && running == true) //Om du inte står stilla, drain.
            {
                Speed = rawSpeed * speedMultiplier;
            }
            else if (running == true)
            {
                running = false;
                Speed = rawSpeed;
            }
            if (!moving) //And out of stamina
            {
                stamReCharge = rawStamRe;
            }
            else //Moving and running.
            {
                playermanager.LooseStamina(20 * Time.deltaTime); //Stamina drain.
                stamReCharge = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp("joystick button 5"))
        {
            Speed = Speed / speedMultiplier; running = false;
            if (Speed < 1)
            {
                Speed = 1;
            }
            if (Speed > rawSpeed || Speed > 1)
            {
                Speed = rawSpeed;
            }
            stamReCharge = rawStamRe;
        }
    }
    void dodger()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 4"))
        {
            playermanager.LooseStamina(dodgeCost); //TODO Change value 40 to variable whose value change depending on equipment.

            if (!playermanager.outOfstamina) //If there is stamina:
            {
                anim.SetTrigger("fDodge");
                PosBeforeDodge = this.curPos;
                if (inHole) //Inside hole coll.
                {
                    pitHole.GetComponent<Collider>().isTrigger = true;
                }
                //Dodge move on player:
                _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));


            }
        }
    }

    void mover()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        float nowMoving = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") / 2;
        anim.SetFloat("Speed", nowMoving);


        if (input.sqrMagnitude > 1f)
            input.Normalize();
        if (inputRaw.sqrMagnitude > 1f)
            inputRaw.Normalize();

        if (inputRaw != Vector3.zero)
            targetRotation = Quaternion.LookRotation(input).eulerAngles;

        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        _controller.Move(input * Time.deltaTime * Speed);


        _velocity.y += Gravity * Time.deltaTime;

        _velocity.x /= 1 + Drag.x * Time.deltaTime;
        _velocity.y /= 1 + Drag.y * Time.deltaTime;
        _velocity.z /= 1 + Drag.z * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole") //&& Right jumping shoes...
        {
            dodgeronies = true;
            Gravity = 0f;
            pitHole = other.gameObject;
            inHole = true;
            overHole = (other.bounds.size.x / DashDistance) + other.bounds.size.x; //TODO: ta Z om spelaren kommer från andra hållet.
            if (dodgeronies)
            {
                DashDistance = overHole;
                dodgeronies = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            inHole = false;
            pitHole.GetComponent<Collider>().isTrigger = false;
            DashDistance = normalDash;
            Gravity = -180f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole" && other.isTrigger == true)
        {
            DelayGravity = Down(DashDistance / 10);
            StartCoroutine(DelayGravity);
            DashDistance = normalDash;
            //Gravity = 0f;
            //Boomerang lul.
            //this.transform.Translate(new Vector3(this.transform.position.x * Time.deltaTime / DashDistance, 0, this.transform.position.z * Time.deltaTime / DashDistance));
        }
    }
    IEnumerator Down(float CDTime) //Coroutine for throw:
    {
        yield return new WaitForSeconds(CDTime);
        Gravity = -180f;

        if (curPos.y < -2)
        {
            this.transform.position = PosBeforeDodge;
            DashDistance = overHole;
        }


        inHole = true;


        yield return new WaitForSeconds(CDTime);
        Gravity = 0f;
    }
}
