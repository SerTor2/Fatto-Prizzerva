using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class TasksCanvasController : MonoBehaviour
    {
        // References
        private CanvasGroup tasksCanvasGroup;

        // Control
        private bool isPanelActive;

        // interaction
        [SerializeField] private KeyCode togglePanel_Button;




        private void Awake()
        {
            tasksCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();

            if (tasksCanvasGroup.alpha == 1f)
                isPanelActive = true;
            else
                isPanelActive = false;
        }


        private void Update()
        {

            if (Input.GetKeyDown(togglePanel_Button))
            {
                if (isPanelActive)
                    StartCoroutine(HideTasks(1f)); 
                else
                    StartCoroutine(ShowTasks(1f));

                isPanelActive = !isPanelActive;

            }


        }

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
            while (tasksCanvasGroup.alpha < 1.0f)
            {
                yield return tasksCanvasGroup.alpha += _deltaAlpha * Time.deltaTime;
            }

        }
        public IEnumerator HideTasks(float _deltaAlpha)
        {
            while (tasksCanvasGroup.alpha > 0.0f)
            {
                yield return tasksCanvasGroup.alpha -= _deltaAlpha * Time.deltaTime;
            }

        }


    }

}

