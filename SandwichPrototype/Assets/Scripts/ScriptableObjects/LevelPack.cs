using UnityEngine;

[CreateAssetMenu(menuName = "Sandwich/LevelPack")]
public class LevelPack : ScriptableObject
{
    [SerializeField] LevelData[] levels;

    int currentLevel;

    void OnEnable()
    {
        currentLevel = 0;
    }

    public void ResetProgress()
    {
        currentLevel = 0;
    }

    public bool GetNextLevel(out LevelData level)
    {
        if (currentLevel < levels.Length)
        {
            level = levels[currentLevel++];
            return true;
        }
        currentLevel = 0;
        level = null;
        return false;
    }
}
