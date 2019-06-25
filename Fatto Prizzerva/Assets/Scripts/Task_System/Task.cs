using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{

    public enum TaskStatus
    {
        NONE = -3,                       // ERROR AND INITIAL STATE
        ACHIEVED = 1,               // resultado positivo
        IN_PROGRESS = 0,            // en progreso (sigue haciendo el check)
        FAILED = -1                 // resultado negativo (fallaste wey)
    }

    public abstract class Task : ScriptableObject
    {
        protected TasksBlackboard blackboard;
        [SerializeField] protected string taskName = "NONE";
        [SerializeField] [TextArea] private string taskDescription = "NONE";

        private TaskStatus currentTaskState;
        protected TaskStatus CurrentTaskState { get => currentTaskState; set => currentTaskState = value; }
        private TaskStatus previousState;
        protected TaskStatus PreviousState { get => previousState; set => previousState = value; }


        protected bool isPinned = false;              // marcamos si estamos siguie3ndo o no esta tarea (mostrar si esta pinned en la interfaz de juego)

        //[Header("Events")] 
        //public UnityEvent OnCompletion_Evnt;
        //public UnityEvent OnFail_Evnt;


        public TaskStatus GetPreviousTaskState()
        {
            return PreviousState;
        }
        public TaskStatus GetCurrentTaskState()
        {
            return CurrentTaskState;
        }
        public bool GetIsPinned ()
        {
            return isPinned;
        }
        public string GetName ()
        {
            return taskName;
        }
        public string GetDescription()
        {
            return taskDescription;
        }


        protected virtual void OnEnable()
        {
            if (taskName == "") taskName = "UNNNAMED_TASK";
            if (taskDescription == "") taskDescription = "NODESCRIPTION_SET";
        }


        // funciones para añadir datos de funcionamiento a la tarea 
        public virtual void Setup()           
        {
            // Override this method
            throw new NotImplementedException();

        }
        public virtual void Setup (TasksBlackboard _blackboard)
        {
            blackboard = _blackboard;
        }
        public virtual void Setup(GameObject _objectToFollow)
        {
            // Override this method
            throw new NotImplementedException();

        }


        // funcion de update (en el caso de ser necesrio)
        public virtual void Tick(float _deltaTime)
        {
            // override IF necesary
        }


        // Funcion de check
        public virtual TaskStatus Check()
        {
            // Override this Method
            throw new NotImplementedException();
        }


    }

    



}


