using UnityEngine;
using TMPro;

public class NitrousSystem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI effectTimeText;
    
    private float effectTimeCount;
    
    private VehicleController vehicleController;
    
    private bool active;
    private float boost;
    private float duration;
    private float timer;
    
    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
    }
    
    void Update()
    {
        if (active)
        {
            SetTimer();
        }

        if (effectTimeCount - Time.deltaTime >= 0f)
        {
            effectTimeCount -= Time.deltaTime;
            effectTimeText.text = System.TimeSpan.FromSeconds(effectTimeCount).ToString(@"s\:ff");
        }
    }
    
    public bool IsActive()
    {
        return active;
    }
    
    public void Activate(float boost, float duration)
    {
        // If nitro is already active, it was called from "Continue". Effect time counter should not be reset
        if (!active)
        {
            effectTimeCount = duration;
            // Debug.Log("Nitrous!");
        }
        
        active = true;
        vehicleController.groundDrag = vehicleController.groundDragDefault * (1 - boost);
        this.boost = boost;
        this.duration = duration;
    }

    public void Deactivate()
    {
        active = false;
        vehicleController.ResetGroundDrag();
        timer = 0;

        // If the player is off the road, set the flag that indicates they've ran out of nitrous
        if (OffRoad.IsPlayerOffRoad)
        {
            OffRoad.NitrousRanOut = true;
            // Debug.Log("You ran out of nitrous while off the road");
        }
    }

    private void SetTimer()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            // Debug.Log("You ran out of nitrous");
            Deactivate();
        }
    }

    public void Continue()
    {
        float elapsedTime = timer;
        timer = 0;
        Activate(boost, duration - elapsedTime);
    }
}
