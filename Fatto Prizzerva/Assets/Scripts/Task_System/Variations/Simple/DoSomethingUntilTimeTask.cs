using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "For Time Task", menuName = "Tasks/Do Something Until x time")]
    public class DoSomethingUntilTimeTask : TimeRelatedTask
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
                CurrentTaskState = TaskStatus.ACHIEVED;

            } else
            {
                CurrentTaskState = TaskStatus.IN_PROGRESS;
            }

            PreviousState = CurrentTaskState;

            return CurrentTaskState;

        }

    }

}


