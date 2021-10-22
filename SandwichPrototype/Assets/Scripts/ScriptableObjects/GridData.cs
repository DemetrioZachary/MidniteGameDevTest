using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/GridData")]
public class GridData : ScriptableObject
{
    public int size = 4;
    public PieceData[] pieces;
}
