using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public Piece selectedPiece;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) SelectPiece();
        if (Input.GetMouseButtonDown(1)) MovePiece();
    }

    private void MovePiece()
    {
        if (selectedPiece == null) return;

        Cell selectedCell = GetFromRay<Cell>();

        if (selectedCell == null) return;

        for (int i = 0; i < selectedPiece.MovePositions.Length; i++)
        {
            if (selectedCell.position == selectedPiece.MovePositions[i])
            {
                selectedPiece.Move(selectedPiece.MovePositions[i]);
            }
        }
    }

    public void SelectPiece()
    {
        Deselect();

        selectedPiece = GetFromRay<Piece>();

        if (selectedPiece) selectedPiece.Selected(true);
    }

    private void Deselect()
    {
        if (selectedPiece) selectedPiece.Selected(false);
        selectedPiece = null;
    }

    public static T GetFromRay<T>()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<T>() != null)
            {
                return hit.transform.GetComponent<T>();
            }
        }

        return default;
    }
}
