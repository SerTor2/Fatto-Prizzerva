using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Time_Task", menuName = "Tasks/Time_Task")]
    public class TimeTask : TimeRelatedTask
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            currentTime = 0;        // for some reason, if i do not give current time a value in the ONENABLE method it start with a value of 7xxx,xxx with is a bit odd
        }

        public override void Tick(float _deltaTime)
        {
            currentTime += _deltaTime;
        }

        public override TaskStatus Check()
        {
            if (currentTime >= targetTime)
            {
                currentTaskState = TaskStatus.ACHIEVED;

            } else
            {
                currentTaskState = TaskStatus.IN_PROGRESS;
            }

            previousState = currentTaskState;

            return currentTaskState;

        }

    }

}


