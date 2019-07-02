using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class KillTask : SimpleTask
    {
        [SerializeField] private MonoBehaviour entityToKill;
        private bool isEntityDead;



        public override void Setup(TasksBlackboard _blackboard)
        {
            base.Setup(_blackboard);
        }


        public override TaskStatus Check()
        {
            if (isEntityDead)
                CurrentTaskState = TaskStatus.ACHIEVED;
            else
                CurrentTaskState = TaskStatus.IN_PROGRESS;

            PreviousState = CurrentTaskState;

            return CurrentTaskState;
        }

    }
}


