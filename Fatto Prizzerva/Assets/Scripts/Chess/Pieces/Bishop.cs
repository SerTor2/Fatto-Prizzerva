using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private Board board;

    private void Start()
    {
        board = Board.instance;

        transform.position = new Vector3(UnityEngine.Random.Range(0, Board.instance.size),
                                         1,
                                         UnityEngine.Random.Range(0, Board.instance.size));
        GetBoardPosition();
    }

    public override void Move(Vector2 position)
    {
        boardPosition = new Vector2Int((int)position.x, (int)position.y);

        transform.position = new Vector3(boardPosition.x, 1, boardPosition.y);

        GetPossibleMoves();
        ShowPossibleMoves(true);
    }

    public override int CalculateCost(Vector2 position)
    {
        return (int)Math.Abs(position.x - boardPosition.x);
    }

    public override void Die()
    {

    }

    public override void GetPossibleMoves()
    {
        MovePositions.Clear();
        board.ClearValidPositions();

        MovePositions.AddRange(FindPossibleMoves(1, 1));
        MovePositions.AddRange(FindPossibleMoves(1, -1));
        MovePositions.AddRange(FindPossibleMoves(-1, 1));
        MovePositions.AddRange(FindPossibleMoves(-1, -1));
    }

    public override void ShowPossibleMoves(bool show)
    {
        for (int i = 0; i < MovePositions.Count; i++)
        {
            board.validPosition[i].transform.position = new Vector3(MovePositions[i].x, .6f, MovePositions[i].y);
            board.validPosition[i].enabled = show;
        }
    }

    private List<Vector2> FindPossibleMoves(int x, int y)
    {
        bool IsValid = true;

        List<Vector2> positions = new List<Vector2>();

        Vector2Int position = boardPosition;

        while (IsValid)
        {
            position += new Vector2Int(x, y);

            IsValid = board.ValidIndex(position.x, position.y);

            if (!IsValid) break;

            if (CalculateCost(position) <= ChessPlayer.instance.movements)
                positions.Add(position);
        }

        return positions;
    }
}
