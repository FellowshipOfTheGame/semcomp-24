using System;
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
    public float turningAngle; // Current turning wheel angle
    
    private float forwardForce = 80f;
    private bool grounded; // State management
    
    // Components
    private Rigidbody _rigidbody;
    private Collider[] _colliders;
    
    #region MonoBehaviour Messages

    protected void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();

        if (preset is null)
            return;
        forwardForce = preset.speed;
    }

    protected void Update()
    {
        // Testing
        if (Keyboard.current.dKey.isPressed)
        {
            turningAngle = 90f;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            turningAngle = -90f;
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
        
        _rigidbody.AddRelativeForce(Vector3.forward * forwardForce);
        _rigidbody.AddForce(-_rigidbody.velocity); // Goes up to a certain maximum speed

        if (turningAngle != 0f)
        {
            float radius = wheelBase / Mathf.Sin(turningAngle * Mathf.Deg2Rad);
            float angularSpeed = _rigidbody.velocity.magnitude * preset.handling / radius;
            _rigidbody.AddRelativeTorque(Vector3.up * angularSpeed);
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
                return;
            }
        }

        grounded = false;
        _rigidbody.drag = airDrag;
    }

}
