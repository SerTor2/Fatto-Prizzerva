using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

namespace Tasks
{

    public enum TaskStatus
    {
        NONE = -3,                  // ERROR AND INITIAL STATE
        ACHIEVED = 1,               // resultado positivo
        IN_PROGRESS = 0,            // en progreso (sigue haciendo el check)
        FAILED = -1                 // resultado negativo (fallaste wey)
    }

    public abstract class Task : ScriptableObject
    {
        protected TasksBlackboard blackboard;
        private Type taskInterface_Type;                    // interface the task uses in order to comunicate with
        System.Random rnd;

        [SerializeField] protected string taskName = "NONE";
        [SerializeField] [TextArea] private string taskDescription = "NONE";

        [Header("Reward")]
        [SerializeField] private Reward reward;

        [Header("Identification")]
        [SerializeField] private int taskId;
        private static List<int> tasksIdOnUse;              // ID EN USO



        private TaskStatus currentTaskState;
        protected TaskStatus CurrentTaskState { get => currentTaskState; set => currentTaskState = value; }
        private TaskStatus previousState;
        protected TaskStatus PreviousState { get => previousState; set => previousState = value; }
        public Type TaskInterface_Type { get => taskInterface_Type; protected set => taskInterface_Type = value; }

        public TaskStatus GetPreviousTaskState()
        {
            return PreviousState;
        }
        public TaskStatus GetCurrentTaskState()
        {
            return CurrentTaskState;
        }
        public string GetName ()
        {
            return taskName;
        }
        public string GetDescription()
        {
            return taskDescription;
        }
        public int GetTaskId()
        {
            return taskId;
        }


        protected virtual void OnEnable()
        {
            rnd = new System.Random();

            if (taskName == "") taskName = "UNNNAMED_TASK";
            if (taskDescription == "") taskDescription = "NODESCRIPTION_SET";


        }



        // setup para tareas que REQUIERAN DE BLACKBOARD (datos generales)
        public virtual void Setup (TasksBlackboard _blackboard)
        {
            blackboard = _blackboard;
            SetRandomId();
            // Override this method
        }
        public virtual void SetTaskData()
        {

        }
        public virtual void SetTaskData(TaskData _taskDataPackage)
        {
            // override this method for each task that needs additional wordl info
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

        private void SetRandomId()
        {
            if (tasksIdOnUse == null)
            {
                tasksIdOnUse = new List<int>();
            }

            // dani mejora esto
            taskId = tasksIdOnUse.Count + 1;

            //Debug.LogError(taskId);
            tasksIdOnUse.Add(taskId);
        }

        public void ApplyReward (Entity _targetOfTheReward)
        {
            if (reward)
            {
                reward.ApplyReward();
            }
        }

        // external way of making a task end 
        public virtual void ForceTaskFinish(TaskStatus _desiredEndStatus = TaskStatus.ACHIEVED)
        {
            if (_desiredEndStatus != TaskStatus.NONE) 
                currentTaskState = _desiredEndStatus;
        }

    }

    



}


