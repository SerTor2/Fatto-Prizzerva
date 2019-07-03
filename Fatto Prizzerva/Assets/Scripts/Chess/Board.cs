using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : MonoBehaviour
{
    public static Board instance;

    public int size = 8;

    public Cell[,] board;

    public BoardSettings boardSettings;

    public Vector2 Offset;

    public Cell NormalCell;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        SetUp();
        CreateBoard();
    }

    public void SetUp()
    {
        board = new Cell[size, size];
    }

    public void CreateBoard()
    {
        if (boardSettings) GetBoardSettings();

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (board[x, y] != null) continue;

                board[x, y] = Instantiate(NormalCell.gameObject).GetComponent<Cell>();
                SetCellPosition(board[x, y], x, y);
            }
        }
    }

    public void GetBoardSettings()
    {
        foreach (CellSetting cell in boardSettings.cells)
        {
            if (!ValidIndex(cell.position.x, cell.position.y)) return;

            Cell currentCell = Instantiate(cell.cellPrefab.gameObject).GetComponent<Cell>();

            SetCellPosition(currentCell, cell.position.x, cell.position.y);
            currentCell.position = cell.position;
            board[cell.position.x, cell.position.y] = currentCell;
        }
    }

    public bool ValidIndex(int x, int y)
    {
        return ((x < size && y < size) && (x >= 0 && y >= 0));
    }

    public Cell GetCell(int x, int y)
    {
        if (ValidIndex(x, y)) return board[x, y];

        return null;
    }

    public void SetCellPosition(Cell cell, int x, int y)
    {
        Vector2 positionAfterOffset = new Vector2(x + Offset.x, y + Offset.y);
        cell.SetPosition(positionAfterOffset);
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (board == null) return;
                Gizmos.color = board[x, y].type == Cell.CellType.Normal ? Color.red : Color.blue;

                if (Gizmos.color == Color.red)
                    Gizmos.DrawWireCube(new Vector3(x, 0, y), Vector3.one);
                else
                    Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
            }
        }
    }
}
