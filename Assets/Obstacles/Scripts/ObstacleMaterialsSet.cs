using UnityEngine;

[CreateAssetMenu(menuName = "Obstacle/Obstacle Materials Set", fileName = "NewObstacleMaterialsSet")]
public class ObstacleMaterialsSet : ScriptableObject
{
    [SerializeField] private Material _presentMaterial;
    [SerializeField] private Material _pastMaterial;

    public Material PresentMaterial => _presentMaterial;
    public Material PastMaterial => _pastMaterial;
}
