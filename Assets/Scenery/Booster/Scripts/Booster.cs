using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float boost;
    
    [SerializeField]
    private float duration = 3f;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            NitrousSystem playerNitrous = other.GetComponent<NitrousSystem>();
    
            if (!playerNitrous.IsActive())
            {
                playerNitrous.Activate(boost, duration);
            }
        }
    }
}
