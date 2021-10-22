using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/Win Condition Sandwich")]
public class WinConditionBreadTopBottom : WinCondition
{
    public override bool Check(List<Element> pieces)
    {
        if (!(pieces[0].data is PieceData))
        {
            Debug.LogError(name + ": Assigned wrong WinCondition: Elements must be sandwich pieces.");
            return false;
        }
        return ((PieceData)pieces[0].data).Type == PieceType.Bread && ((PieceData)pieces[pieces.Count - 1].data).Type == PieceType.Bread;
    }
}
