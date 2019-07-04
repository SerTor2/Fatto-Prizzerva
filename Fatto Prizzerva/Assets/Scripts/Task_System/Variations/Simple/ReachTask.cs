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
        [SerializeField] private GameObject objectToFollow;     
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private Transform targetTransform;
        // temporal (poner en algun tipo de archivo config)
        [Header("Control")]
        [SerializeField] float targetRadius = 0.1f;         // margen de error para el calculo de la distancia al objetivo


        public override void Setup(TasksBlackboard _blackboard)
        {
            base.Setup(_blackboard);

            objectToFollow = _blackboard.Player.gameObject;
            targetTransform = _blackboard.reachTarget;
        }

        // check for completion of the task (currently you can not fail it)
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


