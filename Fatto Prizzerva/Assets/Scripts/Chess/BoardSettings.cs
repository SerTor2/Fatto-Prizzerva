using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardSetting")]
public class BoardSettings : ScriptableObject
{
    public List<CellSetting> cells = new List<CellSetting>();
}

[System.Serializable]
public struct CellSetting
{
    public Cell cellPrefab;

    public Vector2Int position;
}
