using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Physics interactions for vehicles
*/
public class VehicleController : MonoBehaviour
{
    // (Easier way to design different types of vehicles
    [Header("General")]
    public VehicleStatPreset preset;
    
    [Header("Physics")]
    public LayerMask whatIsGround;
    public Transform[] groundDetection;
    public float groundCheckDistance = 0.5f;
    public float groundDrag = 3f;
    public float airDrag = 0.1f;
    
    [Header("Turning")]
    [Tooltip("Distance between wheels")] public float wheelBase = 2f;
    private float turningAngle; // Current turning wheel angle
    
    private float forwardForce = 80f;
    private bool grounded; // State management
    
    // Components
    private Rigidbody _rigidbody;
    private Collider[] _colliders;
    
    public float maximumSpeed { get; private set; }

    public float groundDragDefault { get; private set; }
    
    [Tooltip("The maximum angle the player should be able to turn")]
    public float turningAngleMax = 15f;
    
    [Tooltip("The maximum angle the vehicle can rotate before rotating back to player's original position")]
    public float rotateBackAngle = 60f;
    
    private PlayerInput playerInput;
    private InputAction movement;

    private Vector3 vehicleRotation;
    
    #region MonoBehaviour Messages

    protected void OnEnable()
    {
        playerInput = new PlayerInput();
        movement = playerInput.Player.Movement;
        movement.Enable();

        playerInput.Player.UseItem.performed += UseItem;
        playerInput.Player.UseItem.Enable();
    }

    protected void OnDisable()
    {
        movement.Disable();
        playerInput.Player.UseItem.performed -= UseItem;
        playerInput.Player.UseItem.Disable();
    }

    protected void Awake()
    {
        // [Vmax] = (F / (m * drag)) - (F / m * t) => [Vmax] = a * (1/drag - t)
        maximumSpeed = preset.speed * (1 / groundDrag - Time.fixedDeltaTime);
        // Debug.Log("Maximum speed: " + maximumSpeed);
    }

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();

        groundDragDefault = groundDrag;
        
        if (preset is null)
            return;
        forwardForce = preset.speed;
    }

    protected void Update()
    {
        // Checks rotation boundaries
        bool turnRight = (vehicleRotation.y <= turningAngleMax || vehicleRotation.y > 180);
        bool turnLeft = (vehicleRotation.y >= (360 - turningAngleMax) || vehicleRotation.y <= 180);
        
        if (turnLeft || turnRight)
        {
            turningAngle = movement.ReadValue<float>() * 90f;
        }
        else
        {
            turningAngle = 0f;
        }
    }

    protected void FixedUpdate()
    {
        CheckForGround();
        if (!grounded)
        {
            return;
        }
        
        _rigidbody.AddRelativeForce(Vector3.forward * forwardForce, ForceMode.Acceleration);
        // _rigidbody.AddForce(-_rigidbody.velocity, ForceMode.Acceleration); // Goes up to a certain maximum speed
        
        if (turningAngle != 0f)
        {
            float radius = wheelBase / Mathf.Sin(turningAngle * Mathf.Deg2Rad);
            float angularSpeed = _rigidbody.velocity.magnitude * preset.handling / radius;
            _rigidbody.AddRelativeTorque(Vector3.up * angularSpeed, ForceMode.Acceleration);
        }
        
        vehicleRotation = _rigidbody.rotation.eulerAngles;

        // Checks for rotation boundaries
        if (vehicleRotation.y > rotateBackAngle && vehicleRotation.y < (360 - rotateBackAngle))
        {
            // Really extreme case. Snap back to original rotation
            if (vehicleRotation.y > 150 && vehicleRotation.y < 220)
            {
                _rigidbody.rotation = Quaternion.identity;
                vehicleRotation = _rigidbody.rotation.eulerAngles;
            }
            else
            {
                // Rotation direction
                vehicleRotation.y = vehicleRotation.y <= 180 ? rotateBackAngle : (360 - rotateBackAngle);
                _rigidbody.rotation = Quaternion.Euler(vehicleRotation);
            }
            
            // Debug.Log("Rotate back");
        }
    }

    #endregion
    
    // Raycast down from each of the Transforms in groundDetection
    private void CheckForGround()
    {
        foreach (var t in groundDetection)
        {
            if (Physics.Raycast(t.position, Vector3.down, groundCheckDistance, whatIsGround))
            {
                grounded = true;
                _rigidbody.drag = groundDrag;
                _rigidbody.freezeRotation = false;
                return;
            }
        }
        
        _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.identity, Time.deltaTime/2);

        grounded = false;
        _rigidbody.drag = airDrag;
        _rigidbody.freezeRotation = true;
    }

    public void ResetGroundDrag()
    {
        groundDrag = groundDragDefault;
    }
    
    public void SlowFowardForce(float slowValue)
    {
        StartCoroutine(Slow(slowValue));
    }

    IEnumerator Slow(float slowValue)
    {
        forwardForce -= slowValue;

        yield return new WaitForSeconds(1f);

        forwardForce += slowValue;
    }

    public float GetCurrentSpeed()
    {
        return _rigidbody.velocity.magnitude;
    }

    public float GetMaximumSpeed()
    {
        return maximumSpeed;
    }

    public float GetActualMaximumSpeed()
    {
        return preset.speed * (1 / groundDrag - Time.fixedDeltaTime);
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        Debug.Log("Use item");
    }
}
