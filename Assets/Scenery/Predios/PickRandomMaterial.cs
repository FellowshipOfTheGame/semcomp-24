using UnityEngine;

public class PickRandomMaterial : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private Renderer _renderer;

    void Start()
    {
        if (!TryGetComponent(out _renderer))
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        if (_renderer != null)
        {
            _renderer.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
