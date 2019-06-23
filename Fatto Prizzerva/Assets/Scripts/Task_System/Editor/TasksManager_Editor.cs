using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Tasks
{
    [CustomEditor(typeof(TasksManager))]
    public class TasksManager_Editor : Editor
    {
        TasksManager manager;


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            manager = (TasksManager)target;


            EditorGUILayout.LabelField("Game Tasks Status");

            GUIStyle s = new GUIStyle(EditorStyles.textField);


            // mostramos la lista de tareas activas
            foreach (Task task in manager.GetAllTasks())
            {
                // set data on each entry
                string _taskEntry;
                _taskEntry = task.name + "\t" 
                            + task.GetCurrentTaskState();

                switch (task.GetCurrentTaskState())
                {
                    case TaskStatus.ACHIEVED:
                        s.normal.textColor = Color.green;
                        break;

                    case TaskStatus.FAILED:
                        s.normal.textColor = Color.red;
                        break;

                    case TaskStatus.IN_PROGRESS:
                        s.normal.textColor = Color.blue;
                        break;

                    case TaskStatus.NONE:
                        s.normal.textColor = Color.black;
                        break;
                }
                EditorGUILayout.LabelField(new GUIContent(_taskEntry),s);

                 // DIBUJOADO DE LISTA DE SUBTAREAS
                if (task is ComplexTask)
                {
                    ComplexTask complexTask = (ComplexTask)task;
                    foreach (Task internalTask in complexTask.GetTasksList())
                    {
                        string _subTaskEnbtry;
                        _subTaskEnbtry = "\t" + internalTask.name + "\t" + internalTask.GetCurrentTaskState();

                        switch (internalTask.GetCurrentTaskState())
                        {
                            case TaskStatus.ACHIEVED:
                                s.normal.textColor = Color.green;
                                break;

                            case TaskStatus.FAILED:
                                s.normal.textColor = Color.red;
                                break;

                            case TaskStatus.IN_PROGRESS:
                                s.normal.textColor = Color.blue;
                                break;

                            case TaskStatus.NONE:
                                s.normal.textColor = Color.black;
                                break;
                        }

                        EditorGUILayout.LabelField(new GUIContent(_subTaskEnbtry), s);

                    }

                }
                    

            }

        }
    }
}


