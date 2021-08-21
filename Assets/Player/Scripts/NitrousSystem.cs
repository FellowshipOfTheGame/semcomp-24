using UnityEngine;
using TMPro;

public class NitrousSystem : MonoBehaviour
{
    private bool active;
    private float boost;
    private float duration;
    private float timer;
    
    [SerializeField]
    private TextMeshProUGUI effectTimeText;
    
    private float effectTimeCount;
    
    /// <summary>
    /// Use ActivateMode to specify what happens when nitrous is already active
    /// </summary>
    public enum ActivateMode
    {
        /// <summary>
        /// Add <paramref name="duration"/> and a fraction of the <paramref name="boost"/> power.
        /// </summary>
        Add,
        
        /// <summary>
        /// Add <paramref name="boost"/> power and reset <paramref name="duration"/>. Only a fraction of the boost power is added
        /// </summary>
        AddBoost,
        
        /// <summary>
        /// Add <paramref name="duration"/> and reset <paramref name="boost"/>  power
        /// </summary>
        AddDuration,
        
        /// <summary>
        /// Reset both <paramref name="duration"/> and <paramref name="boost"/> power 
        /// </summary>
        Reset,
        
        /// <summary>
        /// Ignore both <paramref name="duration"/> and <paramref name="boost"/> power values passed and use currently active nitrous values instead  
        /// </summary>
        Continue
    };
    
    [SerializeField] [Tooltip("Maximum number of boosts the player can accumulate. Applies only to additive activate modes")]
    private int boostChainMax = 3;
    
    [SerializeField]
    [Tooltip("Every boost after the first will have its power controlled by this number." +
             " Since this value will be multiplied by the boost, you probably want it to be smaller than 0.1")]
    private float boostChainControl = 0.05f;

    private int boostChainCount;
    private string boostChainCountText;
    
