using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    float Speed = 4f;
    float rawSpeed;
    bool atckn = false;
    [SerializeField]
    float speedMultiplier = 1.5f;
    [SerializeField]
    float GroundDistance = 0.2f;
    [SerializeField]
    float DashDistance = 10f;
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
    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero; 
    private bool _isGrounded = true;
    private Vector3 targetRotation;
    private Transform _groundChecker;

    //Movement and stamina:
    private bool dodging;
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

        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0); //Both



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

            if (!dodging && !playermanager.outOfstamina) //If there is stamina:
            {
                dodging = true;
                anim.SetTrigger("fDodge");
                PosBeforeDodge = this.curPos;
                if (inHole) //Inside hole coll.
                {
                    pitHole.GetComponent<Collider>().isTrigger = true;
                    _body.isKinematic = false;
                }
                //Dodge move on player:
                _body.drag = 3;
                Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
                _body.AddForce(dashVelocity, ForceMode.VelocityChange);

                DelayGravity = DodgeDown(DashDistance / (DashDistance * 2));
                StartCoroutine(DelayGravity);
                
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

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }
    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
        _body.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole") //&& Right jumping shoes...
        {
            dodgeronies = true;
            _body.useGravity = false;
            pitHole = other.gameObject;
            inHole = true;
            float holeSize;

            if (other.bounds.size.x < other.bounds.size.z) //Works alright for cube formed holes, not so much for other forms...
            {
                holeSize = other.bounds.size.z;
            }
            else
            {
                holeSize = other.bounds.size.x;
            }

            overHole = (holeSize / DashDistance) + holeSize * speedMultiplier;
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
            _body.useGravity = true;
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
    IEnumerator DodgeDown(float delay)
    {
        yield return new WaitForSeconds(delay);
        dodging = false;
        _body.drag = 0;
    }
    IEnumerator Down(float CDTime) //Coroutine for Dodge:
    {
        yield return new WaitForSeconds(CDTime);
        _body.useGravity = true;
        _body.isKinematic = false;
        if (curPos.y < -2)
        {
            _body.drag = 0;
            this.transform.position = PosBeforeDodge;
            DashDistance = overHole;
        }


        inHole = true;
    }
}
