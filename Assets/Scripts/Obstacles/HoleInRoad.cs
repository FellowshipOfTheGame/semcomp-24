using UnityEngine;

public class HoleInRoad : MonoBehaviour
{
    [SerializeField]
    private int slowAmount;

    [SerializeField]
    private FMODUnity.StudioEventEmitter _eventEmitter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Hole Collide");
            _eventEmitter.Play();
            VehicleController vehicleController = other.gameObject.GetComponent<VehicleController>();
            vehicleController.forwardForce -= Mathf.Clamp(vehicleController.forwardForce, 0, slowAmount);
        }
    }
}
