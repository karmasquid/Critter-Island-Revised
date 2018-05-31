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
    Vector3 forward, right;

    public float SpeedMultiplier //Variable used to calculate by how much speed is increased when running.
    {
        get { return this.speedMultiplier; }
        set { this.speedMultiplier = value; }
    }

    public bool Attacking //Bool variable used to check if player is attacking.
    {
        set { atckn = value; }
    }

    public bool IsDead //Bool variable used to check if player is dead.
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
    private Vector3 heading;
    private Vector3 targetRotation;

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

        //Variables used to allow the player to move in relation to the main camera.
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        //Invokes and checks last position of player.
        InvokeRepeating("LastPosition", 0f, 0.1f); 

        //Set the components...
        _body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead) //Checking if player is not dead.
        {
            if (!lockMove) //Check if player have controll, used on for jump over bridge.
            {
                mover(); //Handles Movement and Rotation.
            }
            Runner(); //Handles Running.
            Dodger(); //Handles Dodge.
            RestricMove(); //Restricts Movement when attacking.
            Jump(); //Jump over bridge metod.

            PlayerManager.instance.StaminaRecharge = stamReCharge;

        }


    }
    void RestricMove()
    {
        if (InputManager.AttackDown()) //On starting to attack.
        {
            atckn = true;
            Speed = 1;
        }
        if (InputManager.AttackUp()) //On release attack.
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
    void LastPosition() // Check if the player is moving.
    {
        for (int i = 0; i < previousLocations.Length - 1; i++) //Checking the last 3 positions of the player.
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = this.transform.position; //Setting the next last position.

        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= movingOffset)
            {
                //Movement is detected.
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
        if (InputManager.Run() && !atckn) //Checking if the player is running and not attacking.
        {
            Speed = rawSpeed * speedMultiplier; //Gives running speed.
            running = true;
        }
        if (InputManager.Running())
        {
            if (PlayerManager.instance.Stamina.CurrentValue <= 0) //Checking if the player doesn't have enough stamina to continue running.
            {
                if (!atckn)
                {
                    Speed = rawSpeed;
                }
                else // If we are attacking then speed is 1 regardless if running.
                {
                    Speed = 1;
                }

                running = false;
            }

            if (moving || InputManager.MoveMe() && !atckn) //Moving and running.
            {
                PlayerManager.instance.LooseStamina(20 * Time.deltaTime); //Stamina drain.
                stamReCharge = 0f; //Not regaining any stamina.
            }
            else
            {
                stamReCharge = rawStamRe; //Allows player to regain stamina again if they are not trying to use stamina. They won't lose stamina if the player is standing still.
            }
        }

        if (InputManager.Ran()) // Going from running to walking.
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
        if (getOverIt) //If trying to dodge over the bridge.
        {
            //Breifly disables the navmeshagent and moves the player to the other side of the gap.
            this.GetComponent<NavMeshAgent>().enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * speedMultiplier * 2 * Time.deltaTime);
            this.transform.forward = -target.transform.forward;
            //Add animation for jump over bridge here.
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

            if (!dodging && PlayerManager.instance.Stamina.CurrentValue >= dodgeCost) //Checking if the player has stamina and not currently dodging.
            {
                dodging = true;

                if (inHole && pitHole.tag == "Hole" && Inventory.instance.equippedItems[2] != null) //Inside hole coll. && Jumping Shoes...
                {
                    if (Inventory.instance.equippedItems[2].Name == "Feather Stride Boots") //Checking if the player has the correct boots to jump.
                    {
                        target = pitHole.GetComponent<GoOver>().target; //Sets the target gameobject collected from the GoOver script.
                        if (this.transform.forward != pitHole.GetComponent<GoOver>().target.transform.forward)
                        {
                            getOverIt = true;
                            lockMove = true;
                            PlayerManager.instance.LooseStamina(dodgeCost); //Applies dodge cost.
                        }
                    }
                }
                else
                {
                    if (InputManager.MoveMe() == true) //Checking if the player is moving/trying to move.
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
                    PlayerManager.instance.LooseStamina(dodgeCost); //Losing stamina regardless of dodge direction.


                }

                    //Recover the player from the dodge. Stopping it from sliding away.
                    DelayGravity = DodgeDown(DashDistance / (DashDistance * 2));
                    StartCoroutine(DelayGravity);


            }
        }
    }
    

    void mover()
    {
        //Rotation of player, takes input into account for rotation after movement of player.
        Vector3 input = new Vector3(InputManager.Horizontal(), 0.0f, InputManager.Vertical()); 
        Vector3 inputRaw = new Vector3(InputManager.RawHorizontal(), 0.0f, InputManager.RawVertical());

        Vector3 rightMovement = right * Speed * Time.deltaTime * InputManager.Horizontal();
        Vector3 upMovement = forward * Speed * Time.deltaTime * InputManager.Vertical();

        heading = Vector3.Normalize(rightMovement + upMovement);

        //Normalizing the rotation.
        if (heading.sqrMagnitude > 1f) 
            input.Normalize();
        if (inputRaw.sqrMagnitude > 1f)
            inputRaw.Normalize();

        if (inputRaw != Vector3.zero)
            targetRotation = Quaternion.LookRotation(heading).eulerAngles;


        //Makes rotation of player so that he/she may only move in 8 diffrent directions.
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z), Time.deltaTime * rotationSpeed);


        //Animations and transitions while moving.
        float nowMoving = InputManager.Horizontal() + InputManager.Vertical();
        if (nowMoving == 0 && moving)
        {
            nowMoving = 0.1f;
        }
        anim.SetFloat("Speed", nowMoving);

        //See if player is trying to move by checking input of player.
        if (InputManager.MoveMe() == false) 
        {
            anim.SetBool("isStopping", true);
            _body.velocity = Vector3.zero;
        }
        else
        {
            anim.SetBool("isStopping", false);
        }

        //Sets the players forward.
        _inputs = Vector3.zero;
        _inputs.x = InputManager.Horizontal();
        _inputs.z = InputManager.Vertical();
        if (_inputs != Vector3.zero)
            transform.forward = heading;
    }
    void FixedUpdate()
    {
        if (!PlayerManager.instance.dead) //Check if not dead.
        {
            _body.MovePosition(_body.position + heading * Speed * Time.fixedDeltaTime);

            if (!getOverIt)
            {
                _body.AddForce(Physics.gravity, ForceMode.Acceleration); //Gravitation in the world, not applied when jumping over the bridge.
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole") //Takes and stores in the colliding gameobject, used for getting over the bridge. 
        {
                pitHole = other.gameObject;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole") //Sets the player as ready to jump over the hole.
        {
            inHole = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole") //Not ready to jump over hole.
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
        //Resets variables after delay time.
        yield return new WaitForSeconds(delay);
        lockMove = false;
        dodging = false;
        _body.velocity = Vector3.zero;

        if (getOverIt)
        {
            getOverIt = false;
        }
    }
} //Mattias Eriksson
