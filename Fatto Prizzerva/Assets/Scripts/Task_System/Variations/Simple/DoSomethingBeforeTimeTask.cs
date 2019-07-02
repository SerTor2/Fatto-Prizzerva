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
    [CreateAssetMenu(fileName = "Do before x Time", menuName = "Tasks/Do Something Before x Time")]
    public class DoSomethingBeforeTimeTask : TimeRelatedTask
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
                CurrentTaskState = TaskStatus.FAILED;

            } else
            {
                CurrentTaskState = TaskStatus.IN_PROGRESS;
                // wait until someone tells you the task has been acomplished
            }

            PreviousState = CurrentTaskState;
            return CurrentTaskState;


        }

    }
}


