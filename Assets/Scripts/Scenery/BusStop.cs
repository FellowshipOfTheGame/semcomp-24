using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField]
    public int healAmount;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthSystem>().Heal(healAmount);
        }
    }
}
