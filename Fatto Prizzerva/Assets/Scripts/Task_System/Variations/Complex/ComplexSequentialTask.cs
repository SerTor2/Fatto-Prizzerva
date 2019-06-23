using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Una secuencia es una lista de tareas cuyoo comportamiento se basa en:
/// - Para acabar una tarea de la lista esta ha de ser completada (normal)
/// - Para iniciar la siguiente tarea antes la anterior ha de estar completada
/// - Si una tarea falla la tarea macro (la que las contiene) retornara fallo y la tarea acabara
/// </summary>


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SequentialTaskList_Task", menuName = "Tasks/Complex_Task/Sequence")]
    public class ComplexSequentialTask : ComplexTask
    {
        private int currentTaskIndex;

        protected override void OnEnable()
        {
            currentTaskIndex = 0;
        }

        public override TaskStatus Check()
        {
            Task _currentTask = tasksToAcomplish[currentTaskIndex];
            TaskStatus _tempTaskStatus = _currentTask.Check();
            
            if (_tempTaskStatus == TaskStatus.FAILED)
            {
                currentTaskState = TaskStatus.FAILED;             
                
            } else if (_tempTaskStatus == TaskStatus.ACHIEVED)
            {
                Debug.LogWarning("microtask " + _currentTask.name + " acomplished ");

                if (currentTaskIndex == tasksToAcomplish.Count - 1)
                {
                    // serie completada
                    currentTaskState = TaskStatus.ACHIEVED;

                } else
                {
                    // sino sumamos uno al indice para proceder con el siguiente elemento de la lista
                    currentTaskIndex ++;
                }
            } else
            {
                currentTaskState = TaskStatus.IN_PROGRESS;
            }

            previousState = currentTaskState;

            return currentTaskState;

        }

        public override void Tick(float _deltaTime)
        {
            tasksToAcomplish[currentTaskIndex].Tick(_deltaTime);   
        }

    }
}


