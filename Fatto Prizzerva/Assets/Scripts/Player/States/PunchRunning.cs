using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchRunning : BaseState
{
    CharacterController characterController;
    public float costStaminaPerPunch = 5f;
    private float speed = 0;
    private float multiply = 2;

    [SerializeField] private RunScript run;

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        run = GetComponent<RunScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Enter()
    {
        player.currentTimeState = 0;
        player.stamina.ModifiyStamina(-costStaminaPerPunch * (player.speed - player.normalSpeed));
        speed = player.run.GetComponent<RunScript>().speed * multiply;
        player.speed = speed;
    }

    public override void Execute()
    {
        player.currentTimeState += Time.deltaTime;

        if (player.currentTimeState >= 0.15f)
            player.ChangeState(PlayerScript.State.MOVING);
        
        CollisionFlags collisionFlags = characterController.Move(run.toMove * Time.deltaTime * speed);

    }

    public override void Exit()
    {
        player.currentTimePunch += Time.deltaTime;
        player.ResetSpeed();
    }
}
