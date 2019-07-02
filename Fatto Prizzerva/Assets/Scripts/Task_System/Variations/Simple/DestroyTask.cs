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
        // seria muy guay el poder hacer que este objeto se subscriva a un evento de destruccion del objeto 
        // y que al SER DESTRUIDO llame a su evdnto y por tanto esta tarea sienta las repercusiones

        [SerializeField] private MonoBehaviour objectToDestroy;
        private bool isObjectDestroyed;

        // dani falta el setup en el cual adquiriras el objeto y te subscriviras a su evento de muerte OnDestruction o OnDestroy
        public override void Setup(TasksBlackboard _blackboard)
        {
            base.Setup(_blackboard);


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

    }
}


