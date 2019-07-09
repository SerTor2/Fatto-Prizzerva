using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{


    public class TasksManager : MonoBehaviour
    {
        // singleton
        private static TasksManager _instance;
        public static TasksManager Instance { get { return _instance; } }
        public PseudoGM gameController;

        [SerializeField] private TasksBlackboard blackboard;

        [Header("References")]
        [SerializeField] private TasksCanvasController tasksCanvasController;

        [Header("Perfomance")]
        [SerializeField] private int updatesPerSecond;
        private float currentUpdateTime;
        private float timeForUpdate;
        [SerializeField] private int checksPerSecond;
        private float currentCheckTime;
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


            // null checks
            if (tasksCanvasController == null) Debug.LogError("TASKS_MANAGER_NULL: tasksCanvasController");


            // Performance setup
            timeForUpdate = ((1.0000f / (float)updatesPerSecond) * 0.6000f);     // ticks per second caching
            timeForCheck = ((1.0000f / (float)checksPerSecond) * 0.6000f);     // ticks per second caching

            // initializations
            //activeTasks = new List<Task>();
            //achievedTasks = new List<Task>();
            //failedTasks = new List<Task>();

            TasksBlackboardSetup();

            //foreach (Task task in gameTasks)
            //{
            //    SetupTask(task);
            //}

        }

        private void Start()
        {
   

        }


        private void Update()
        {
            currentUpdateTime += Time.deltaTime;
            currentCheckTime += Time.deltaTime;

            // CHECK ------------------------------------------------------------------------ //
            if (currentCheckTime >= timeForCheck)
            {
                foreach (Task task in gameTasks)
                {
                    CheckTask(task);

                    // si hemos completado la tarea avisamos al manager visual para que la quite
                    if (task.GetCurrentTaskState() == TaskStatus.ACHIEVED)
                    {
                        // --------------------- DEPENDANT ------------------------ //
                        //task.ApplyReward(gameController.Player);            // aplicamos recompensa al jugador (ahora mismo es algo directo)
                        // --------------------- -------- ------------------------ //
                        //tasksCanvasController.UpdatetaskStatus(task);
                    }
                }
                currentCheckTime = 0f;
            }

            // UPDATE ------------------------------------------------------------------------ //
            if (currentUpdateTime >= timeForUpdate)
            {
                // check for completion and tick
                foreach (Task task in activeTasks)
                {
                    TickTask(task);
                }
                currentUpdateTime = 0f;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public List<Task> GetActiveTasks()
        {
            return activeTasks;
        }
        public List<Task> GetGameTasks()
        {
            return gameTasks;
        }

        public bool GetAllTasksComplete()
        {
            if (activeTasks.Count == 0)
                return true;
            else
                return false;
        }

        public Task GetTask(int _taskID, List<Task> _tasksListToSearchFrom)
        {
            Task currentTask;

            for (int index = 0; index < gameTasks.Count; index++)
            {
                currentTask = gameTasks[index];

                // if is a complex task we start doing recursion ............................. //
                if (currentTask is ComplexTask)
                {
                    List<Task> childrenTasks = new List<Task>();
                    childrenTasks = (currentTask as ComplexTask).GetTasksList();

                    foreach (Task childTask in childrenTasks)
                    {
                        GetTask(_taskID, childrenTasks);
                    }

                    // If is a simple task we just check if the id coincides targetId == taskId ....... //
                }
                else
                {
                    if (CheckIsDesiredTask(_taskID, currentTask))
                        return currentTask;
                }
            }

            Debug.LogWarning("WARNING: la tarea con indice " + _taskID + " no ha sido encontrada en la lista GAMETASKS");
            return null;
        }


        private bool CheckIsDesiredTask(int _targetIndex, Task _taskToCheck)
        {
            // Is it the task we are looking for?
            if (_taskToCheck.GetTaskId() == _targetIndex)
                return true;
            else
                return false;
            
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

        public void SetupTask(Task _task, TaskData _taskInitializationData, bool _addToActiveTasks = true)
        {
            if (!(gameTasks.Contains(_task)))
                gameTasks.Add(_task);

            // le damos la informacion comun de todas las tareas
            _task.Setup(blackboard);

            tasksCanvasController.AddTaskToCanvas(_task);
            tasksCanvasController.UpdatetaskStatus(_task);

            System.Type taskType = _task.GetType();

            Debug.LogError(taskType);

            // le pasamos los datos que pueda necesitar
            if (_task is ReachTask)
                (_task as ReachTask).SetTaskData(_taskInitializationData);
            if (_task is DestroyTask)
                (_task as DestroyTask).SetTaskData(_taskInitializationData);



            // debido a la propia recursividad de este metodo y el hecho que no queremos que todas las subtareas se añadan a las tareas activas utilizaremos el flag
            if (_addToActiveTasks)
                ActivateTask(_task, true);

            // Propagation
            if (_task is ComplexTask)
            {
                // buscamos sus hijos
                foreach (Task task in (_task as ComplexTask).GetTasksList())
                {
                    SetupTask(task, false);
                }
            }
        }
        public void SetupTask(Task _task, bool _addToActiveTasks = true)
        {


            // le damos la informacion comun de todas las tareas
            _task.Setup(blackboard);

            tasksCanvasController.AddTaskToCanvas(_task);
            tasksCanvasController.UpdatetaskStatus(_task);

            // debido a la propia recursividad de este metodo y el hecho que no queremos que todas las subtareas se añadan a las tareas activas utilizaremos el flag
            if (_addToActiveTasks)
                ActivateTask(_task, true);

            // Propagation
            if (_task is ComplexTask)
            {
                // buscamos sus hijos
                foreach (Task task in (_task as ComplexTask).GetTasksList())
                {
                    SetupTask(task, false);
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void TasksBlackboardSetup()
        {
            blackboard.Player = GameObject.FindGameObjectWithTag("Player").GetComponent<TestPlayer>();
        }

        private void TasksSetup()
        {
            
        }
        

        private bool CheckTask(Task _task)
        {
            TaskStatus previousTaskState = _task.GetPreviousTaskState();
            TaskStatus newTaskState = _task.Check();

            Debug.Log("Checking " + _task.name + " with status " + newTaskState);

            if ((int)previousTaskState != (int)newTaskState)
            {
                SetTaskCategory(_task, newTaskState, previousTaskState);
                tasksCanvasController.UpdatetaskStatus(_task);

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
            _task.Tick(timeForUpdate);
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

            // asi es como deberia estar
            // tasksCanvasController.UpdatetaskStatus(_task);

        }

        #endregion
    }
}






