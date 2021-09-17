using UnityEngine;

public class HoleInRoad : MonoBehaviour
{
    [SerializeField]
    private int slowAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Hole Collide");
            VehicleController vehicleController = other.gameObject.GetComponent<VehicleController>();
            vehicleController.forwardForce -= Mathf.Clamp(vehicleController.forwardForce, 0, slowAmount);
        }
    }
}
