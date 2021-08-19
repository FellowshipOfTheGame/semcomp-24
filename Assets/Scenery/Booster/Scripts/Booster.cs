using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float boost;
    
    [SerializeField]
    private float duration = 3f;

    [SerializeField]
    private NitrousSystem.ActivateMode activateMode;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            // Activate nitrous using the specified mode
            other.GetComponent<NitrousSystem>().Activate(boost, duration, NitrousSystem.ActivateMode.Reset);
        }
    }
}