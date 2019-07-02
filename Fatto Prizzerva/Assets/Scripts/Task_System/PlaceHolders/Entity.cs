using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public delegate void DeathEventHandler();           // delegado con la definicion de los metodos del evento
    public event DeathEventHandler death;               // evento que llamaremos internamente


    // actino is a method without parameters
    public void SubscriveToDeathEvent(Action _methodToSubscrive)
    {
        death += SayHi;
    }
    public void OnDeath()
    {
        death?.Invoke();        // esto llamara a los metodos del delegado
    }
    private void SayHi()
    {
        Debug.LogError("Hi");
    }



    private void Awake()
    {
        death += SayHi;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Matamos a la entidad
            OnDeath();

            Debug.LogError("Task " +  Tasks.TasksManager.Instance.GetTask(2 ,Tasks.TasksManager.Instance.GetGameTasks()).name);

        }

    }

    private void OnDisable()
    {
        death -= SayHi;
    }
}
