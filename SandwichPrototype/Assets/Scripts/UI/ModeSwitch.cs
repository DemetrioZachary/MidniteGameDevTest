using UnityEngine;

public class ModeSwitch : MonoBehaviour
{
    [SerializeField] GameData sandwichData;
    [SerializeField] GameData _2048Data;
    [Space]
    [SerializeField] GameGrid grid;
    [SerializeField] TileMerger merger;

    public void Switch()
    {
        if (grid.GameData == sandwichData)
        {
            _2048Data.LevelPack.ResetProgress();
            merger.enabled = true;
            grid.LoadGameData(_2048Data);
        }
        else
        {
            sandwichData.LevelPack.ResetProgress();
            merger.enabled = false;
            grid.LoadGameData(sandwichData);
        }
    }
}
