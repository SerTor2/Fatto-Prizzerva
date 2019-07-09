using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tasks;

public class Entity : MonoBehaviour
{
    public delegate void DeathEventHandler();           // delegado con la definicion de los metodos del evento
    public event DeathEventHandler death;               // evento que llamaremos internamente

    [SerializeField] DestroyTaskInterface destroyTaskInteraction;

    // actino is a method without parameters
    public void SubscriveToDeathEvent(Action _methodToSubscrive)
    {
        death += SayHi;
    }
    public void OnDeath()
    {
        death.Invoke();        // esto llamara a los metodos del delegado
        destroyTaskInteraction.SetTaskAsCompleted(TaskStatus.ACHIEVED);

    }
    private void SayHi()
    {
        Debug.LogError("Hi");

    }



    private void Awake()
    {
        SubscriveToDeathEvent(SayHi);

        // inicializamos las tareas que tienen que ver consigo mismo en el awake
        destroyTaskInteraction.InitializeTask();
    }



    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Matamos a la entidad
            OnDeath();

            // Debug.LogError("Task " +  Tasks.TasksManager.Instance.GetTask(2 ,Tasks.TasksManager.Instance.GetGameTasks()).name);

        }


    }

    private void OnDisable()
    {
        death -= SayHi;
    }
}
