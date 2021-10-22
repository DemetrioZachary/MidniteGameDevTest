using UnityEngine;

[CreateAssetMenu(menuName = "GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] LevelPack levelPack;
    [SerializeField] LevelGenerator generator;
    [SerializeField] WinCondition winCondition;
    [SerializeField] SOAnimation soAnimation;

    public LevelPack LevelPack => levelPack;
    public LevelGenerator Generator => generator;
    public WinCondition WinCondition => winCondition;
    public SOAnimation SOAnimation => soAnimation;
}
