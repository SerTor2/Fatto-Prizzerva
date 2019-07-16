using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackScript : BaseState
{
    private CharacterController characterController;
    private float speed = 0;
    private float timeToExit = 0;
    private float decreseSpeed = 10;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    public override void Enter()
    {
        player.currentTimeState = 0;
        player.GetStatsKnockBack(out speed, out timeToExit, out direction);
        decreseSpeed = speed / timeToExit;
    }

    public override void Execute()
    {
        speed -= Time.deltaTime * decreseSpeed;
        if (speed <= 0)
            speed = 0.1f;
        player.currentTimeState += Time.deltaTime;
        if (player.currentTimeState >= timeToExit)
            player.ChangeState(PlayerScript.State.MOVING);
        CollisionFlags collisionFlags = characterController.Move(direction * Time.deltaTime * speed);
    }

    public override void Exit()
    {

    }
}
