using UnityEngine;

public class ElementData : ScriptableObject
{
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    public Mesh Mesh => mesh;
    public Material Material => material;
}
