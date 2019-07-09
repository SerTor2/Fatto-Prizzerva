using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class DestroyTaskInterface : TasKInterface
    {
        [SerializeField] private MonoBehaviour objectToDestroy;

        private void OnValidate()
        {
            if (!(TargetTask is DestroyTask) && TargetTask != null)
            {
                Debug.LogError("The assigned task is not from the expected type " + typeof(DestroyTask));
                TargetTask = null;
            }
        }

        public override void InitializeTask()
        {
            DestroyTaskData taskData = new DestroyTaskData(objectToDestroy);
            TasksManager.Instance.SetupTask(TargetTask, taskData);
        }

    }
}

