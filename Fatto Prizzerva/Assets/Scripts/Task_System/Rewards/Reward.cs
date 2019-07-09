using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasks
{
    // Clas with contains what the player is rewarded 
    [CreateAssetMenu(fileName = "Reward_Object", menuName = "Reward")]
    public class Reward : ScriptableObject
    {
        [SerializeField] private string rewardName;
        [SerializeField] [TextArea] private string rewardDescription;


        public virtual void ApplyReward()
        {
            Debug.LogWarning("DANOLE UN REWARD AL host");
        }

        // Se pueden aplicar al jugador o al enemigo

        // Pude activar el desbloqueo de algo GDD  pacifico en caballos
        // skin
        // accion <--------<
    }
}


