using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/GridData")]
public class GridData : ScriptableObject
{
    public int size = 4;
    public PieceData[] pieces;

    public PieceData GetPiece(Vector2Int coords)
    {
        return pieces[coords.x + size * coords.y];
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (pieces.Length != size * size)
        {
            Array.Resize(ref pieces, size * size);
        }
    }
#endif
}
