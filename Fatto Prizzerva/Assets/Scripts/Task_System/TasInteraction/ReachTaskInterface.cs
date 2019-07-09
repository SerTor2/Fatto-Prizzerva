using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    public class ReachTaskInterface : TasKInterface
    {
        public GameObject objectToFollow;
        public GameObject targetObject;

        private void OnValidate()
        {
            if (!(TargetTask is ReachTask) && TargetTask != null)
            {
                Debug.LogError("The assigned task is not from the expected type " + typeof(ReachTask));
                TargetTask = null;
            }
        }

        public override void InitializeTask()
        {    
            ReachTaskData taskData = new ReachTaskData(objectToFollow, targetObject);
            TasksManager.Instance.SetupTask(TargetTask, taskData);
        }


    }
}

