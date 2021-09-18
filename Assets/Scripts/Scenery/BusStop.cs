using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField]
    public int healAmount;
    
    public FMODUnity.StudioEventEmitter eventEmitter;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eventEmitter.Play();
            other.gameObject.GetComponent<HealthSystem>().Heal(healAmount);
        }
    }
}
