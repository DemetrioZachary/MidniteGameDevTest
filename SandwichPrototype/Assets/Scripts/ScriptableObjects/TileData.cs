using UnityEngine;

public enum TileValue
{
    _2, _4, _8, _16, _32, _64, _128, _256, _512, _1024, _2048
}

[CreateAssetMenu(menuName = "2048/Tile")]
public class TileData : ElementData
{
    [SerializeField] TileValue value;

    public TileValue Value => value;
}
