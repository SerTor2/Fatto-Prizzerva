using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tasks
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Test_Task", menuName = "Tasks/Test_Task")]
    public class TestTask : SimpleTask
    {

        [Header("Task Specific Atributes")]
        [SerializeField] private const string yourName = "Daniel";

        public override void Setup()
        {
            Debug.Log("Nothing to setup cause this is a test, but your name is " + yourName);
            Debug.Log("Even when I'm wrong I'm true");
        }

        // this test DOES NOT HAVE A CHECK method
        // because its just a test mate

    }

}


