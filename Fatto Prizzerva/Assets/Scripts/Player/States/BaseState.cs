using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{

    [SerializeField] protected PlayerScript player;
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

}
