using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    // [CreateAssetMenu(fileName = "Complex_Task", menuName = "Tasks/Complex_Task")]
    [System.Serializable]
    public abstract class ComplexTask : Task
    {
        [Header("Setup")]
        [SerializeField] protected List<Task> tasksToAcomplish;

        [Header("Control")]
        [SerializeField] protected byte acomplishedTasks;

        public List<Task> GetTasksList ()
        {
            return tasksToAcomplish;
        }

        protected override void OnEnable()
        {
            acomplishedTasks = 0;
        }

        // no hacemos override del metodo tick debido a que cada tarea compleja lo usa de un 
        // modo distinto


    }
}


