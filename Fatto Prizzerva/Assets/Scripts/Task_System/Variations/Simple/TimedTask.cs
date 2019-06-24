using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasks
{

    /// <summary>
    /// Do something before the time ends NOT YET DONE
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "Timed_Task", menuName = "Tasks/Timed_Task")]
    public class TimedTask : TimeRelatedTask
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            currentTime = targetTime;
        }

        public override void Tick(float _deltaTime)
        {
            currentTime -= _deltaTime;
        }

        // Dani hace falta una condicion de ACHIEVED
        public override TaskStatus Check()
        {
            if (currentTime <= 0)
            {
                currentTaskState = TaskStatus.FAILED;

            } else
            {
                currentTaskState = TaskStatus.IN_PROGRESS;
                // wait until someone tells you the task has been acomplished
            }

            previousState = currentTaskState;
            return currentTaskState;


        }

    }
}


