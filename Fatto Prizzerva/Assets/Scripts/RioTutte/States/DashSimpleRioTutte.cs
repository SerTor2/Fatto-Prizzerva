using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSimpleRioTutte : BaseState
{
    private RioTutteMainScript mainScript;
    private CharacterController characterController;
    private float currentTime = 0;
    public float speedDash = 30;
    public float timeDelay = 0.15f;
    public float timeDash = 1f;
    private bool dashing = false;

    private void Awake()
    {
        mainScript = GetComponent<RioTutteMainScript>();
        player = mainScript.GetPlayer();
        characterController = mainScript.GetCharacterController();
    }

    public override void Enter()
    {
        currentTime = 0;
        mainScript.direction = (player.gameObject.transform.position - gameObject.transform.position).normalized;
    }

    public override void Execute()
    {
        if(currentTime > timeDelay)
        {
            if (!dashing)
            {
                dashing = true;
            }
            CollisionFlags collisionFlags = characterController.Move(mainScript.direction * Time.deltaTime * speedDash / 2);
            if(currentTime > timeDash)
            {
                switch (mainScript.phase)
                {
                    case 1:
                        mainScript.GetPhase1().ChangeState(RioTuttePhase1.State.MOVING);
                        break;
                }
            }
        }
        else
        {
            CollisionFlags collisionFlags = characterController.Move(-1 *mainScript.direction * Time.deltaTime * speedDash / 2f);
        }

        currentTime += Time.deltaTime;
    }

    public override void Exit()
    {
        dashing = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject == player.gameObject && dashing)
        {
            player.StartKnockBack(35, 0.3f, (player.gameObject.transform.position - gameObject.transform.position).normalized);
            if (currentTime < timeDash - timeDash / 10)
                currentTime = timeDash - timeDash / 10;
        }
    }

}
