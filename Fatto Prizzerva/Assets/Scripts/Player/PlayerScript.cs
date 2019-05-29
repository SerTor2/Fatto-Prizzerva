using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum State { MOVING, PUNCHING, RUNING, PUNCHRUNNING, KNOCKBACK, HABILITY };
    public State currentState = State.MOVING;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;


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

    private CharacterController characterController;
    private SpriteRenderer spriteRenderer;
    private Color startColor;
    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        currentStamina = maxStamina;
        speed = normalSpeed;
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
            case State.KNOCKBACK:
                break;
            case State.HABILITY:
                break;
        }
        CheckStats();

    }

    private void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.MOVING:
                break;
            case State.PUNCHING:
                spriteRenderer.color = startColor;
                speed = normalSpeed;
                currentTimePunch += Time.deltaTime;
                break;
            case State.RUNING:
                running = false;
                break;
            case State.PUNCHRUNNING:
                running = false;
                spriteRenderer.color = startColor;
                currentTimePunch += Time.deltaTime;
                speed = normalSpeed;
                break;
            case State.KNOCKBACK:
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
                spriteRenderer.color = Color.red;
                currentStamina -= costStaminaPerPunch;
                canvasPlayer.ChangeStamina();
                currentTimeState = 0;
                speed = normalSpeed / 2;
                break;
            case State.RUNING:
                running = true;
                currentTimeState = 0;
                break;
            case State.PUNCHRUNNING:
                running = true;
                spriteRenderer.color = Color.red;
                currentStamina -= costStaminaPerPunch * (speed - normalSpeed);
                canvasPlayer.ChangeStamina();
                currentTimeState = 0;
                speed = speed * 2;
                break;
            case State.KNOCKBACK:
                currentTimeState = 0;
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
            toMove += Vector3.up;
        if (Input.GetKey(downKey))
            toMove += Vector3.down;
        if (Input.GetKey(rightKey))
            toMove += Vector3.right;
        if (Input.GetKey(leftKey))
            toMove += Vector3.left;

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
        if(currentState == State.PUNCHING)
            print(damageBase * speed);
        if(currentState == State.PUNCHRUNNING)
            print(damageBase * speed / 2);

    }
}
