using UnityEngine;

public static class GlobalData
{
    public const float pieceHeight = 0.1f;

    public static int gridMask = LayerMask.GetMask("Grid");

    public static Vector3 pieceScale = Vector3.one * 0.9f;
}
