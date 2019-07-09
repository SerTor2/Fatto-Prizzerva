
using UnityEngine;

namespace Tasks
{
    [CreateAssetMenu(fileName = "TasksBlackboard", menuName = "Tasks/Blackboard")]
    public class TasksBlackboard : ScriptableObject
    {
        [Header("Player realted Data")]
        [SerializeField] private TestPlayer player;
        public TestPlayer Player { get; set; }

        [Header("Enviorment related Data")]
        public GameObject placeHolder;

        [Header("Enemy related Data")]
        public GameObject placeh9lder;






    }

   


}

