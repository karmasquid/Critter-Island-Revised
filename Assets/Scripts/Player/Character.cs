﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField]
    float Speed = 4f;
    [SerializeField]
    float speedMultiplier = 1.5f;
    [SerializeField]
    float GroundDistance = 0.2f;
    [SerializeField]
    float DashDistance = 4f;
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

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
        set { this.speedMultiplier = value; }
    }

    public bool Attacking
    {
        set { atckn = value; }
    }

    public bool IsDead
    {
        set
        {
            isDead = value;
        }
    }

    //Controls and rotation.
    private bool running;
    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero; 
    private bool _isGrounded = true;
    private Vector3 targetRotation;
    private Transform _groundChecker;

    //Movement and stamina:
    private Vector3 curPos;
    private Vector3 pastPos;
    private bool moving;
    private float rawStamRe;
    private float rawSpeed;

    //Restraints:
    private bool dodging;
    bool atckn = false;
    private bool lockMove = false;
    bool isDead;

    //Hole dodge:
    private bool inHole = false;
    private GameObject pitHole;
    private IEnumerator DelayGravity;
    private bool getOverIt = false;
    private GameObject target;

    public static Character instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Modular variables. Saves start values set in inspector:
        rawStamRe = stamReCharge;
        rawSpeed = Speed;

        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0); //Both

        InvokeRepeating("LastPosition", 0f, 0.1f); //Invokes and checks last position of player.

        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (!isDead)
        {
            if (!lockMove)
            {
                mover(); //Handles Movement and Rotation.
            }
            Runner(); //Handles Running.
            Dodger(); //Handles Dodge.
            RestricMove(); //Restricts Movement when attacking.
            Jump();
            PlayerManager.instance.StaminaRecharge = stamReCharge;

        }


    }
    void RestricMove()
    {
        if (InputManager.AttackDown())
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
        if (InputManager.AttackUp()) // Om slag attack.
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
    void Runner()
    {
        if (InputManager.Run())
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
        if (InputManager.Running())
        {
            if (PlayerManager.instance.Stamina.CurrentValue > 0 && running == true) //Om du inte står stilla, drain.
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
                PlayerManager.instance.LooseStamina(20 * Time.deltaTime); //Stamina drain.
                stamReCharge = 0f;
            }
        }

        if (InputManager.Ran())
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
    void Jump()
    {
        if (getOverIt)
        {

            this.GetComponent<NavMeshAgent>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * speedMultiplier * 2 * Time.deltaTime);
            this.transform.forward = -target.transform.forward;
        }
        else
        {
            this.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    void Dodger()
    {
        if (InputManager.Dodge())
        { 

            if (!dodging && PlayerManager.instance.Stamina.CurrentValue >= dodgeCost ) //If there is stamina:
            {
                dodging = true;
                if (inHole && pitHole.tag == "Hole") //Inside hole coll. && Jumping Shoes...
                {
                    target = pitHole.GetComponent<GoOver>().target;
                    if (this.transform.forward != pitHole.GetComponent<GoOver>().target.transform.forward)
                    {
                        getOverIt = true;
                        lockMove = true;
                        PlayerManager.instance.LooseStamina(dodgeCost);
                    }
                }
                else
                {
                    dodging = true;
                    anim.SetTrigger("fDodge");
                    PlayerManager.instance.LooseStamina(dodgeCost);

                    //Normal Dodge move on player:
                    Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
                    _body.AddForce(dashVelocity, ForceMode.VelocityChange);
                }

                //Recover:
                DelayGravity = DodgeDown(DashDistance / (DashDistance * 2));
                StartCoroutine(DelayGravity);
                
                
            }
        }
    }

    void mover()
    {
        Vector3 input = new Vector3(InputManager.Horizontal(), 0.0f, InputManager.Vertical()); //InputManager.VerticalAxis
        Vector3 inputRaw = new Vector3(InputManager.RawHorizontal(), 0.0f, InputManager.RawVertical());

        float nowMoving = InputManager.Horizontal() + InputManager.Vertical() / 2;
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
        _inputs.x = InputManager.Horizontal();
        _inputs.z = InputManager.Vertical();
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }
    void FixedUpdate()
    {
        if (!PlayerManager.instance.dead)
        {
            _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);

            if (!getOverIt)
            {
                _body.AddForce(Physics.gravity, ForceMode.Acceleration);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole" && Inventory.instance.equippedItems[2].Name == "Feather Stride Boots") //&& Right jumping shoes...
        {
            
            pitHole = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole") //&& Right jumping shoes...
        {
            inHole = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            inHole = false;
        }
            if (other.tag == "NotGround")
        {
            getOverIt = false;
            target = null;
        }
    }
    IEnumerator DodgeDown(float delay)
    {
        yield return new WaitForSeconds(delay);
        lockMove = false;
        dodging = false;

        if (getOverIt)
        {
            getOverIt = false;
        }
    }
}
