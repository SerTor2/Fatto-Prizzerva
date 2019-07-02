using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TasksCanvasController : MonoBehaviour
    {
        // References
        private CanvasGroup tasksCanvasGroup;

        [Header("Interaction")]
        [SerializeField] private KeyCode togglePanel_Button;

        [Header("Prefabs")]
        [SerializeField] private GameObject simpleTaskPrefab;

        [Header("Canvas Transforms")]
        [SerializeField] private Transform activeTasksContainer;
        [SerializeField] private Transform completedTasksContainer;
        [SerializeField] private Transform failedTasksContainer;

        [Header("Listed Tasks")]
        [SerializeField] private List<VisualTask> canvasedTasks_Lst;

        // Control
        private bool isPanelActive;                 // flag to control if the object is being used actively
        public bool isTransitioning = false;        // flag to control if the animation is playing    



        private void Awake()
        {
            tasksCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();

            if (activeTasksContainer == null || completedTasksContainer == null || failedTasksContainer == null)
                Debug.LogError("ERROR_TASKS_CANVAS_CONTROLLER: Faltan asignar transformadas del canvas");

            canvasedTasks_Lst = new List<VisualTask>();

            isPanelActive = (tasksCanvasGroup.alpha == 1f) ? 
                (isPanelActive = true) : (isPanelActive = false);
        }


        private void Update()
        {

            PlayerInput();
        }




        #region Visual
        public void ShowTasks()
        {
            tasksCanvasGroup.alpha = 1f;
        }
        public void HideTasks()
        {
            tasksCanvasGroup.alpha = 0f;
        }
        public IEnumerator ShowTasks(float _deltaAlpha)
        {
            isTransitioning = true;
            while (tasksCanvasGroup.alpha < 1.0f)
            {
                yield return tasksCanvasGroup.alpha += _deltaAlpha * Time.deltaTime;
            }
            isTransitioning = false;

        }
        public IEnumerator HideTasks(float _deltaAlpha)
        {
            isTransitioning = true;
            while (tasksCanvasGroup.alpha > 0.0f)
            {
                yield return tasksCanvasGroup.alpha -= _deltaAlpha * Time.deltaTime;
            }
            isTransitioning = false;

        }
        #endregion

        #region Logic 

        private void PlayerInput()
        {
            if (Input.GetKeyDown(togglePanel_Button))
            {
                if (isTransitioning == false)
                {
                    if (isPanelActive)
                        StartCoroutine(HideTasks(1f));

                    else
                        StartCoroutine(ShowTasks(1f));

                }
                isPanelActive = !isPanelActive;

            }
        }

        public void AddTaskToCanvas (Task _taskToAdd)
        {
            // el canvas que hay durnate el juego muestra informacion de objetivos muy simplificada 
            // de momento se muestran igual las tareas simples y las complejas

            GameObject _canvasedTask = Instantiate(simpleTaskPrefab, activeTasksContainer);
            VisualTask _visualTask = _canvasedTask.GetComponent<VisualTask>();

            canvasedTasks_Lst.Add(_visualTask);
            _visualTask.SetupData(_taskToAdd, false);

        }

        public void RemoveTaskFromCanvas(Task _task)
        {
            VisualTask _visualtaskToDestroy = null;

            // LOCATE -------------------------------------------------- //
            foreach (VisualTask visualTask in canvasedTasks_Lst)
            {
                Task _referencedTask = visualTask.GetReferencedTask();
                Debug.Log(_referencedTask);

                if (_referencedTask == _task)
                {
                    _visualtaskToDestroy = visualTask;
                    break;
                }                    
            }

            // REMOVAL ----------------------------------------------- //
            if (_visualtaskToDestroy)
            {
                Debug.Log("TASKSCANVAS: Destroying task " + _task.GetName());
                canvasedTasks_Lst.Remove(_visualtaskToDestroy);
                Destroy(_visualtaskToDestroy.gameObject);
            
            } else
            {
                Debug.LogError("TASKSCANVAS: Unable to destroy task " + _task.GetName());

            }
            

        }


        #endregion




    }

}

