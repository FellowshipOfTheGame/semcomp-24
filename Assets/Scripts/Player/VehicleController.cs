using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AccelerationRange
{
    [SerializeField]
    private float acceleration;
    
    [SerializeField]
    private int upperLimit;

    public static float reference;

    public float Acceleration => acceleration;

    public bool Contain(float value)
    {
        return (value / reference * 100 <= upperLimit);
    }
}

/**
 * Physics interactions for vehicles
*/
public class VehicleController : MonoBehaviour
{
    // (Easier way to design different types of vehicles
    [Header("General")]
    public VehicleStatPreset preset;
    
    [Header("SFX's")]
    [SerializeField]
    private FMODUnity.StudioEventEmitter landingEventEmitter;
    [SerializeField]
    private FMODUnity.StudioEventEmitter motorEventEmmiter;

    [Header("Physics")]
    
    public AccelerationRange[] accelerationRanges;
    
    public float forwardForce = 80f;
    public float initialForwardForce = 0f;
    
    public LayerMask whatIsGround;
    public Transform[] groundDetection;
    public float groundCheckDistance = 0.5f;
    public float groundDrag = 3f;
    public float airDrag = 0.1f;
    public Transform centerOfMass;

    [Header("Turning")]
    
    [Tooltip("Distance between wheels")]
    public float wheelBase = 2f;

    [Tooltip("The maximum angle the player should be able to turn")]
    public float turningAngleMax = 15f;
    
    [Tooltip("The maximum angle the vehicle can rotate before rotating back to player's original position")]
    public float rotateBackAngle = 60f;

    [Tooltip("The compensation force that is applied in the x component of the forward force")]
    public float compensationForceControl = 3f;
    
    private float turningAngle; // Current turning wheel angle
    
    [Header("Wheels (aesthetic)")]
    
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private float wheelsRotationSpeed = 5f;
    
    private bool grounded; // State management

    // Components
    private Rigidbody _rigidbody;
    private Collider[] _colliders;
    private ItemSystem itemSystem;

    public BoxCollider VehicleCollider { get; private set; }

    public float maximumSpeed { get; private set; }

    public float groundDragDefault { get; private set; }
    
    private PlayerInput playerInput;
    private InputAction movement;
    private Vector3 vehicleRotation;

    private RigidbodyConstraints rigidbodyDefaultConstraints;

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
        itemSystem = GetComponent<ItemSystem>();

        VehicleCollider = GetComponent<BoxCollider>(); 

        groundDragDefault = groundDrag;
        
        if (preset is null)
            return;
        //forwardForce = preset.speed;
        forwardForce = initialForwardForce;

        rigidbodyDefaultConstraints = _rigidbody.constraints;

        AccelerationRange.reference = maximumSpeed;

        _rigidbody.centerOfMass = centerOfMass.position;
    }

    protected void Update()
    {
        // Check rotation boundaries
        bool turnRight = (vehicleRotation.y <= turningAngleMax || vehicleRotation.y > 180);
        bool turnLeft = (vehicleRotation.y >= (360 - turningAngleMax) || vehicleRotation.y <= 180);
        
        float turningInput = movement.ReadValue<float>();
        turningAngle = turnLeft || turnRight ? turningInput * 90f : 0f;

        Vector3 wheelRotationLeft = frontLeftWheel.localEulerAngles;
        Vector3 wheelRotationRight = frontRightWheel.localEulerAngles;

        if (turningInput != 0f)
        {
            if (vehicleRotation.y <= 180 && turningInput > 0 || vehicleRotation.y > 180 && turningInput < 0) // input para a direita virado para e esquerda ou input para e esquerda virado para a direita 
            {
                wheelRotationLeft.y = Mathf.LerpAngle(wheelRotationLeft.y, -90 + transform.localEulerAngles.y, wheelsRotationSpeed * Time.deltaTime);
                wheelRotationRight.y = Mathf.LerpAngle(wheelRotationRight.y, 90 + transform.localEulerAngles.y, wheelsRotationSpeed * Time.deltaTime);
            }
            else if (vehicleRotation.y <= 180 && turningInput < 0 || vehicleRotation.y > 180 && turningInput > 0) // input para a esquerda virado para a esquerda ou input para a direita virado para a esquerda
            {
                wheelRotationLeft.y = Mathf.LerpAngle(wheelRotationLeft.y, -90 - transform.localEulerAngles.y, wheelsRotationSpeed * Time.deltaTime);
                wheelRotationRight.y = Mathf.LerpAngle(wheelRotationRight.y, 90 - transform.localEulerAngles.y, wheelsRotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            wheelRotationLeft.y = Mathf.MoveTowardsAngle(wheelRotationLeft.y, -90, wheelsRotationSpeed * Time.deltaTime);
            wheelRotationRight.y = Mathf.MoveTowardsAngle(wheelRotationRight.y, 90, wheelsRotationSpeed * Time.deltaTime);
        }

        frontLeftWheel.localRotation = Quaternion.Euler(wheelRotationLeft);
        frontRightWheel.localRotation = Quaternion.Euler(wheelRotationRight);

        float rpm = Mathf.Clamp01(GetCurrentSpeed() / GetMaximumSpeed());
        motorEventEmmiter.SetParameter(motorEventEmmiter.Params[0].ID, rpm);
    }

    protected void FixedUpdate()
    {
        CheckForGround();
        if (!grounded)
        {
            return;
        }

        int i = 0;
        while (!accelerationRanges[i].Contain(GetCurrentSpeed())) i++;
        
        forwardForce = Mathf.MoveTowards(forwardForce, preset.speed, accelerationRanges[i].Acceleration * Time.deltaTime);
        
        _rigidbody.AddRelativeForce(Vector3.forward * forwardForce, ForceMode.Acceleration);

        if (i > 1)
        {
            Vector3 compensationDirection = turningAngle > 0 ? Vector3.right : turningAngle < 0 ? Vector3.left : Vector3.zero;
            _rigidbody.AddRelativeForce(compensationDirection * Mathf.Clamp((forwardForce / compensationForceControl), -preset.speed, preset.speed), ForceMode.Acceleration);
        }
        
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
            }
            
            // Debug.Log("Rotate back");
        }
        
        vehicleRotation.z = 0f;
        _rigidbody.rotation = Quaternion.Euler(vehicleRotation);
    }

    #endregion

    // Raycast down from each of the Transforms in groundDetection
    private void CheckForGround()
    {
        foreach (var t in groundDetection)
        {
            if (Physics.Raycast(t.position, Vector3.down, groundCheckDistance, whatIsGround))
            {
                if(!grounded)
                    landingEventEmitter.Play();
                
                grounded = true;
                _rigidbody.drag = groundDrag;
                _rigidbody.constraints = rigidbodyDefaultConstraints;
                return;
            }
        }
        
        _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.identity, Time.deltaTime/2);

        grounded = false;
        _rigidbody.drag = airDrag;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | rigidbodyDefaultConstraints;
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
        itemSystem.ActivateItem();
    }
}
