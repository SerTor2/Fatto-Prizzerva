
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSetup))]
public class GameSetup_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Utiliza este SO para setear los tags y todos los datos que necesites acceder desde el game manager y de forma externa",
                                MessageType.Info);

        base.OnInspectorGUI();
    }

}
