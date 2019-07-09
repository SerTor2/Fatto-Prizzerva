using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class VisualTask : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Task hostTask;

        [Header("GUI")]
        [SerializeField] private TextMeshProUGUI taskName;
        [SerializeField] private TextMeshProUGUI taskDescription;
        [SerializeField] private TaskStatus currentTaskStatus;

        [Header("Control")]           
        [SerializeField] private bool isPined;
        private Transform parent;
        private static int lastPinnedobject_Idx;


        // GETTERS --------------------------------------------- //
        public Task GetReferencedTask()
        {
            return hostTask;
        }
        public bool GetIsPinned()
        {
            return isPined;
        }
        private void GetLastPinnedObject(ref int _lastPinnedobject_Idx)
        {
           _lastPinnedobject_Idx = -1;

            for (int index = 0; index < parent.childCount; index++)
            {
                VisualTask visualTask = parent.GetChild(index).GetComponent<VisualTask>();

                if ((visualTask != this) && visualTask.GetIsPinned())
                    _lastPinnedobject_Idx = visualTask.transform.GetSiblingIndex();
            }

        }

        // SETTERS -------------------------------------------- /
        public void SetPinned(Toggle _toggleComponent)
        {
            isPined = _toggleComponent.isOn;

            // Set text STYLE
            if (isPined)
                UpdateVisualStyle(FontStyles.Bold);
            else
                UpdateVisualStyle(FontStyles.Normal);

            // movemos el transform hacia arriba para quedar justo despues del ultimo objeto pineado
            GetLastPinnedObject(ref lastPinnedobject_Idx);

            if (isPined)
            {
                // modificamos la posicin
                if (lastPinnedobject_Idx != -1)
                    this.transform.SetSiblingIndex(lastPinnedobject_Idx + 1);      // nos ponemos detras del ultimo
                else
                    this.transform.SetAsFirstSibling();                              // nos ponemos los primeros

            // movemos el resto de pineados haca la nueva posicion
            } else
            {
                int _myIndex = this.transform.GetSiblingIndex();

                //// movemos el resto de objetos 
                //for (int index = _myIndex + 1; index < parent.childCount; index++)
                //{
                //    Transform visualTask = parent.GetChild(index);
                //    visualTask.SetSiblingIndex(visualTask.GetSiblingIndex() - 1);
                //}

                // Colocamos el unipineado al final de la lista
                this.transform.SetAsLastSibling();

            }

        }
        private void UpdateVisualStyle(TMPro.FontStyles _newStyle)
        {
            taskName.fontStyle = _newStyle;
        }
        public void UpdateVisualStyle (CustomFontStyle _newStyle)
        {
            _newStyle.ApplyStyleToText(taskName);
        }
        public void SetCurrentStateDebug(TaskStatus _taskState)
        {
            currentTaskStatus = _taskState;
        }

        // MAIN --------------------------------------------- //
        public void SetupData(Task _task, bool _mindDescription)
        {
            hostTask = _task;
            parent = transform.parent;

            taskName.text = hostTask.GetName();

            if (_mindDescription)
                taskDescription.text = hostTask.GetDescription();

        }


    }
}


