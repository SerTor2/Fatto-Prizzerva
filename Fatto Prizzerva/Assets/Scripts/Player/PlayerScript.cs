using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum State { MOVING, PUNCHING, RUNING, PUNCHRUNNING, KNOCKBACK, FLYINGKICK, HABILITY, INSIDEPLANT,
        JUMPING, PLANNING };
    public State currentState = State.MOVING;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] public IEstaminable stamina;

    #region STATES
    public BaseState punch;
    public BaseState moving;
    public BaseState run;
    public BaseState punchRunning;
    public BaseState punchFly;
    public BaseState knockBack;
    public BaseState jumping;
    public BaseState planning;

    private PunchScript punchScript;
    private MoveScript moveScript;
    private RunScript runScript;
    private PunchRunning punchRunningScript;
    private PunchFly punchFlyScript;
    #endregion
    public float normalSpeed = 7;
    public float speed;
    public float recoverStamina = 0;
    public float damageBase = 2;
    public float currentTimePunch = 0;
    private float coolDownPunch = 0.75f;
    public float currentTimeState = 0;
    public Vector3 toMove = Vector3.zero;
    private Vector3 lastDirection = Vector3.up;
    private bool running = false;
    private bool onGround = false;
    private float gravity = 30;
    private float verticalSpeed = 0;
    private float distanceWithCamera;
    private int layer = 0;

    private float speedKnockBack = 0;
    private float timeKnockBack = 0;

    private float forceJump = 0;
    private PlantaTierra plantaTierra;
    private bool planningPlant = false;

    private Vector3 directionKnockBack;



    private CharacterController characterController;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private Color startColor;
    public GameObject children;
    public StateMachine stateMachine;
    // Start is called before the first frame update
    void Awake()
    {
        stamina = GetComponent<IEstaminable>();
        characterController = GetComponent<CharacterController>();
        startColor = spriteRenderer.color;
        speed = normalSpeed;

        punch = children.GetComponent<PunchScript>();
        punchFly = children.GetComponent<PunchFly>();
       
        punchScript = punch.GetComponent<PunchScript>();
        moveScript = moving.GetComponent<MoveScript>();
        runScript = run.GetComponent<RunScript>();
        punchRunningScript = punchRunning.GetComponent<PunchRunning>();
        punchFlyScript = punchFly.GetComponent<PunchFly>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckVariablesUntilMoving();
        stateMachine.ExecuteState();
        switch (currentState)
        {
            case State.MOVING:
                stamina.RegenStamina();
                break;
            case State.PUNCHING:
                break;
            case State.RUNING:
                break;
            case State.PUNCHRUNNING:
                break;
            case State.FLYINGKICK:
                break;
            case State.KNOCKBACK:
                break;
            case State.HABILITY:
                break;
        }
        CheckStats();
    }

    public void ChangeState(State newState)
    {

        switch (newState)
        {
            case State.MOVING:
                stateMachine.ChangeState(moving);
                break;
            case State.PUNCHING:
                stateMachine.ChangeState(punch);
                break;
            case State.RUNING:
                running = true;
                stateMachine.ChangeState(run);
                break;
            case State.PUNCHRUNNING:
                running = true;
                stateMachine.ChangeState(punchRunning);

                break;
            case State.KNOCKBACK:
                stateMachine.ChangeState(knockBack);
                break;
            case State.FLYINGKICK:
                stateMachine.ChangeState(punchFly);

                break;
            case State.HABILITY:
                break;

            case State.JUMPING:
                stateMachine.ChangeState(jumping);
                break;

            case State.PLANNING:
                break;
        }

        currentState = newState;
    }


    private void CheckVariablesUntilMoving()
    {
        if (Input.GetMouseButton(0) && CanPunch() && currentState == State.MOVING)
        {
            ChangeState(State.PUNCHING);
            return;
        }
        if (Input.GetKey(runKey) && currentState == State.MOVING && stamina.Stamina > 2)
        {
            ChangeState(State.RUNING);
            return;
        }
        else if (Input.GetKeyUp(runKey) && running)
            ChangeState(State.MOVING);

        if (Input.GetKeyDown(jumpKey) && currentState == State.MOVING)
            StartJump();

        if (Input.GetKey(runKey) && running && CanPunchRunning() && speed > normalSpeed + 1 && Input.GetMouseButton(0))
            ChangeState(State.PUNCHRUNNING);

        if (Input.GetMouseButton(1) && currentState.Equals(State.MOVING))
            OnActionButton();

    }

    private bool CanPunch()
    {
        return currentTimePunch == 0 && stamina.Stamina >= punchScript.costStaminaPerPunch && currentState == State.MOVING;
    }

    private bool CanPunchRunning()
    {
        return currentTimePunch == 0 && stamina.Stamina >= punchRunningScript.costStaminaPerPunch * (speed - normalSpeed) 
            && currentState == State.RUNING;
    }

    private void CheckStats()
    {
        if (currentTimePunch > 0 && currentState != State.PUNCHING && currentState != State.PUNCHRUNNING && 
            currentState != State.FLYINGKICK)
        {
            currentTimePunch += Time.deltaTime;
            if (currentTimePunch >= coolDownPunch)
                currentTimePunch = 0;
        }

        if (!running)
        {
            if (stamina.CurrentRegenStamina > 0)
            {
                stamina.RegenStamina();
            }

            if (speed > normalSpeed)
            {
                speed -= Time.deltaTime * runScript.decreseSpeedSecond;
                if (speed < normalSpeed)
                    speed = normalSpeed;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemie")
        {
            if (currentState == State.PUNCHING)
                hit.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((hit.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed);
            if (currentState == State.PUNCHRUNNING || currentState == State.FLYINGKICK)
                hit.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((hit.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed / 3F);
            ChangeState(State.MOVING);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemie")
        {
            if (currentState == State.PUNCHING)
                other.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((other.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed);
            if (currentState == State.PUNCHRUNNING || currentState == State.FLYINGKICK)
                other.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((other.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed / 3F);
            ChangeState(State.MOVING);
        }
    }

    private void OnActionButton()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out rayHit, 25, layerMask))
        {
            if ((rayHit.collider.transform.position - gameObject.transform.position).magnitude <= 3)
            {
                if (rayHit.collider.gameObject.GetComponent<PlantaCatapulta>() != null)
                {
                    ChangeState(State.INSIDEPLANT);
                    rayHit.collider.gameObject.GetComponent<PlantaCatapulta>().EnterInThePlant(this);
                }

            }

        }
    }

    public float ChangeSpeed(float _speed)
    {
        _speed = Mathf.Clamp(_speed, normalSpeed, runScript.maxSpeed);
        speed = _speed;
        return speed;
    }

    public void StartKnockBack(float _damage, float _time, Vector3 _direction)
    {
        speedKnockBack = _damage;
        timeKnockBack = _time;
        directionKnockBack = _direction;
        ChangeState(State.KNOCKBACK);
    }

    public void GetStatsKnockBack( out float _speed, out float _time, out Vector3 _direction)
    {
        _speed = speedKnockBack;
        _time = timeKnockBack;
        _direction = directionKnockBack;
    }

    public void ResetSpeed()
    {
        speed = normalSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetLayerPlayer(int _layer)
    {
        layer = _layer;
    }

    public int GetLayerPlayer()
    {
        return layer;
    }

    public void StartJump()
    {
        if(plantaTierra != null)
        {
            planningPlant = plantaTierra.planningPlant;
            if(planningPlant)
            {
                toMove = toMove * speed / normalSpeed;
            }
            else
            {
                toMove = plantaTierra.direction;
            }
            ChangeState(State.JUMPING);
        }
    }

    public float GetForceJump()
    {
        return forceJump;
    }

    public bool GetPlanningPlant()
    {
        return planningPlant;
    }

    public float GetGravity()
    {
        return gravity;
    }

    public void SetPlantaTierra(PlantaTierra _planta)
    {
        plantaTierra = _planta;
        if (plantaTierra != null)
        {
            forceJump = plantaTierra.forceJump;
        }
        else
            forceJump = 0;
    }
}
