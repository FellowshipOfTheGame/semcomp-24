using UnityEngine;

public class OffRoad : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float speedDecreaseFactor;
    
    private float groundDragDefault;

    private GameObject player;
    private VehicleController playerVehicleController;
    private NitrousSystem playerNitrous;

    public static bool IsPlayerOffRoad { get; private set; }
    public static bool NitrousRanOut;

    private bool nitrousWasActive; // Was nitrous active when player entered the off-road terrain?

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (player == null)
            {
                Debug.Log("Entrou aqui");
                
                player = other.gameObject;
                playerVehicleController = player.GetComponent<VehicleController>();
                playerNitrous = player.GetComponent<NitrousSystem>();
            }

            IsPlayerOffRoad = true;
            nitrousWasActive = playerNitrous.IsActive();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            // Gets player drag and checks if the player ran out of nitrous off-road (if they did, update drag)
            if (groundDragDefault == 0 || NitrousRanOut)
            {
                groundDragDefault = playerVehicleController.groundDrag;
                NitrousRanOut = false;
                // Debug.Log("Resetting off-road drag");
            }
            
            // If nitrous is activated off-road
            if (!nitrousWasActive && playerNitrous.IsActive())
            {
                Debug.Log("You activated the nitrous off-road");
            }
            else
            {
                // Increasing the drag instead of applying a force gives a smoother acceleration boost
                playerVehicleController.groundDrag = groundDragDefault * (1 + speedDecreaseFactor);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // If nitrous is still active when the player is getting back on the road, continue to use it
            if (playerNitrous.IsActive())
            {
                playerNitrous.Continue();
            }
            else
            {
                playerVehicleController.ResetGroundDrag();
            }
            
            // Reset flags
            IsPlayerOffRoad = false;
            NitrousRanOut = false;
            groundDragDefault = 0;
        }
    }
}
