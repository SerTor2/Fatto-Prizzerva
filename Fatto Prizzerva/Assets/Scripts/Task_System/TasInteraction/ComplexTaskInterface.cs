using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
                    Type type; 

                    foreach (Task childTask in _complexTargetTask.GetTasksList())
                    {
                        type = childTask.TaskInterface_Type;

                        if (type != null)
                            requiredInterfaces.Add(gameObject.AddComponent(type) as TasKInterface);
                        else
                            Debug.LogError("COMPLEX TASK INTERFACE: A type of interface ouuld not be found");

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


