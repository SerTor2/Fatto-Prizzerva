using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    public class ComplexTaskInterface : TasKInterface
    {
        private List<TasKInterface> requiredInterfaces;

#if Editor
        private void OnValidate()
        {

            if (TargetTask != null)
            {
                if (!(TargetTask is ComplexTask))
                {
                    Debug.LogError("The assigned task is not from the expected type " + typeof(ComplexTask));
                    TargetTask = null;

                }
                else
                {
                    // FOR EVERY TASK INTO THE COMPLEX TASK (at the moment just one level of deph) we create an interface to set them up)
                    ComplexTask _complexTargetTask = (TargetTask as ComplexTask);

                    foreach (Task childTask in _complexTargetTask.GetTasksList())
                    {
                        if (childTask is ReachTask)
                        {
                            requiredInterfaces.Add(this.gameObject.AddComponent<ReachTaskInterface>());
                        }
                        else if (childTask is DestroyTask)
                        {
                            requiredInterfaces.Add(this.gameObject.AddComponent<DestroyTaskInterface>());
                        }
                        else if (childTask is DoSomethingUntilTimeTask)
                        {
                            requiredInterfaces.Add(this.gameObject.AddComponent<DoUntilTimeInterface>());
                        }
                        else if (childTask is DoSomethingBeforeTimeTask)
                        {
                            requiredInterfaces.Add(this.gameObject.AddComponent<DoBeforeTimeInterface>());
                        }
                        else
                        {
                            Debug.LogError("COMPLEX TASK INTERFACE: This complex task does have a task without a interface");
                        }
                    }
                }

            } else
            {
                foreach (TasKInterface tasKInterface in requiredInterfaces)
                {
                    // aplicamos un delay a la destruccion del objeto (debido a que estamos en edit mode y de otro modo unity no puede destruir el objeto al estar
                    // en un OnValidate)
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        Debug.LogWarning("Destroying " + tasKInterface);
                        DestroyImmediate(tasKInterface);
                    };             
                }
            }

           
        }

#endif

        public override void InitializeTask()
        {
            foreach (TasKInterface childTaskInterface in requiredInterfaces)
            {
                TasksManager.Instance.SetupTask(childTaskInterface.TargetTask,false);

            }

            TasksManager.Instance.SetupTask(TargetTask);
        }

    }
}


