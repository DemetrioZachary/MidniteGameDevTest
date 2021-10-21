using System.Collections.Generic;
using UnityEngine;

public struct Piece
{
    public PieceData data;
    public Transform transform;
}

public class GridCell
{
    public List<Piece> pieces;

    public Vector3 Position { get; set; }
    public bool IsEmpty => Depth == 0;
    public int Depth => pieces == null ? 0 : pieces.Count;

    public Transform[] GetObjects()
    {
        Transform[] objs = new Transform[pieces.Count];
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i] = pieces[i].transform;
        }
        return objs;
    }

    public void AddToCell(Piece piece)
    {
        AddToCell(new List<Piece> { piece });
    }

    public void AddToCell(List<Piece> newPieces)
    {
        int depth = Depth;
        if (pieces == null)
        {
            pieces = newPieces;
        }
        else
        {
            pieces.AddRange(newPieces);
        }

        for (int i = 0; i < newPieces.Count; i++)
        {
            //newPieces[i].transform.position = Position + Vector3.up * (depth + i) * 0.1f;
        }
    }

    public void MoveToCell(GridCell otherCell, Vector2Int dir)
    {
        pieces.Reverse();
        otherCell.AddToCell(pieces);
        pieces = null;
    }
}
