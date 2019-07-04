using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clas with contains what the player is rewarded 
[CreateAssetMenu(fileName = "Reward_Object", menuName = "Reward")]
public class Reward : ScriptableObject
{
    [SerializeField] private string rewardName;
    [SerializeField] [TextArea] private string rewardDescription;

    // recompensas posibles que el jugador conseguira   TOTALMENTE INVENTADOS DE MOMENTO
    [Header("Recursos")]
    [Range(0,100)] [SerializeField] private int gold;
    [Range(0,100)] [SerializeField] private int experience;



}
