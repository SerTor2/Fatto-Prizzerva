using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public abstract class TimeRelatedTask : SimpleTask
    {
        [SerializeField] protected float targetTime;
        protected float currentTime;

        public float GetCurrentTime() {
            return currentTime;
        }
        public float GetTargetTime() {
            return targetTime;
        }

    }


}





