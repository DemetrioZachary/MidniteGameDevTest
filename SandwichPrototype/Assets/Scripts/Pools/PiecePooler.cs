using UnityEngine;
using PrsdTech.Pools.Core;

[CreateAssetMenu(menuName = "PrsdTech/ScriptableObjects/Pooler", order = 380)]
public class PiecePooler : ScriptableObject
{
    [SerializeField] GameObject prefab = default;
    [SerializeField] int maxInstances = 100;

    DynamicPrefabPool pool;

    public GameObject Get(PieceData data, Vector3 position, Quaternion rotation)
    {
        if (pool == null)
        {
            CreatePool();
            if (!prefab)
            {
                Debug.LogError(name + ": Unable to get Poolable instance! Missing prefab data.");
                return null;
            }
        }

        GameObject o = pool.Get(position, rotation, out bool _);
        o.GetComponent<MeshFilter>().mesh = data.mesh;
        o.GetComponent<MeshRenderer>().material = data.material;
        o.transform.localScale = GameUtility.pieceScale;
        return o;
    }

    public void Recycle(GameObject o)
    {
        if (pool != null)
        {
            pool.Recycle(o);
        }
        else
        {
            Debug.Log(name + ": Unable to recycle GameObject! Pool doesn't exist.");
        }
    }

    public void RecycleAll()
    {
        if (pool != null)
        {
            pool.RecycleAll();
        }
    }

    void CreatePool()
    {
        pool = new DynamicPrefabPool(prefab, null, maxInstances);
    }

    public void ClearPool()
    {
        if (pool != null)
        {
            pool.ClearAsync();
            pool = null;
        }
    }
}
