using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RioTutteMainScript : MonoBehaviour
{
    public int phase = 0;
    private RioTuttePhase1 phase1;
    private StateMachine stateMachine;
    private CharacterController characterController;
    private PlayerScript player;
    public Vector3 direction = Vector3.back;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        characterController = GetComponent<CharacterController>();
        phase1 = GetComponent<RioTuttePhase1>();
        stateMachine = GetComponent<StateMachine>();
    }

    private void Start()
    {
        ChangePhase(1);
    }

    // Update is called once per frame
    void Update()
    {
        switch(phase)
        {
            case 1:phase1.Execute();
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    private void ChangePhase(int _newPhase)
    {
        switch(_newPhase)
        {
            case 1:
                phase = 1;
                phase1.StartExecution(stateMachine);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    public PlayerScript GetPlayer()
    {
        return player;
    }

    public CharacterController GetCharacterController()
    {
        return characterController;
    }

    public RioTuttePhase1 GetPhase1()
    {
        return phase1;
    }
}
