using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualTask : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;


    public void SetupData(string _taskName)
    {
        taskName.text = _taskName;
    }

    public void SetupData(string _taskName, string _taskDescription)
    {
        taskName.text = _taskName;
        taskDescription.text = _taskDescription;
    }

}
