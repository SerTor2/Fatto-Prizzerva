using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    /// <summary>
    /// Task used to check if you destroyed x OBJECT 
    /// </summary>
    [CreateAssetMenu(fileName = "Destroy_Task", menuName = "Tasks/Destroy_Entity")]
    public class DestroyTask : SimpleTask
    {
        private MonoBehaviour objectToDestroy;
        private bool isObjectDestroyed;

        public override void Setup(TasksBlackboard _blackboard)
        {
            base.Setup(_blackboard);
        }

        

        protected override void OnEnable()
        {
            base.OnEnable();
            isObjectDestroyed = false;
            TaskInterface_Type = typeof(DestroyTaskInterface);

        }

        public override TaskStatus Check()
        {
            if (isObjectDestroyed)
                CurrentTaskState = TaskStatus.ACHIEVED;
            else
                CurrentTaskState = TaskStatus.IN_PROGRESS;

            PreviousState = CurrentTaskState;

            return CurrentTaskState;

        }

        public override void ForceTaskFinish(TaskStatus _desiredEndStatus)
        {
            // base.ForceTaskFinish(_desiredEndStatus);
            isObjectDestroyed = true;
        }


    }

     
}


