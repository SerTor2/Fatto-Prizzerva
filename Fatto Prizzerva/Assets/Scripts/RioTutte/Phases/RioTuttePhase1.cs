using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioTuttePhase1 : MonoBehaviour
{
    public enum State { MOVING, KNOCKBACK, DASH}; //dash provisional
    public State currentState = State.MOVING;
    private StateMachine stateMachine;
    public BaseState moving;
    public BaseState knockback;
    public BaseState dash;

    public void StartExecution(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        ChangeState(State.DASH);
    }

    public void Execute()
    {
        stateMachine.ExecuteState();
        /*if (Input.GetKey(KeyCode.Space))
            ChangeState(State.DASH);*/
    }

    public void ChangeState(State _newState)
    {
        switch(_newState)
        {
            case State.MOVING:
                stateMachine.ChangeState(moving);
                break;
            case State.KNOCKBACK:
                //stateMachine.ChangeState(knockback);
                break;
            case State.DASH:
                stateMachine.ChangeState(dash);
                //provisional
                break;
        }
    }
}
