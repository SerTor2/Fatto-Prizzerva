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
        int deph = 0;

        private List<Task> scannedTasks;

        public Color inProgress_Clr = Color.blue;
        public Color completed_Clr = Color.green;
        public Color failed_Clr = Color.red;
        public Color other_Clr = Color.black;

        public string debugInfo;

        private void Awake()
        {
            scannedTasks = new List<Task>();
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            manager = (TasksManager)target;


            EditorGUILayout.LabelField("Game Tasks Status\r\n");
            EditorGUILayout.HelpBox(debugInfo, MessageType.Warning,true);


            MonitorTaskList(manager.GetGameTasks());
        }

        private void MonitorTaskList (List<Task> _listToMonitor)
        {
            // Body drawing
            foreach (Task task in _listToMonitor)
            {
                ScanTask(task, ref deph);
            }

            // restauramos la lista de tareas escaneadas
            scannedTasks = new List<Task>();

            deph = 0;
        }

        // Scan task for children tasks RECURSIVE
        private void ScanTask (Task _task, ref int _deph)
        {

            // if the task has been already processed we stop to evade a infinite loop
            if (scannedTasks.Contains(_task))
            {
                debugInfo = "Las tareas no pueden estar repetidas";
                //Debug.LogError("ERROR_TASKMANAGER-EDITOR: "  + debugInfo + " " + _task.name);
                return;
            } else
            {
                debugInfo = "Funcionando normalmente";
                scannedTasks.Add(_task);
            }

            CreateTaskLabel(_task, _deph);

            if (_task is ComplexTask)
            {
                _deph += 1;

                List<Task> tasksList = (_task as ComplexTask).GetTasksList();
                int tasksListCount = (_task as ComplexTask).GetTasksList().Count;

                foreach (Task task in tasksList)
                {
                    ScanTask(task, ref _deph);              // scan for children

                    // if it is the last task on the list we decrease one deph level
                    if (task == tasksList[tasksListCount - 1])
                    {
                        _deph -= 1;
                    }

                }


            }



        }

        // create the label
        private void CreateTaskLabel (Task _task, int _deph)
        {
            string taskEntry = "";

            // add tabulations _____________________________________ //
            if (_deph > 0)
            {
                // add tabulations
                for (int i = 0; i < _deph; i++)
                {
                    taskEntry += "\t";
                }
            }
            // add id ______________________________________________ //
            taskEntry += _task.GetTaskId() + "___";

            // Set Task Color _____________________________________ //
            GUIStyle s = new GUIStyle(EditorStyles.textField);
            switch (_task.GetCurrentTaskState())
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

            // add task name
            taskEntry += _task.name;

            // iF TIME TASK ADD TIME 
            if (_task is TimeRelatedTask)
            {
                taskEntry += "\t";
                taskEntry += (int)(_task as TimeRelatedTask).GetCurrentTime() + " / " + (_task as TimeRelatedTask).GetTargetTime();
            }


            // create label _____________________________________ //
            EditorGUILayout.LabelField(new GUIContent(taskEntry), s);

            scannedTasks.Add(_task);

        }


        
    }
}


