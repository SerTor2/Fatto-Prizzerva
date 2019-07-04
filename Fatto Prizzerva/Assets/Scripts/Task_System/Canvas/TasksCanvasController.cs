using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        [Header("Estilos visualess")]
        public FontStyles normalStyle;
        public FontStyles achievedStyle;
        public FontStyles failedStyle;
        public FontStyles inProgressStyle;

        public FontStyle customFontStyle;
        public TMPro.ColorMode colorMode;


        // GETTERS ---------------------------------------- //


        // SETTERS ---------------------------------------- //



        private void Awake()
        {
            tasksCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();

            //if (activeTasksContainer == null || completedTasksContainer == null || failedTasksContainer == null)
            //    Debug.LogError("ERROR_TASKS_CANVAS_CONTROLLER: Faltan asignar transformadas del canvas");

            canvasedTasks_Lst = new List<VisualTask>();

            isPanelActive = (tasksCanvasGroup.alpha == 1f) ? 
                (isPanelActive = true) : (isPanelActive = false);
        }


        private void Update()
        {
            PlayerInput();
        }




        #region Visual
        public IEnumerator ShowTasks(float _deltaAlpha)
        {
            isTransitioning = true;
            while (tasksCanvasGroup.alpha < 1.0f)
            {
                yield return tasksCanvasGroup.alpha += _deltaAlpha * Time.deltaTime;
            }
            isTransitioning = false;

            tasksCanvasGroup.interactable = true;
            tasksCanvasGroup.blocksRaycasts = true;
        }
        public IEnumerator HideTasks(float _deltaAlpha)
        {
            tasksCanvasGroup.interactable = false;
            tasksCanvasGroup.blocksRaycasts = false;

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
                    {
                        StartCoroutine(HideTasks(1f));
                    }
                    else
                    {
                        StartCoroutine(ShowTasks(1f));   
                    }

                }
                isPanelActive = !isPanelActive;
            }
        }

        public void AddTaskToCanvas (Task _taskToAdd)
        {

            GameObject _canvasedTask = Instantiate(simpleTaskPrefab, activeTasksContainer);
            VisualTask _visualTask = _canvasedTask.GetComponent<VisualTask>();

            canvasedTasks_Lst.Add(_visualTask);
            _visualTask.SetupData(_taskToAdd, false);

        }
        /// <summary>
        /// Localiza el objeto que representa a esta tarea en el canvas
        /// </summary>
        /// <returns></returns>
        private VisualTask LocateTaskRelative( Task _task)
        {
            foreach (VisualTask visualTask in canvasedTasks_Lst)
            {
                Task _referencedTask = visualTask.GetReferencedTask();
                Debug.Log(_referencedTask);

                if (_referencedTask == _task)
                {
                    return visualTask;
                }
            }

            return null;
        }

        private void RemoveTaskFromCanvas(Task _task)
        {
            VisualTask _visualTaskToRemove = null;

            // LOCATION -------------------------------------------------- //
            _visualTaskToRemove = LocateTaskRelative(_task);

            // REMOVAL ----------------------------------------------------- //
            if (_visualTaskToRemove)
            {
                Debug.Log("TASKSCANVAS: Destroying task " + _task.GetName());

                canvasedTasks_Lst.Remove(_visualTaskToRemove);
                Destroy(_visualTaskToRemove.gameObject);
            
            } else
            {
                Debug.LogError("TASKSCANVAS: Unable to destroy task " + _task.GetName());
            }
            
        }
        
        /// <summary>
        /// Cambiamos la apareicnai visual de la tarea basandonos en el nuevo estado de esta (completada, fallada ...)
        /// </summary>
        /// <param name="_newTaskState"></param>
        public void UpdatetaskStatus (Task _task)
        {
            TaskStatus newTaskStatus = _task.GetCurrentTaskState();
            VisualTask _targetVisualTask = LocateTaskRelative(_task);

            switch (newTaskStatus)
            {
                case TaskStatus.NONE:
                    break;

                case TaskStatus.ACHIEVED:
                    //RemoveTaskFromCanvas(_task);            // TEMPORAL
                    _targetVisualTask.UpdateVisualStyle(achievedStyle);

                    break;

                case TaskStatus.IN_PROGRESS:
                    _targetVisualTask.UpdateVisualStyle(inProgressStyle);

                    break;

                case TaskStatus.FAILED:
                    _targetVisualTask.UpdateVisualStyle(failedStyle);
                    break;

                default:
                    break;
            }

        }

        


        #endregion




    }

}

