using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public int healthPoints;

    public bool selected;

    public Vector2Int boardPosition;
    public Vector2[] MovePositions;

    public void Selected(bool selected)
    {
        GetComponent<MeshRenderer>().material.color = selected ? Color.red : Color.white;

        this.selected = selected;
        ShowPossibleMoves(selected);
    }

    public void GetBoardPosition()
    {
        boardPosition = Board.instance.GetCell((int)transform.position.x, (int)transform.position.z).position;
    }

    public abstract void Move(Vector2 position);
    public abstract void ShowPossibleMoves(bool show);
    public abstract void Die();

}
