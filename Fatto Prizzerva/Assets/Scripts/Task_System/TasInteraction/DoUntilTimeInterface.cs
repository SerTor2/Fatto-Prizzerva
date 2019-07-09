using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class DoUntilTimeInterface : TasKInterface
    {

        private void OnValidate()
        {
            if (!(TargetTask is DoSomethingUntilTimeTask) && TargetTask != null)
            {
                Debug.LogError("The assigned task is not from the expected type " + typeof(DoSomethingUntilTimeTask));
                TargetTask = null;
            }
        }

        public override void InitializeTask()
        {
            TasksManager.Instance.SetupTask(TargetTask);
        }

    }
}


