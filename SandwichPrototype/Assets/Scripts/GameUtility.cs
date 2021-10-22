using UnityEngine;

public static class GameUtility
{
    public const int movesQueueLenght = 3;

    public const float animationSpeedUpFactor = 0.7f;

    public const float pieceHeight = 0.1f;

    public static int gridMask = LayerMask.GetMask("Grid");

    public static Vector3 pieceScale = Vector3.one * 0.9f;

    public const string levelAssetFolderPath = "Assets/Data/Levels/";
}
