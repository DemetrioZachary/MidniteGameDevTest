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
    List<Element> elements;
    Vector3 position;

    public GridCell(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 Position => position;
    public bool IsEmpty => Depth == 0;
    public int Depth => elements == null ? 0 : elements.Count;
    public List<Element> Elements => elements;

    public void AddToCell(Element piece)
    {
        AddToCell(new List<Element> { piece });
    }

    public void AddToCell(List<Element> newPieces)
    {
        if (elements == null)
        {
            elements = newPieces;
        }
        else
        {
            elements.AddRange(newPieces);
        }
    }

    public void MoveToCell(GridCell otherCell)
    {
        elements.Reverse();
        otherCell.AddToCell(elements);
        elements = null;
    }
}
