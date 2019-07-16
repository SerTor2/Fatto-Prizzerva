using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState currentState;

    private BaseState previousState;

    public void ChangeState(BaseState nextState)
    {
        if (currentState != null) currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }

    public void ExecuteState()
    {
        if (currentState != null) currentState.Execute();
    }

    public void BackToPrevoius()
    {

        currentState.Exit();

        currentState = previousState;

        currentState.Enter();

    }
}


