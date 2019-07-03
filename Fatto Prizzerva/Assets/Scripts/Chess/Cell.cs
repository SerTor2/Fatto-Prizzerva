using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour
{
    public enum CellType
    {
        Normal,
        Jumper,
        Portal,
        DestructibleWall,
        IndestructibleWall,
        Storm,
    }

    public CellType type;

    public Vector2Int position;

    public void SetPosition(Vector2 position)
    {
        this.position = new Vector2Int((int)position.x, (int)position.y);
        transform.position = new Vector3(position.x, 0, position.y);
    }
}
