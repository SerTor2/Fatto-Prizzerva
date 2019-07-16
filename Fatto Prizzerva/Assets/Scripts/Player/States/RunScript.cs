using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunScript : BaseState
{
    public float speed = 0;
    public float costStaminaPerRunningSecond = 5;
    public float maxSpeed = 10f;
    public float incrementSpeedSecond = 2f;
    public float decreseSpeedSecond = 10f;
    public float resistenceToGirRunning = 15;

    public Vector3 toMove = Vector3.zero;
    private Vector3 lastDirection = Vector3.up;

    private CharacterController characterController;
    private GameObject myCamera;
    [SerializeField] private MoveScript moving;

    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        myCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Enter()
    {
        anim.SetBool("Running", true);
        player.currentTimeState = 0;
        lastDirection = moving.lastDirection;
        speed = player.GetSpeed();
    }

    public override void Execute()
    {
        float lastStamina = player.stamina.Stamina;
        float lastSpeed = speed;
        toMove = Vector3.zero;
        if (Input.GetKey(player.upKey))
            toMove += myCamera.transform.up;
        if (Input.GetKey(player.downKey))
            toMove -= myCamera.transform.up;
        if (Input.GetKey(player.rightKey))
            toMove += myCamera.transform.right;
        if (Input.GetKey(player.leftKey))
            toMove -= myCamera.transform.right;

        toMove = new Vector3(toMove.x, 0, toMove.z);
        toMove.Normalize();
        if (toMove.magnitude > 0)
        {

            float magnitudVector = (lastDirection - toMove).magnitude;
            if (magnitudVector == 0)
            {
                float value = Time.deltaTime * costStaminaPerRunningSecond * (speed - player.normalSpeed);
                player.stamina.ModifiyStamina(-value);
                if (speed < maxSpeed)
                {
                    speed += Time.deltaTime * incrementSpeedSecond;
                    speed = player.ChangeSpeed(speed);

                }
            }
            else if (magnitudVector > 0 && magnitudVector < 1)
            {
                toMove = (toMove / resistenceToGirRunning + lastDirection).normalized;
                lastDirection = toMove;
                float secondvalue = Time.deltaTime * costStaminaPerRunningSecond * (speed - player.normalSpeed);
                player.stamina.ModifiyStamina(-secondvalue);
                if (speed > player.normalSpeed)
                {
                    speed -= Time.deltaTime * incrementSpeedSecond / 4;
                    speed = player.ChangeSpeed(speed);

                }
            }
            else if (magnitudVector < 1.75f)
            {
                toMove = (toMove / resistenceToGirRunning + lastDirection).normalized;
                lastDirection = toMove;
                float secondvalue = Time.deltaTime * costStaminaPerRunningSecond * (speed - player.normalSpeed);
                player.stamina.ModifiyStamina(-secondvalue);
                if (speed > player.normalSpeed)
                {
                    speed -= Time.deltaTime * incrementSpeedSecond / 2;
                    speed = player.ChangeSpeed(speed);

                }
            }
            else
            {
                toMove = lastDirection;
                float otherValue = Time.deltaTime * costStaminaPerRunningSecond * (speed - player.normalSpeed);
                player.stamina.ModifiyStamina(-otherValue);
                if (speed < maxSpeed)
                {
                    speed += Time.deltaTime * incrementSpeedSecond;
                    speed = player.ChangeSpeed(speed);

                }
            }


            gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, toMove);

        }
        else
        {

            toMove = lastDirection;
            float otherValue = Time.deltaTime * costStaminaPerRunningSecond * (speed - player.normalSpeed);
            player.stamina.ModifiyStamina(-otherValue);
            if (speed < maxSpeed)
            {
                speed += Time.deltaTime * incrementSpeedSecond;
                speed = player.ChangeSpeed(speed);
            }

        }

        CollisionFlags collisionFlags = characterController.Move(toMove * Time.deltaTime * speed);

        if((collisionFlags & CollisionFlags.Sides) != 0)
        {
            speed = lastSpeed;
            player.stamina.Stamina = lastStamina;
        }

        if (player.stamina.Stamina <= 0)
            player.ChangeState(PlayerScript.State.MOVING);
    }

    public override void Exit()
    {
        moving.lastDirection = lastDirection;
        anim.SetBool("Running", false);
        player.ChangeSpeed(player.normalSpeed);
    }
}
