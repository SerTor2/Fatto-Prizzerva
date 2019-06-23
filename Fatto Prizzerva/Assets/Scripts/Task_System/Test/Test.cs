using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptable", menuName = "Tasks/Test")]
public class Test : ScriptableObject
{
    [TextArea] public string objectName;
    [TextArea] public string description;

    private bool isReady;

    public bool GetIsReady()
    {
        return isReady;
    }
    public void SetIsReady(bool _isReady)
    {
        isReady = _isReady;
    }

}



