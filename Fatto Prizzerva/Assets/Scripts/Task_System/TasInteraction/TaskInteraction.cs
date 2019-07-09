using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    public abstract class TasKInterface : MonoBehaviour
    {
        // task we are controling
        [SerializeField] private Task targetTask;
        public Task TargetTask { get => targetTask; protected set => targetTask = value; }

        public abstract void InitializeTask();
        public virtual void SetTaskAsCompleted(TaskStatus _desiredTaskStatus)
        {
            Debug.LogWarning("Updating task!");
            TargetTask.ForceTaskFinish(_desiredTaskStatus);
        }

    }
}

