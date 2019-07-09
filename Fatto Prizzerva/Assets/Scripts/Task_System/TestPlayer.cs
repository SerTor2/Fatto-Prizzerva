using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tasks;

public class TestPlayer : MonoBehaviour
{
    TasksManager tasksManager;

    [SerializeField] TasKInterface reachPositionTaskInteraction;

    private void Start()
    {
        tasksManager = TasksManager.GetInstance();
    }

    private void OnEnable()
    {
        reachPositionTaskInteraction.InitializeTask();
    }







}
