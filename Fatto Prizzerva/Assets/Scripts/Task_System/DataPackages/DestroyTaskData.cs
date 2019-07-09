using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    public class DestroyTaskData : TaskData
    {
        [SerializeField] MonoBehaviour entityToDestroy;

        public DestroyTaskData (MonoBehaviour _entityToKill) 
        {
            entityToDestroy = _entityToKill;
        }

        
    }
}


