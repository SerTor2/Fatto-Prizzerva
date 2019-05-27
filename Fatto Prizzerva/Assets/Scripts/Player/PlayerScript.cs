using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum State { MOVING, PUNCHING, RUNING, KNOCKBACK, HABILITY };
    public State currentState = State.MOVING;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode runKey = KeyCode.LeftAlt;


    [SerializeField] private float normalSpeed = 7;
    private float speed;
    private float maxStamina = 100;
    private float currentStamina;
    private float costStaminaPerPunch = 2;
    private float currentTimePunch = 0;
    private float coolDownPunch = 0.75f;
    private float currentTimeState = 0;
    private Vector3 toMove = Vector3.zero;
    private Vector3 lastDirection = Vector3.zero;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Color startColor;
    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
                gameObject.transform.position += lastDirection * Time.deltaTime * speed;

                break;
            case State.RUNING:
                break;
            case State.KNOCKBACK:
                break;
            case State.HABILITY:
                break;
        }

        if (currentTimePunch > 0 && currentState != State.PUNCHING)
        {
            currentTimePunch += Time.deltaTime;
            if (currentTimePunch >= coolDownPunch)
                currentTimePunch = 0;
        }
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
                break;
            case State.RUNING:
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
                currentTimeState = 0;
                speed = normalSpeed / 2;
                currentTimePunch += Time.deltaTime;
                break;
            case State.RUNING:
                currentTimeState = 0;
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
            lastDirection = toMove;

        gameObject.transform.position += toMove * Time.deltaTime * speed;
    }

    private void CheckVariablesUntilMoving()
    {
        if (Input.GetMouseButton(0) && CanPunch())
            ChangeState(State.PUNCHING);
        else if (Input.GetKey(runKey))
            print("hacer que corra");
    }

    private bool CanPunch()
    {
        return currentTimePunch == 0 && currentStamina >= costStaminaPerPunch;
    }
}
