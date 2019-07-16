using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : BaseState
{
    #region VARIABLES
    private float speed = 0;
    private float distanceWithCamera = 0;
    private float verticalSpeed = 0;
    [SerializeField] private float gravity = 15;

    private bool onGround = true;

    private Vector3 toMove = Vector3.zero;
    public Vector3 lastDirection = Vector3.up;


    private CharacterController characterController;

    private GameObject myCamera;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        myCamera = Camera.main.gameObject;
        distanceWithCamera = myCamera.transform.position.y - player.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.currentState != PlayerScript.State.INSIDEPLANT && player.currentState != PlayerScript.State.FLYINGKICK 
            && player.currentState != PlayerScript.State.JUMPING && player.currentState != PlayerScript.State.PLANNING)
            CheckGravity();
    }

    public void MoveCameraUpLayer()
    {
        myCamera.gameObject.transform.position = new Vector3(myCamera.transform.position.x, gameObject.transform.position.y + distanceWithCamera, myCamera.transform.position.z);

    }

    private void CheckGravity()
    {
        verticalSpeed -= gravity * Time.deltaTime;
        CollisionFlags collisionFlags = characterController.Move(new Vector3(0, verticalSpeed, 0) * Time.deltaTime);
        if ((collisionFlags & CollisionFlags.Below) != 0)
        {
            onGround = false;
        }
        else
            onGround = true;
        if (onGround)
            verticalSpeed = 0;
    }

    public override void Enter()
    {
        player.currentTimeState = 0;
        speed = player.GetSpeed();
    }

    public override void Execute()
    {
        speed = player.GetSpeed();
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
            lastDirection = toMove;
            gameObject.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, toMove);

        }
        speed = player.ChangeSpeed(speed);
        CollisionFlags collisionFlags = characterController.Move(toMove * Time.deltaTime * speed);
    }

    public override void Exit()
    {
    }
}
