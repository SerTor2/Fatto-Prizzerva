using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Una tarea Random se basa en el siguiente comportamiento:
/// - Las tareas se han de completar pero el orden de complecion no es importante
/// - Si una de las tareas falla la serie entera retorna fallo
/// - 
/// </summary>


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "RandomOrderTaskList_Task", menuName = "Tasks/Complex_Task/RandomOrder")]
    public class ComplexRandomTask : ComplexTask
    {

        public override TaskStatus Check()
        {

            for (int index = 0; index < tasksToAcomplish.Count; index++)
            {
                Task _currentTask = tasksToAcomplish[index];
                TaskStatus _tempTaskStatus = _currentTask.Check();

                // si hemos conseguido completar una tarea lo marcamos y ademas sumamos el contador
                // si el contador indica que todas estan acabadas lo remitimos 
                if (_tempTaskStatus == TaskStatus.ACHIEVED)
                {
                    acomplishedTasks++;

                    Debug.LogWarning("microtask " + _currentTask.name + " acomplished " + acomplishedTasks + "/" + tasksToAcomplish.Count);

                    // Achieved check
                    if (acomplishedTasks == tasksToAcomplish.Count)
                    {
                        currentTaskState = TaskStatus.ACHIEVED;
                    }

                }
                else if (_tempTaskStatus == TaskStatus.FAILED)
                {
                    currentTaskState = _tempTaskStatus;
                }
                else
                {
                    currentTaskState = TaskStatus.IN_PROGRESS;
                }

            }

            previousState = currentTaskState;

            return currentTaskState;

        }

        // tick de todos las tareas debido a que su completacion es aleatoria
        public override void Tick(float _deltaTime)
        {
            foreach (Task task in tasksToAcomplish)
            {
                task.Tick(_deltaTime);
            }

        }



    }
}


