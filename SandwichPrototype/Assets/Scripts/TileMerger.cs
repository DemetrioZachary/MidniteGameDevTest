using UnityEngine;
using PrsdTech.SO.Events;

public class TileMerger : MonoBehaviour
{
    [SerializeField] SOEventListener moveListener;
    [SerializeField] ElementPooler pooler;

    [SerializeField] TileData[] tileReferences;

    void OnEnable()
    {
        moveListener.Enable(Merge);
    }

    void OnDisable()
    {
        moveListener.Disable();
    }

    void Merge(SOEventArgs args)
    {
        if (!(args is MoveEventArgs)) { return; }

        var cell = (args as MoveEventArgs).endCell;
        var elements = cell.Elements;
        if (elements == null || !(elements[0].data is TileData)) { return; }

        bool modified = true;
        while (modified && elements.Count > 1)
        {
            modified = false;
            for (int i = 0; i < elements.Count - 1; i++)
            {
                if (((TileData)elements[i].data).Value == ((TileData)elements[i + 1].data).Value)
                {
                    int value = (int)((TileData)elements[i].data).Value + 1;
                    pooler.Recycle(elements[i].transform.gameObject);
                    pooler.Recycle(elements[i + 1].transform.gameObject);
                    elements[i] = new Element() {
                        data = tileReferences[value],
                        transform = pooler.Get(tileReferences[value], Vector3.zero, Quaternion.identity).transform
                    };
                    elements.RemoveAt(i + 1);
                    modified = true;
                }
            }
        }

        for (int i = 0; i < elements.Count; i++)
        {
            if (Vector3.Dot(elements[i].transform.up, Vector3.up) > 0f)
            {
                elements[i].transform.position = cell.Position + Vector3.up * i * GameUtility.elementHeight;
            }
            else
            {
                elements[i].transform.position = cell.Position + Vector3.up * (i + 1) * GameUtility.elementHeight;
            }
        }
    }
}
