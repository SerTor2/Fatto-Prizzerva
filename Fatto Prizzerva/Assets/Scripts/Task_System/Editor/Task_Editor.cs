using UnityEngine;
using UnityEditor;



namespace Tasks
{
    // custom editor for the tasks
    [CustomEditor(typeof(Task))]
    public class Task_Editor : Editor
    {
        private Color labelColor;
        private Vector2 colorFieldPosition;
        private Vector2 colorFieldSize;

        private void OnEnable()
        {
            colorFieldSize = new Vector2(20, 20);
            colorFieldPosition = new Vector2(0, 0);
        }

        //EditorUtility.SetDirty();
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Task task = (Task)target;           // referncia al objeto al cual estamos modificando el inspector

            switch (task.GetCurrentTaskState())
            {
                case (TaskStatus.ACHIEVED):
                    labelColor = Color.green;
                    break;

                case (TaskStatus.FAILED):
                    labelColor = Color.red;
                    break;

                case (TaskStatus.IN_PROGRESS):
                    labelColor = Color.blue;
                    break;

                case (TaskStatus.NONE):
                    labelColor = Color.black;
                    break;
            }

            GUILayout.Toggle(false, new GUIContent("HI"));

            EditorGUI.ColorField(new Rect(0f, 0f, 10, 2), labelColor);

        }


    }
}


