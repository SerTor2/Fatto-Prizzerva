using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    [RequireComponent(typeof(TasksBlacboard))]
    public class TasksManager : MonoBehaviour
    {
        // singleton
        private static TasksManager _instance;
        public static TasksManager Instance { get { return _instance; } }


        [Header("References")]
        [SerializeField] private TasksCanvasController tasksCanvasController;
        [SerializeField] private TasksBlacboard tasksBlackboard;

        [Header("Perfomance")]
        [SerializeField] private int tickPerSecond;
        private float currentTime;
        private float timeForCheck;

        [Header("Task Lists")]
        [SerializeField] private List<Task> gameTasks;              // list with ALL the game tasks
        [SerializeField] private List<Task> activeTasks;            // tasks being checked
        [SerializeField] private List<Task> achievedTasks;          // tasks completed SUCCESFULLY
        [SerializeField] private List<Task> failedTasks;            // tasks completed in FAILURE
    

        #region ENGINE METHODS

        private void Awake()
        {
            #region(Singleton Pattern)
            DontDestroyOnLoad(this);
            // Si _instancia tiene una referencia que no somos nosotros nos destruimos 
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
            #endregion

            tasksBlackboard = GetComponent<TasksBlacboard>();

            // null checks
            if (tasksCanvasController == null) Debug.LogError("TASKS_MANAGER_NULL: tasksCanvasController");
            if (tasksBlackboard == null) Debug.LogError("TASKS_MANAGER_NULL: No blackboard found");


            // Performance setup
            timeForCheck = ((1.0000f / (float)tickPerSecond) * 0.6000f);     // ticks per second caching

            // initializations
            activeTasks = new List<Task>();
            achievedTasks = new List<Task>();
            failedTasks = new List<Task>();

            TasksSetup();

        }

        private void Start()
        {
            foreach (Task task in activeTasks)
            {
                tasksCanvasController.AddTaskToCanvas(task);
            }

        }


        private void Update()
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeForCheck)
            {
                // check for completion and tick
                foreach (Task task in activeTasks)
                {
                    // CHECK 
                    if (CheckTask(task))
                        return;

                    // TICK
                    TickTask(task);
                }

                currentTime = 0f;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public List<Task> GetActiveTasks ()
        {
            return activeTasks;
        }  
        public List<Task> GetAllTasks ()
        {
            return gameTasks;
        }

        public static TasksManager GetInstance()
        {
            return Instance;
        }

        public void ActivateTask(Task _taskToActivate, bool _safeActivation = true)
        {
            // check if task is already active or not (just in case)
            if (_safeActivation)
            {
                if (activeTasks.Contains(_taskToActivate) == false)
                {
                    SetTaskCategory(_taskToActivate, TaskStatus.IN_PROGRESS);
                }
                else
                {
                    Debug.LogError("ERROR-LIST: The task you are trying to add is alredy active");
                }
            }
            else
            {
                // Insecure addition USE WITH CAUTION
                SetTaskCategory(_taskToActivate, TaskStatus.IN_PROGRESS);
            }

        }


        #endregion

        #region PRIVATE METHODS

        private void TasksSetup()
        {
            foreach (Task _task in gameTasks)
            {
                Debug.LogWarning("Initiallizing " + _task.name);

                // SIMPLE ----------------------------------------- //
                if (_task is SimpleTask)
                {
                    SimpleTask _simpleTask = _task as SimpleTask;

                    if (_simpleTask is ReachTask)
                        _simpleTask.Setup(tasksBlackboard.GetPlayer().gameObject);

                }
                // COMPLEX --------------------------------------- //   
                else if (_task is ComplexTask)
                {
                    ComplexTask _complexTask = _task as ComplexTask;

                    foreach (Task _internalTask in _complexTask.GetTasksList())
                    {
                        // Dani esto estaria bien que el setup de los hijos los haga el padre
                        if (_internalTask is ReachTask)
                            _internalTask.Setup(tasksBlackboard.GetPlayer().gameObject);
                    }

                }

                ActivateTask(_task, true);
            }
        }
        private bool CheckTask(Task _task)
        {
            TaskStatus previousTaskState = _task.GetPreviousTaskState();
            TaskStatus newTaskState = _task.Check();

            Debug.Log("Checking " + _task.name + " with status " + newTaskState);

            if ((int)previousTaskState != (int)newTaskState)
            {
                SetTaskCategory(_task, newTaskState, previousTaskState);
                return true;
            }
            else
            {
                // Category change not needed
                return false;
            }
        }


        private void TickTask(Task _task)
        {
            _task.Tick(timeForCheck);       // el dt pasado tiene una precision muy baja BUSCAR SOLUCION A ESTO
        }

        private void SetTaskCategory(Task _task, TaskStatus _newStatus, TaskStatus _previousStatus = TaskStatus.NONE)
        {
            // REMOVING OF LIST
            switch (_previousStatus)
            {
                case TaskStatus.NONE:
                    break;
                case TaskStatus.ACHIEVED:
                    activeTasks.Remove(_task);
                    break;
                case TaskStatus.IN_PROGRESS:
                    activeTasks.Remove(_task);
                    break;
                case TaskStatus.FAILED:
                    failedTasks.Remove(_task);
                    break;
                default:
                    Debug.LogError("ERROR_TASKS: Someghing went wrong, make sure the task " + _task.name + " of type " + _task.GetType() + " inherits properly");
                    break;
            }


            // ADDING ON LIST
            switch (_newStatus)
            {
                case TaskStatus.NONE:
                    Debug.LogError("ERROR_TASKS: Someghing went wrong, make sure the task " + _task.name + " of type " + _task.GetType() + " inherits properly");
                    break;

                case TaskStatus.ACHIEVED:
                    achievedTasks.Add(_task);
                    break;

                case TaskStatus.IN_PROGRESS:
                    activeTasks.Add(_task);
                    break;

                case TaskStatus.FAILED:
                    failedTasks.Add(_task);
                    break;

                default:
                    Debug.LogError("ERROR_TASKS: Someghing went wrong, make sure the task " + _task.name + " of type " + _task.GetType() + " inherits properly");
                    break;
            }
        }

        #endregion
    }
}






