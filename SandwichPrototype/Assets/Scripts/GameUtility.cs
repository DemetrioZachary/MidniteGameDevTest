using UnityEngine;

public static class GameUtility
{
    public const int movesQueueLenght = 2;

    public const float animationSpeedUpFactor = 0.7f;

    public const float elementHeight = 0.09f;

    public static Vector3 elementScale = new Vector3(-1f, 1f, -1f) * 0.9f;

    public static int gridMask = LayerMask.GetMask("Grid");

    public const string levelAssetFolderPath = "Assets/Data/Levels/";
}
