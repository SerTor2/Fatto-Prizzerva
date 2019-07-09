using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Reach_Task", menuName = "Tasks/Reach_Task")]
    public class ReachTask : SimpleTask
    {

        [Header("Task Specific Atributes")]
        private GameObject objectToFollow;     
        private Transform targetTransform;

        [Header("Control")]
        [SerializeField] float targetRadius = 0.1f;         // margen de error para el calculo de la distancia al objetivo

        protected override void OnEnable()
        {
            base.OnEnable();
            TaskInterface_Type = typeof(ReachTaskInterface);
        }

        public override void Setup(TasksBlackboard _blackboard)
        {
            base.Setup(_blackboard);

            objectToFollow = _blackboard.Player.gameObject;
        }

        public override void SetTaskData(TaskData _taskData)
        {
            objectToFollow = (_taskData as ReachTaskData).GetObjectToFollow();
            targetTransform = (_taskData as ReachTaskData).GetTargetObject().transform;
        }

        // check for completion of the task 
        public override TaskStatus Check()
        {

            Vector3 _objectPosition = objectToFollow.transform.position;
            Vector3 _distanceToTarget_Vect = targetTransform.position - _objectPosition;
            float _distanceToTarget = Vector3.Magnitude(_distanceToTarget_Vect);

            if (_distanceToTarget <= targetRadius)
                CurrentTaskState = TaskStatus.ACHIEVED;
            else
                CurrentTaskState = TaskStatus.IN_PROGRESS;

            PreviousState = CurrentTaskState;

            return CurrentTaskState;
        }


    }


    
}


