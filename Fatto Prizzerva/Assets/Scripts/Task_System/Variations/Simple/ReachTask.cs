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
        // temporal (poner en algun tipo de archivo config)
        [Header("Control")]
        [SerializeField] float targetRadius = 0.1f;         // margen de error para el calculo de la distancia al objetivo


        // we do need to give the data to the task prior to check it, this way we only need one method for the Check and it's easier to mantain
        public override void Setup(GameObject _objectToFollow)
        {
            objectToFollow = _objectToFollow;
        }

        // check for completion of the task (currently you can not fail it)
        public override TaskStatus Check()
        {

            Vector3 _objectPosition = objectToFollow.transform.position;
            Vector3 _distanceToTarget_Vect = targetPosition - _objectPosition;
            float _distanceToTarget = Vector3.Magnitude(_distanceToTarget_Vect);

            if (_distanceToTarget <= targetRadius)
                currentTaskState = TaskStatus.ACHIEVED;
            else
                currentTaskState = TaskStatus.IN_PROGRESS;

            previousState = currentTaskState;

            return currentTaskState;
        }


    }
}


