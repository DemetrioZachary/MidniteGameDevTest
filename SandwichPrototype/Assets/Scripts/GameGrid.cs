using UnityEngine;
using PrsdTech.SO.Events;

public class GameGrid : MonoBehaviour
{
    [SerializeField] int size = 4;
    [SerializeField] SOEventListener inputBegunListener;
    [SerializeField] SOEventListener inputDragListener;

    bool dragValid;
    Vector2Int startCell;

    void Awake()
    {
        transform.position = new Vector3(size * 0.5f, 0f, size * 0.5f);
        transform.localScale = Vector3.one * 0.1f * size;
    }

    void OnEnable()
    {
        inputBegunListener.Enable(OnInputBegan);
        inputDragListener.Enable(OnInputDrag);
    }

    void OnDisable()
    {
        inputBegunListener.Disable();
        inputDragListener.Disable();
    }

    bool GetCoordsFromInput(Vector3 position, out Vector2Int coords)
    {
        coords = Vector2Int.zero;
        var ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Grid")))
        {
            Vector3 p = hitInfo.point;
            coords = new Vector2Int((int)p.x, (int)p.z);
            return true;
        }
        return false;
    }

    void OnInputBegan(SOEventArgs args)
    {
        if (GetCoordsFromInput((args as InputEventArgs).position, out Vector2Int coords))
        {
            startCell = coords;
            dragValid = true;
        }
    }

    void OnInputDrag(SOEventArgs args)
    {
        if (!dragValid) { return; }
        if (GetCoordsFromInput((args as InputEventArgs).position, out Vector2Int coords))
        {
            if (coords != startCell)
            {
                Debug.Log(startCell + " -> " + coords);
                dragValid = false;
            }
        }
        else
        {
            Debug.Log(startCell + " -> Null");
            dragValid = false;
        }
    }
}
