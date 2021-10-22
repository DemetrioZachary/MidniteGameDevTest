using UnityEngine;
using PrsdTech.SO.Events;

public class MoveEventArgs : SOEventArgs
{
    public GridCell endCell;
}

public partial class GameGrid : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] GameData gameData;
    [SerializeField] ElementPooler elementPooler;
    [Header("Events")]
    [SerializeField] SOEventListener inputBegunListener;
    [SerializeField] SOEventListener inputDragListener;
    [SerializeField] SOEventListener inputEndListener;
    [SerializeField] SOEvent winEvent;
    [SerializeField] SOEvent loseEvent;
    [SerializeField] SOEvent packCompletedEvent;
    [SerializeField] SOEvent moveEvent;

    LevelData level;
    int size;

    Vector3 startDragPosition;
    bool dragValid;
    GridCell startCell;

    GridCell[] cells;

    public GameData GameData => gameData;

    void OnEnable()
    {
        LoadNextLevel();

        inputBegunListener.Enable(OnInputBegan);
        inputDragListener.Enable(OnInputDrag);
        inputEndListener.Enable(OnInputEnd);
    }

    void OnDisable()
    {
        inputBegunListener.Disable();
        inputDragListener.Disable();
        inputEndListener.Disable();
    }

    public void LoadGameData(GameData data)
    {
        gameData = data;
        LoadNextLevel();
    }

    public void GenerateRandomLevel()
    {
        level = gameData.Generator.Generate();
        InitializeGrid();
    }

    public void LoadNextLevel()
    {
        if (!gameData.LevelPack.GetNextLevel(out level))
        {
            packCompletedEvent.Invoke();
            return;
        }
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        elementPooler.RecycleAll();

        size = level.size;
        transform.position = new Vector3(size * 0.5f, 0f, size * 0.5f);
        transform.localScale = Vector3.one * 0.1f * size;

        cells = new GridCell[size * size];
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 position = new Vector3(i % size + 0.5f, 0f, i / size + 0.5f);
            var cell = new GridCell(position);
            var pieceData = level.pieces[i];
            if (pieceData)
            {
                Transform t = elementPooler.Get(pieceData, position, Quaternion.identity).transform;
                cell.AddToCell(new Element { data = pieceData, transform = t });
            }
            cells[i] = cell;
        }
    }

    bool GetCellFromPosition(Vector3 position, out GridCell cell)
    {
        int x = (int)position.x;
        int z = (int)position.z;
        if (x >= 0 && x < size && z >= 0 && z < size)
        {
            cell = cells[x + size * z];
            return true;
        }
        cell = null;
        return false;
    }

    bool GetCellFromInput(Vector3 position, out GridCell cell)
    {
        var ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, GameUtility.gridMask))
        {
            Vector3 p = hitInfo.point;
            cell = cells[(int)p.x + size * (int)p.z];
            return true;
        }
        cell = null;
        return false;
    }

    void OnInputBegan(SOEventArgs args)
    {
        if (!(args is InputEventArgs)) { return; }

        if (GetCellFromInput((args as InputEventArgs).position, out GridCell cell))
        {
            if (!cell.IsEmpty)
            {
                startDragPosition = (args as InputEventArgs).position;
                startCell = cell;
                dragValid = true;
            }
        }
    }

    void OnInputDrag(SOEventArgs args)
    {
        if (!dragValid || !(args is InputEventArgs)) { return; }

        if (GetCellFromInput((args as InputEventArgs).position, out GridCell cell))
        {
            if (cell != startCell)
            {
                if (!cell.IsEmpty)
                {
                    AddAnimationToQueue(startCell, cell);
                }
                dragValid = false;
            }
        }
        else
        {
            dragValid = false;
        }
    }

    void OnInputEnd(SOEventArgs args)
    {
        if (!dragValid || !(args is InputEventArgs)) { return; }

        Vector3 diff = (args as InputEventArgs).position - startDragPosition;
        if (diff.sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 direction;
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                direction = diff.x > 0f ? Vector3.right : Vector3.left;
            }
            else
            {
                direction = diff.y > 0f ? Vector3.forward : Vector3.back;
            }

            if (GetCellFromPosition(startCell.Position + direction, out GridCell cell) && !cell.IsEmpty)
            {
                AddAnimationToQueue(startCell, cell);
            }
        }
        dragValid = false;
    }

    void CheckCells()
    {
        int count = 0;
        int index = -1;
        bool allCellsIsolated = true;

        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].IsEmpty)
            {
                count++;
                index = i;

                bool missingRightCell = (i + 1) % size == 0 || cells[i + 1].IsEmpty;
                bool missingUpCell = i >= size * (size - 1) || cells[i + size].IsEmpty;
                bool isolated = missingRightCell && missingUpCell;
                allCellsIsolated &= isolated;
            }
        }

        if (allCellsIsolated) // WinCondition can be extended to a wider control level
        {
            if (count == 1)
            {
                if (gameData.WinCondition.Check(cells[index].Elements))
                {
                    winEvent.Invoke();
                }
                else
                {
                    loseEvent.Invoke();
                }
            }
            else
            {
                loseEvent.Invoke();
            }
        }
    }
}
