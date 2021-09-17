using UnityEngine;

public class OffRoad : MonoBehaviour
{
    [SerializeField] [Range(0f, 1.5f)]
    private float speedDecreaseFactor = 1f;
    
    private VehicleController vehicleController;
    private NitrousSystem nitrous;
    
    private float groundDragDefault;
    private bool nitrousWasActive;

    public bool IsPlayerOffRoad { get; private set; }

    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
        nitrous = GetComponent<NitrousSystem>();
    }

    void Update()
    {
        if (IsPlayerOffRoad)
        {
            bool nitrousRanOut = (nitrousWasActive && !nitrous.IsActive());
            
            if (groundDragDefault == 0 || nitrousRanOut)
            {
                groundDragDefault = vehicleController.groundDrag;
                // Debug.Log("Resetting off-road drag");

                if (nitrousRanOut)
                {
                    nitrousWasActive = false;
                }
            }

            bool nitrousActivatedOffRoad = (!nitrousWasActive && nitrous.IsActive());

            if (nitrousActivatedOffRoad)
            {
                // Debug.Log("You activated the nitrous off-road");
            }
            else
            {
                vehicleController.groundDrag = groundDragDefault * (1 + speedDecreaseFactor);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("OffRoad") && !IsPlayerOffRoad)
        {
            IsPlayerOffRoad = true;
            nitrousWasActive = nitrous.IsActive();

            if (nitrousWasActive)
            {
                groundDragDefault = 0;
                // Debug.Log("You entered off-road with the nitrous active");
            }
            else
            {
                // Debug.Log("You are now off-road");
            }
        }
        
        if (other.gameObject.CompareTag("Road"))
        {
            // Debug.Log("You are now on the road");
            IsPlayerOffRoad = false;
            groundDragDefault = 0;
            nitrousWasActive = false;

            if (nitrous.IsActive())
            {
                nitrous.Continue();
            }
            else
            {
                vehicleController.ResetGroundDrag();
            }
        }
    }
}