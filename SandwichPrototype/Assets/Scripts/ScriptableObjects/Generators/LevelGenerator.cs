using UnityEngine;

public abstract class LevelGenerator : ScriptableObject
{
    [SerializeField] protected int size = 4;

    void OnEnable()
    {
        Random.InitState(System.DateTime.Now.Second);
    }

    public abstract LevelData Generate();
}
