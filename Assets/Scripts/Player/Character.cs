using System.Collections;
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
    private float movingOffset = 0.25f;
    Vector3[] previousLocations = new Vector3[3]; //Vector array size = 3.
    private float rawStamRe;
    private float rawSpeed;

    //Restraints:
    public bool dodging;
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
            Speed = 1;
        }
        if (InputManager.AttackUp()) // Om slag attack.
        {
            atckn = false;
            if (running)
            {
                Speed = rawSpeed * speedMultiplier; 
            }
            else
            {
                Speed = rawSpeed;
            }
        }
    }
    void LastPosition() // Check for movement.
    {
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = this.transform.position;

        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= movingOffset)
            {
                //The minimum movement has been detected between frames
                moving = true;
                break;
            }
            else
            {
                moving = false;
            }
        }
    }
    void Runner()
    {
        if (InputManager.Run() && !atckn)
        {
            Speed = rawSpeed * speedMultiplier;
            running = true;
        }
        if (InputManager.Running())
        {
            if (PlayerManager.instance.Stamina.CurrentValue <= 0) //Om du inte står stilla, drain.
            {
                if (!atckn)
                {
                    Speed = rawSpeed;
                }
                else
                {
                    Speed = 1;
                }

                running = false;
            }

            if (moving || InputManager.MoveMe() && !atckn) //Moving and running.
            {
                PlayerManager.instance.LooseStamina(20 * Time.deltaTime); //Stamina drain.
                stamReCharge = 0f;
            }
            else
            {
                stamReCharge = rawStamRe;
            }
        }

        if (InputManager.Ran())
        {
            if (!atckn)
            {
                Speed = rawSpeed;
            }
            else
            {
                Speed = 1;
            }

            stamReCharge = rawStamRe;
            running = false;
        }
    }
    void Jump()
    {
        if (getOverIt)
        {

            this.GetComponent<NavMeshAgent>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * speedMultiplier * 2 * Time.deltaTime);
            this.transform.forward = -target.transform.forward;
            //Animation for jump over bridge...
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

            if (!dodging && PlayerManager.instance.Stamina.CurrentValue >= dodgeCost) //If there is stamina:
            {
                dodging = true;

                if (inHole && pitHole.tag == "Hole" && Inventory.instance.equippedItems[2] != null) //Inside hole coll. && Jumping Shoes...
                {
                    if (Inventory.instance.equippedItems[2].Name == "Feather Stride Boots")
                    {
                        target = pitHole.GetComponent<GoOver>().target;
                        if (this.transform.forward != pitHole.GetComponent<GoOver>().target.transform.forward)
                        {
                            getOverIt = true;
                            lockMove = true;
                            PlayerManager.instance.LooseStamina(dodgeCost);
                        }
                    }
                }
                else
                {
                    if (InputManager.MoveMe() == true)
                    {
                        //Forward dodge

                        //Animation
                        anim.SetTrigger("fDodge");

                        //Dodge calculation:
                        Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
                        _body.AddForce(dashVelocity, ForceMode.VelocityChange);
                    }
                    else
                    {
                        //Backward dodge

                        //Animation
                        anim.SetTrigger("bDodge");

                        //Dodge calculation:
                        Vector3 dashVelocity = Vector3.Scale(-transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
                        _body.AddForce(dashVelocity, ForceMode.VelocityChange);
                    }
                    //Stamina loss, same regardless of direction.
                    PlayerManager.instance.LooseStamina(dodgeCost);


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

        if (InputManager.MoveMe() == false) //Makes Run <--> Idle transition smooth.
        {
            anim.SetBool("isStopping", true);
        }
        else
        {
            anim.SetBool("isStopping", false);
        }

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
        if (other.tag == "Hole") //&& Right jumping shoes...
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
        _body.velocity = Vector3.zero;

        if (getOverIt)
        {
            getOverIt = false;
        }
    }
}
