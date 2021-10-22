using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2048/Win Condition 2048")]
public class WinCondition2048 : WinCondition
{
    public override bool Check(List<Element> pieces)
    {
        if (!(pieces[0].data is TileData))
        {
            Debug.LogError(name + ": Assigned wrong WinCondition: Elements must be 2048 Tiles.");
            return false;
        }
        return pieces.Count == 1;
    }
}
