using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace PrsdTech.Pools.Core
{

    public class DynamicPrefabPool
    {
        int maxPoolObjects;
        GameObject prefab;
        List<GameObject> actives;
        Queue<GameObject> inactives;
        Transform parent;

        public DynamicPrefabPool(GameObject prefab, Transform parent, int maxPoolObjects)
        {
            this.parent = parent;
            this.maxPoolObjects = maxPoolObjects;
            this.prefab = prefab;

            actives = new List<GameObject>();
            inactives = new Queue<GameObject>();
        }

        public GameObject Get(Vector3 p, Quaternion r, out bool newObjectCreated)
        {
            GameObject o;
            newObjectCreated = false;

            if (inactives.Count > 0)
            {
                o = inactives.Dequeue();
                o.SetActive(true);
            }
            else if (actives.Count < maxPoolObjects)
            {
                if (!prefab)
                {
                    Debug.LogError(parent.name + ": Prefab not found! Make sure a prefab asset is assigned to the pool.");
                }
                o = Object.Instantiate(prefab, parent);
                newObjectCreated = true;
            }
            else
            {
                o = actives[0];
                actives.RemoveAt(0);
            }

            o.transform.localPosition = p;
            o.transform.localRotation = r;

            actives.Add(o);
            return o;
        }

        public void Recycle(GameObject o)
        {
            if (actives.Contains(o))
            {
                actives.Remove(o);
                o.SetActive(false);
                inactives.Enqueue(o);
                return;
            }
            Debug.LogError("Pool doesn't contain '" + o.name + "'!");
        }

        public void RecycleAll()
        {
            for (int i = actives.Count - 1; i >= 0; i--)
            {
                Recycle(actives[i]);
            }
        }

        public async void ClearAsync()
        {
            for (int i = 0; i < actives.Count; i++)
            {
                await new Task<bool>(() => { Object.Destroy(actives[i]); return true; });
            }

            while (inactives.Count > 0)
            {
                await new Task<bool>(() => { Object.Destroy(inactives.Dequeue()); return true; });
            }

            Object.Destroy(parent.gameObject);

            actives.Clear();
            inactives.Clear();
            parent = null;
            prefab = null;
            maxPoolObjects = 0;
        }
    }

}
