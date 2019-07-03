using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private void Start()
    {
        transform.position = new Vector3(Random.Range(0, Board.instance.size), 1, Random.Range(0, Board.instance.size));
        GetBoardPosition();
    }

    public override void Move(Vector2 position)
    {
        boardPosition = new Vector2Int((int)position.x, (int)position.y);

        transform.position = new Vector3(boardPosition.x, 1, boardPosition.y);

        ShowPossibleMoves(true);
    }

    public override void ShowPossibleMoves(bool show)
    {
        MovePositions = new Vector2[4];

        MovePositions[0] = new Vector2(boardPosition.x + 1, boardPosition.y + 1);
        MovePositions[1] = new Vector2(boardPosition.x + 1, boardPosition.y - 1);
        MovePositions[2] = new Vector2(boardPosition.x - 1, boardPosition.y + 1);
        MovePositions[3] = new Vector2(boardPosition.x - 1, boardPosition.y - 1);
    }

    public override void Die()
    {

    }

    private void OnDrawGizmos()
    {
        if (MovePositions == null) return;

        for (int i = 0; i < MovePositions.Length; i++)
        {
            if (!Board.instance.ValidIndex((int)MovePositions[i].x, (int)MovePositions[i].y)) return;

            Gizmos.DrawCube(new Vector3(MovePositions[i].x, 0, MovePositions[i].y), Vector3.one);
        }
    }
}
