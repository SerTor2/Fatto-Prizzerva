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

        [SerializeField] private Task hostTask;

        [SerializeField] private TextMeshProUGUI taskName;
        [SerializeField] private TextMeshProUGUI taskDescription;

        [Header("Control")]
        [SerializeField] private bool isPined;



        // GETTERS --------------------------------------------- //
        public Task GetReferencedTask()
        {
            return hostTask;
        }
        public bool GetIsPinned()
        {
            return isPined;
        }

        // SETTERS ------------------------------------------ //
        public void SetPinned(Toggle _toggleComponent)
        {
            isPined = _toggleComponent.isOn;

            // Set text type
            if (isPined)
                taskName.fontStyle =  FontStyles.Bold;
            else
                taskName.fontStyle = FontStyles.Normal;
        }


        public void SetupData(Task _task, bool _mindDescription)
        {
            hostTask = _task;

            taskName.text = hostTask.GetName();

            if (_mindDescription)
                taskDescription.text = hostTask.GetDescription();
        }


        
    }
}


