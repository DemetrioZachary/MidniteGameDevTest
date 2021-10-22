using System.Collections.Generic;
using UnityEngine;

public struct Piece
{
    public PieceData data;
    public Transform transform;
}

public class GridCell
{
    // Pieces are arranged from bottom to top
    List<Piece> pieces;
    Vector3 position;

    public GridCell(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 Position => position;
    public bool IsEmpty => Depth == 0;
    public int Depth => pieces == null ? 0 : pieces.Count;
    public List<Piece> Pieces => pieces;

    public void AddToCell(Piece piece)
    {
        AddToCell(new List<Piece> { piece });
    }

    public void AddToCell(List<Piece> newPieces)
    {
        if (pieces == null)
        {
            pieces = newPieces;
        }
        else
        {
            pieces.AddRange(newPieces);
        }
    }

    public void MoveToCell(GridCell otherCell)
    {
        pieces.Reverse();
        otherCell.AddToCell(pieces);
        pieces = null;
    }
}
