using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : BaseState
{
    private float gravity = 0;
    private float forceJump = 0;
    private CharacterController characterController;
    private Vector3 direction;

    private GameObject myCamera;
    private bool planning = false;


    private void Start()
    {
        characterController = player.GetComponent<CharacterController>();
        myCamera = Camera.main.gameObject;
    }

    public override void Enter()
    {
        forceJump = player.GetForceJump();
        gravity = player.GetGravity();
        player.currentTimeState = 0;
        planning = player.GetPlanningPlant();
        direction = player.toMove;
        player.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward, direction);
    }

    public override void Execute()
    {
        if(forceJump > -2)
        {
            CollisionFlags collisionFlags = characterController.Move(Vector3.up * forceJump * Time.deltaTime + direction * Time.deltaTime * 2f);

            if((collisionFlags & CollisionFlags.Below) != 0)
            {
                player.ChangeState(PlayerScript.State.MOVING);
            }

        }
        else
        {
            if(planning)
                player.ChangeState(PlayerScript.State.PLANNING);
            else
                player.ChangeState(PlayerScript.State.MOVING);

        }
        forceJump -= gravity * Time.deltaTime;
    }

    public override void Exit()
    {

    }
}
