using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class ReachTaskData : TaskData
    {
        GameObject objectToFollow;
        GameObject targetObject;

        public ReachTaskData(GameObject _objectToFollow, GameObject _targetObject)
        {
            objectToFollow = _objectToFollow;
            targetObject = _targetObject;
        }

        public GameObject GetObjectToFollow()
        {
            return objectToFollow;
        }
        public GameObject GetTargetObject()
        {
            return targetObject;
        }
    }
}