    private VehicleController vehicleController;
    
    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
    }

    void Update()
    {
        if (active)
        {
            Timer();
        }

        if (effectTimeCount - Time.deltaTime >= 0f)
        {
            boostChainCountText = boostChainCount > 1 ? "(" + boostChainCount + ") " : null;
            effectTimeCount -= Time.deltaTime;
            effectTimeText.text = boostChainCountText + System.TimeSpan.FromSeconds(effectTimeCount).ToString("ss\\:ff");
        }
    }
    
     /// <summary>
     /// Activate nitrous. Use ActivateMode to specify what happens when nitrous is already active
     /// </summary>
     /// 
     /// <remarks>
     ///
     /// There's only one timer and one active boost, which means that if you activate the nitrous in a mode that
     /// accumulates boost power, there's no way, for instance, to have a boost chain in which the effect of a more
     /// powerful boost wears off earlier than a less powerful one, in a way that the player loses boost power while
     /// having the nitrous remain active. This also means that the boost chain count will only be reset when the timer
     /// is also reset and the nitrous, deactivated 
     /// 
     /// </remarks>
     ///
     /// <param name="boost">
     /// Boost power <c> [drag = drag * (1 - boost)] </c>
     /// </param>
     ///
     /// <param name="duration">
     /// Time in seconds that the nitrous remains active
     /// </param>
     ///
     /// <param name="mode">
     /// Specify what happens when nitrous is already active
     /// </param>
     /// 
     /// <seealso cref="ActivateMode"/>
     /// 
     public void Activate(float boost, float duration, ActivateMode mode = ActivateMode.Reset)
    {
        switch (mode)
        {
            case ActivateMode.Add:
            {
                // Checks if it's the first of a chain 
                if (active)
                {
                    if (boostChainCount < boostChainMax)
                    {
                        this.boost += boost * boostChainControl;
                        this.duration += duration;
                        effectTimeCount += duration;
                        boostChainCount++;
                        // Debug.Log("Nitrous! Add (boost: " + boost * boostChainControl + ", duration: " + duration + ")");
                    }
                    else
                    {
                        // Debug.Log("Nitrous! Add (failed:  you reached the maximum number of boosts)");
                    }
                }
                else
                {
                    this.boost = boost;
                    this.duration = duration;
                    effectTimeCount = duration;
                    boostChainCount++;
                    // Debug.Log("Nitrous! Add (boost: " + boost + ", duration: " + duration + ")");
                }
                break;
            }
            case ActivateMode.AddBoost:
            {
                if (active)
                {
                    if (boostChainCount < boostChainMax)
                    {
                        this.boost += boost * boostChainControl;
                        boostChainCount++;
                        // Debug.Log("Nitrous! AddBoost (boost: " + boost * boostChainControl + ")");
                    }
                    else
                    {
                        // Debug.Log("Nitrous! AddBoost (failed:  you reached the maximum number of boosts)");
                    }
                }
                else
                {
                    this.boost = boost;
                    boostChainCount++;
                    // Debug.Log("Nitrous! AddBoost (boost: " + boost + ")");
                }
                
                this.duration = duration;
                effectTimeCount = duration;
                timer = 0;
                break;
            }
            case ActivateMode.AddDuration:
            {
                this.boost = boost;
                
                if (active)
                {
                    if (boostChainCount < boostChainMax)
                    {
                        this.duration += duration;
                        effectTimeCount += duration;
                        boostChainCount++;
                        // Debug.Log("Nitrous! AddDuration (duration: " + duration + ")");
                    }
                    else
                    {
                        // Debug.Log("Nitrous! AddDuration (failed:  you reached the maximum number of boosts)");
                    }
                }
                else
                {
                    this.duration = duration;
                    effectTimeCount = duration;
                    boostChainCount++;
                    // Debug.Log("Nitrous! AddDuration (duration: " + duration + ")");
                }
                break;
            }
            case ActivateMode.Reset:
            {
                this.boost = boost;
                this.duration = duration;
                effectTimeCount = duration;
                timer = 0;
                boostChainCount = 1;
                // Debug.Log("Nitrous! Reset");
                break;
            }
            case ActivateMode.Continue:
            {
                if (!active)
                {
                    // Debug.Log("Nitrous! Continue");
                    // Since nitrous is not active yet, we need to set the parameters, and reset mode does exactly that
                    goto case ActivateMode.Reset;
                }
                break;
            }
        }
        
        active = true;
        
        // If player is off-road, use the actual ground drag (modified by OffRoad script) instead of the default one
        float groundDragDefault = OffRoad.IsPlayerOffRoad ? vehicleController.groundDrag : vehicleController.groundDragDefault;
        vehicleController.groundDrag = groundDragDefault * (1 - this.boost);
    }

     /// <summary>
     /// Deactivate nitrous. Boost chain count will be reset
     /// </summary>
     /// 
     public void Deactivate()
    {
        active = false;
        vehicleController.ResetGroundDrag();
        timer = 0;
        boostChainCount = 0;

        // If the player is off the road, set the flag that indicates they've ran out of nitrous
        if (OffRoad.IsPlayerOffRoad)
        {
            OffRoad.NitrousRanOut = true;
            // Debug.Log("You ran out of nitrous while you were off the road");
        }
    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            // Debug.Log("You ran out of nitrous");
            Deactivate();
        }
    }

    /// <summary>
    /// If nitrous is active, continue to use it. Useful when you need to change the vehicle rigidbody drag without
    /// affecting the active nitrous
    /// </summary>
    ///
    /// <remarks>
    ///
    /// Note that this method doesn't ask for any parameters, which means that if nitrous is not active, nothing will
    /// happen. If in that case you want it to turn active instead, use <see cref="Activate"/>
    ///  
    /// </remarks>
    ///
    /// <seealso cref="ActivateMode.Continue"/>
    /// 
    public void Continue()
    {
        if (active)
        {
            Activate(0, 0, ActivateMode.Continue);
        }
    }
    
    /// <summary> Is the nitrous active? </summary>
    /// <returns> Nitrous current active state  </returns>
    /// 
    public bool IsActive()
    {
        return active;
    }
}
