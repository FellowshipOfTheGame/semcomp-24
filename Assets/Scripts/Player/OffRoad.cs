using SubiNoOnibus;
using UnityEngine;

public class OffRoad : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float speedDecreaseFactor = 0.5f;
    
    private VehicleController vehicleController;
    private NitrousSystem nitrous;
    
    private float maximumSpeed;
    private bool nitrousWasActive;

    public static bool IsPlayerOffRoad { get; private set; }

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
            
            if (maximumSpeed == 0 || nitrousRanOut)
            {
                maximumSpeed = vehicleController.GetActualMaximumSpeed();
                // Debug.Log("Resetting off-road clamped speed);

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
                vehicleController.forwardForce = Mathf.Clamp(vehicleController.forwardForce, 0, maximumSpeed * (1 - speedDecreaseFactor));
                // Debug.Log($"current: {vehicleController.forwardForce}, default: {maximumSpeed}, min: {maximumSpeed * 0.2f}, max: {maximumSpeed * (1 - speedDecreaseFactor)}");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (TimeTravel.InThePast)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("OffRoad") && !IsPlayerOffRoad)
        {
            IsPlayerOffRoad = true;
            nitrousWasActive = nitrous.IsActive();

            if (nitrousWasActive)
            {
                maximumSpeed = vehicleController.forwardForce;
                // maximumSpeed = vehicleController.GetActualMaximumSpeed() + vehicleController.forwardForce;
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
            maximumSpeed = 0;
            nitrousWasActive = false;

            if (nitrous.IsActive())
            {
                nitrous.Continue();
            }
            // else
            // {
            //     vehicleController.ResetGroundDrag();
            // }
        }
    }
}