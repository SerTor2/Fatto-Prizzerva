using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum State { MOVING, PUNCHING, RUNING, PUNCHRUNNING, KNOCKBACK, FLYINGKICK, HABILITY };
    public State currentState = State.MOVING;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [SerializeField] private LayerMask layerMask;


    [SerializeField] private float normalSpeed = 7;
    [SerializeField] private CanvasPlayerScript canvasPlayer;
    private float speed;
    public float maxStamina = 100;
    private float currentStamina;
    public float costStaminaPerPunch = 2;
    public float costStaminaPerRunningSecond = 5;
    public float recoverStaminaRunning = 0;
    public float maxSpeed = 10f;
    public float incrementSpeedSecond = 2f;
    public float decreseSpeedSecond = 10f;
    public float multiplyStaminaPerSpeed = 1f;
    public float resistenceToGirRunning = 30;
    public float damageBase = 2;
    private float currentTimePunch = 0;
    private float coolDownPunch = 0.75f;
    private float currentTimeState = 0;
    private Vector3 toMove = Vector3.zero;
    private Vector3 lastDirection = Vector3.up;
    private bool running = false;
    private bool onGround = false;
    private float gravity = 30;
    private float verticalSpeed = 0;
    private GameObject myCamera;
    private float distanceWithCamera;
    private int layer = 0;
    private SphereCollider colliderPunch;

    private CharacterController characterController;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    private Color startColor;
    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        startColor = spriteRenderer.color;
        currentStamina = maxStamina;
        speed = normalSpeed;
        myCamera = Camera.main.gameObject;
        distanceWithCamera =  myCamera.transform.position.y - gameObject.transform.position.y;
        colliderPunch = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.MOVING:
                Move();
                CheckVariablesUntilMoving();
                break;
            case State.PUNCHING:
                Move();
                currentTimeState += Time.deltaTime;
                if (currentTimeState >= 0.3f)
                    ChangeState(State.MOVING);

                break;
            case State.RUNING:
                CheckVariablesUntilMoving();
                if (currentStamina <= 0)
                    ChangeState(State.MOVING);
                Move();
                break;
            case State.PUNCHRUNNING:
                currentTimeState += Time.deltaTime;
                PunchRunning();
                if (currentTimeState >= 0.15f)
                    ChangeState(State.MOVING);
                break;
            case State.FLYINGKICK:
                currentTimeState += Time.deltaTime;
                MoveFlyKick();
                if (currentTimeState >= 0.15f)
                    ChangeState(State.MOVING);
                break;
            case State.KNOCKBACK:
                break;
            case State.HABILITY:
                break;
        }
        CheckStats();
        CheckGravity();
    }

    public void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.MOVING:
                break;
            case State.PUNCHING:
                spriteRenderer.color = startColor;
                speed = normalSpeed;
                currentTimePunch += Time.deltaTime;
                colliderPunch.enabled = false;

                break;
            case State.RUNING:
                running = false;
                anim.SetBool("Running", running);
                break;
            case State.PUNCHRUNNING:
                running = false;
                spriteRenderer.color = startColor;
                currentTimePunch += Time.deltaTime;
                speed = normalSpeed;
                colliderPunch.enabled = false;

                break;
            case State.KNOCKBACK:
                break;
            case State.FLYINGKICK:
                spriteRenderer.color = startColor;
                speed = normalSpeed;
                colliderPunch.enabled = false;

                break;
            case State.HABILITY:
                break;
        }

        switch (newState)
        {
            case State.MOVING:
                currentTimeState = 0;
                break;
            case State.PUNCHING:
                anim.SetTrigger("Fist");
                currentStamina -= costStaminaPerPunch;
                canvasPlayer.ChangeStamina();
                currentTimeState = 0;
                speed = normalSpeed / 2;
                colliderPunch.enabled = true;
                break;
            case State.RUNING:
                running = true;
                anim.SetBool("Running", running);
                currentTimeState = 0;
                break;
            case State.PUNCHRUNNING:
                running = true;
                spriteRenderer.color = Color.red;
                currentStamina -= costStaminaPerPunch * (speed - normalSpeed);
                canvasPlayer.ChangeStamina();
                currentTimeState = 0;
                speed = normalSpeed * 2;
                colliderPunch.enabled = true;

                break;
            case State.KNOCKBACK:
                currentTimeState = 0;
                break;
            case State.FLYINGKICK:
                speed = normalSpeed * 3f;
                spriteRenderer.color = Color.red;
                currentTimeState = 0;
                colliderPunch.enabled = true;

                break;
            case State.HABILITY:
                currentTimeState = 0;
                break;
        }

        currentState = newState;
    }

    private void Move()
    {
        toMove = Vector3.zero;
        if (Input.GetKey(upKey))
            toMove += myCamera.transform.up;
        if (Input.GetKey(downKey))
            toMove -= myCamera.transform.up;
        if (Input.GetKey(rightKey))
            toMove += myCamera.transform.right;
        if (Input.GetKey(leftKey))
            toMove -= myCamera.transform.right;

        toMove = new Vector3(toMove.x, 0, toMove.z);
        toMove.Normalize();
        if (toMove.magnitude > 0)
        {
            if (running)
            {
                float magnitudVector = (lastDirection - toMove).magnitude;
                if(magnitudVector == 0)
                {
                    float value = Time.deltaTime * costStaminaPerRunningSecond * (speed - normalSpeed) * multiplyStaminaPerSpeed;
                    currentStamina -= value;
                    recoverStaminaRunning += value;
                    canvasPlayer.ChangeStamina();
                    if (speed < maxSpeed)
                    {
                        speed += Time.deltaTime * incrementSpeedSecond;
                        if (speed > maxSpeed)
                            speed = maxSpeed;
                    }
                }
                else if(magnitudVector > 0 && magnitudVector < 1)
                {
                    toMove = (toMove / 30 + lastDirection).normalized;
                    lastDirection = toMove;
                    float secondvalue = Time.deltaTime * costStaminaPerRunningSecond * (speed - normalSpeed) * multiplyStaminaPerSpeed;
                    currentStamina -= secondvalue;
                    recoverStaminaRunning += secondvalue;
                    canvasPlayer.ChangeStamina();
                    if (speed > normalSpeed)
                    {
                        speed -= Time.deltaTime * incrementSpeedSecond / 4;
                        if (speed > normalSpeed)
                            speed = normalSpeed;
                    }
                }
                else if(magnitudVector < 1.75f)
                {
                    toMove = (toMove / 30 + lastDirection).normalized;
                    lastDirection = toMove;
                    float secondvalue = Time.deltaTime * costStaminaPerRunningSecond * (speed - normalSpeed) * multiplyStaminaPerSpeed;
                    currentStamina -= secondvalue;
                    recoverStaminaRunning += secondvalue;
                    canvasPlayer.ChangeStamina();
                    if (speed > normalSpeed)
                    {
                        speed -= Time.deltaTime * incrementSpeedSecond / 2;
                        if (speed > normalSpeed)
                            speed = normalSpeed;
                    }
                }
                else
                {
                    toMove = lastDirection;
                    float otherValue = Time.deltaTime * costStaminaPerRunningSecond * (speed - normalSpeed) * multiplyStaminaPerSpeed;
                    currentStamina -= otherValue;
                    recoverStaminaRunning += otherValue;
                    canvasPlayer.ChangeStamina();
                    if (speed < maxSpeed)
                    {
                        speed += Time.deltaTime * incrementSpeedSecond;
                        if (speed > maxSpeed)
                            speed = maxSpeed;
                    }
                }

            }
            else lastDirection = toMove;
            gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, toMove);

        }
        else
        {
            if (running)
            {
                toMove = lastDirection;
                float otherValue = Time.deltaTime * costStaminaPerRunningSecond * (speed - normalSpeed) * multiplyStaminaPerSpeed;
                currentStamina -= otherValue;
                recoverStaminaRunning += otherValue;
                canvasPlayer.ChangeStamina();
                if (speed < maxSpeed)
                {
                    speed += Time.deltaTime * incrementSpeedSecond;
                    if (speed > maxSpeed)
                        speed = maxSpeed;
                }
            }
        }

        CollisionFlags collisionFlags = characterController.Move(toMove * Time.deltaTime * speed);
    }

    private void PunchRunning()
    {
        CollisionFlags collisionFlags = characterController.Move(toMove * Time.deltaTime * speed );
    }

    private void CheckVariablesUntilMoving()
    {
        if (Input.GetMouseButton(0) && CanPunch() && !running)
        {
            ChangeState(State.PUNCHING);
            return;
        }
        if (Input.GetKey(runKey) && !running && currentStamina > 2)
        {
            ChangeState(State.RUNING);
            return;
        }
        else if(Input.GetKeyUp(runKey))
            ChangeState(State.MOVING);
        if (Input.GetKey(runKey) && running && CanPunchRunning() && speed > normalSpeed + 1 && Input.GetMouseButton(0))
            ChangeState(State.PUNCHRUNNING);

        if (Input.GetMouseButton(1) && currentState.Equals(State.MOVING))
            OnActionButton();

    }

    private bool CanPunch()
    {
        return currentTimePunch == 0 && currentStamina >= costStaminaPerPunch;
    }

    private bool CanPunchRunning()
    {
        return currentStamina >= costStaminaPerPunch * (speed - normalSpeed);
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public float GetCurrentRecoverStamina()
    {
        return recoverStaminaRunning;
    }

    private void RecoverStamina()
    {
        float value = Time.deltaTime;
        currentStamina += value;
        recoverStaminaRunning -= value;

        if (recoverStaminaRunning < 0)
        {
            currentStamina += recoverStaminaRunning;
            recoverStaminaRunning = 0;
        }
        canvasPlayer.ChangeStamina();

    }

    private void CheckStats()
    {
        if (currentTimePunch > 0 && currentState != State.PUNCHING)
        {
            currentTimePunch += Time.deltaTime;
            if (currentTimePunch >= coolDownPunch)
                currentTimePunch = 0;
        }

        if (!running)
        {
            if (recoverStaminaRunning >= 0)
            {
                RecoverStamina();
            }

            if (speed > normalSpeed)
            {
                speed -= Time.deltaTime * decreseSpeedSecond;
                if (speed < normalSpeed)
                    speed = normalSpeed;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Enemie")
        {
            if (currentState == State.PUNCHING)
                hit.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((hit.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed);
            if (currentState == State.PUNCHRUNNING || currentState == State.FLYINGKICK)
                hit.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((hit.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed / 2);
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
                other.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((other.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speed / 2);
            ChangeState(State.MOVING);
        }
    }

    private void OnActionButton()
    {
        RaycastHit rayHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Camera.main.transform.position, ray.direction, out rayHit, 25, layerMask))
        {
            if((rayHit.collider.transform.position - gameObject.transform.position).magnitude <= 3)
            {
                if (rayHit.collider.gameObject.GetComponent<PlantaCatapulta>() != null)
                {
                    rayHit.collider.gameObject.GetComponent<PlantaCatapulta>().EnterInThePlant(this);
                }

            }
            
        }
    }

    public void StartFlyKick(Vector3 _direction)
    {
        lastDirection = _direction;
        gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, _direction);
        ChangeState(State.FLYINGKICK);
    }

    private void CheckGravity()
    {
        verticalSpeed -= gravity * Time.deltaTime;
        CollisionFlags collisionFlags = characterController.Move(new Vector3(0,verticalSpeed, 0) * Time.deltaTime);
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            onGround = false;
        }
        else
            onGround = true;
        if (onGround)
            verticalSpeed = 0;

    }

    public void MoveCameraUpLayer()
    {
        myCamera.gameObject.transform.position = new Vector3(myCamera.transform.position.x, gameObject.transform.position.y + distanceWithCamera, myCamera.transform.position.z);

    }

    private void MoveFlyKick()
    {
        characterController.Move(lastDirection * Time.deltaTime * speed);
    }

    public void SetLayerPlayer(int _layer)
    {
        layer = _layer;
    }

    public int GetLayerPlayer()
    {
        return layer;
    }
}
