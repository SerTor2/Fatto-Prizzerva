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

        // Control
        private bool isPanelActive;                 // flag to control if the object is being used actively
        public bool isTransitioning = false;        // flag to control if the animation is playing    



        private void Awake()
        {
            tasksCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();

            if (activeTasksContainer == null || completedTasksContainer == null || failedTasksContainer == null)
                Debug.LogError("ERROR_TASKS_CANVAS_CONTROLLER: Faltan asignar transformadas del canvas");



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

            // simple tasks
            if (_taskToAdd is SimpleTask)
            {
                // cutre para ir probando
                GameObject _canvasedTask = Instantiate(simpleTaskPrefab, activeTasksContainer);
                VisualTask _visualTask = _canvasedTask.GetComponent<VisualTask>();

                _visualTask.SetupData(_taskToAdd.GetName());

            } else if (_taskToAdd is ComplexTask)
            {

                // cutre para ir probando
                GameObject _canvasedTask = Instantiate(simpleTaskPrefab, activeTasksContainer);
                VisualTask _visualTask = _canvasedTask.GetComponent<VisualTask>();

                _visualTask.SetupData(_taskToAdd.GetName());

            }


        }


        #endregion




    }

}

