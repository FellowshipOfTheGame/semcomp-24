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
            other.gameObject.GetComponent<VehicleController>().SlowFowardForce(slowAmount);
        }
    }
}
