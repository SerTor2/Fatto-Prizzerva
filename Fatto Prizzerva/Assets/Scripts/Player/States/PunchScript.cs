using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : BaseState
{
    public float costStaminaPerPunch = 5;
    [SerializeField] private float speedPunch = 3;
    [SerializeField] private float damageBase = 2;
    [SerializeField] private float timerDamage = 0.25f;

    private BaseState moving;

    [SerializeField] private SphereCollider colliderPunch;
    // Start is called before the first frame update
    void Start()
    {
        colliderPunch = gameObject.GetComponent<SphereCollider>();
        moving = player.moving;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Enter()
    {
        player.anim.SetTrigger("Fist");
        player.stamina.ModifiyStamina(-costStaminaPerPunch);
        player.currentTimeState = 0;
        player.ChangeSpeed(speedPunch);
        moving.Enter();
    }

    public override void Execute()
    {
        player.currentTimeState += Time.deltaTime;
        if (player.currentTimeState >= timerDamage && player.currentTimeState < 0.3f)
            colliderPunch.enabled = true;
        if (player.currentTimeState >= 0.3f)
            player.ChangeState(PlayerScript.State.MOVING); //player.stateMachine.ChangeState(player.moving);
        moving.Execute();

    }

    public override void Exit()
    {
        player.ResetSpeed();
        player.currentTimePunch += Time.deltaTime;
        colliderPunch.enabled = false;
        moving.Exit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemie")
        {
            if (player.currentState == PlayerScript.State.PUNCHING) //(player.stateMachine.currentState == this)
                other.gameObject.GetComponent<EnemieBasic>().MoveDirectionHit((other.gameObject.transform.position - gameObject.transform.position).normalized, damageBase * speedPunch);
            player.ChangeState(PlayerScript.State.MOVING);
        }
    }
}
