using UnityEngine;

public class HoleInRoad : MonoBehaviour
{
    [SerializeField]
    private int slowAmount;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Debug.Log("Hole Collide");

            other.gameObject.GetComponent<VehicleController>().SlowFowardForce(slowAmount);
        }
    }
}
