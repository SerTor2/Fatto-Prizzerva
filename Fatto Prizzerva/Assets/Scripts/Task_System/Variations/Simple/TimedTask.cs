using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{

    /// <summary>
    /// Do something before the time ends
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "Timed_Task", menuName = "Tasks/Timed_Task")]
    public class TimedTask : SimpleTask
    {

        [SerializeField] private float timeBeforeExpiration;
        private float currentTime;

        protected override void OnEnable()
        {
            currentTime = timeBeforeExpiration;
        }

        public override void Tick(float _deltaTime)
        {
            currentTime -= _deltaTime;
        }

        public override TaskStatus Check()
        {
            if (currentTime <= 0)
            {
                return TaskStatus.FAILED;

            } else
            {
                // wait until someone tells you the task has been acomplished
                return TaskStatus.IN_PROGRESS;
            }





        }

    }
}


