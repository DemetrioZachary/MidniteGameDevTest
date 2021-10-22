using System.Collections.Generic;
using UnityEngine;

public struct Element
{
    public ElementData data;
    public Transform transform;
}

public class GridCell
{
    // Pieces are arranged from bottom to top
    List<Element> pieces;
    Vector3 position;

    public GridCell(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 Position => position;
    public bool IsEmpty => Depth == 0;
    public int Depth => pieces == null ? 0 : pieces.Count;
    public List<Element> Pieces => pieces;

    public void AddToCell(Element piece)
    {
        AddToCell(new List<Element> { piece });
    }

    public void AddToCell(List<Element> newPieces)
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
