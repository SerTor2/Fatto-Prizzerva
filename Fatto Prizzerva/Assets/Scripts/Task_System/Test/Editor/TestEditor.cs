using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(Test))]
public class TestEditor : Editor
{

    public override void OnInspectorGUI()
    {


        Test test = (Test)target;
        EditorUtility.SetDirty(test);


        DrawDefaultInspector();

        test.SetIsReady( GUILayout.Toggle(test.GetIsReady(),new GUIContent("Ready?")));
 

    }
}
