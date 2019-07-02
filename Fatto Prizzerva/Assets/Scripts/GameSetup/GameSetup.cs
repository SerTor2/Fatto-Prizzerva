using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetupData", menuName = "GameSetupData")]
public class GameSetup : ScriptableObject
{
    [TextArea] [SerializeField] private string description;

    [Header("Game Tags")]
    public string player_Tag;
    public string enemy_Tag;


}
