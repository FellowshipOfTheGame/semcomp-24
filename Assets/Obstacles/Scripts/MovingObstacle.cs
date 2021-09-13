using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum speed the obstacle will reach. Note that if the randomize field is checked, this value will only be used as a basis")]
    private int maximumSpeed = 30;

    [SerializeField]
    [Tooltip("Check this to randomize the maximum speed within a certain range. This is NOT applicable to acceleration")]
    private bool randomizeMaximumSpeed = true;

    [SerializeField]
    [Tooltip("This field is used only as a basis and its actual value will vary randomly")]
    private float acceleration = 8f;

    [SerializeField]
    [Tooltip("Obstacle orientation. If left unchecked, the obstacle will move backwards")]
    private bool forward = true;

    [SerializeField]
    [Tooltip("The objects from which the obstacle will keep distance")]
    private LayerMask lookAtLayers;

    [SerializeField]
    [Tooltip("Distance at which the obstacle starts to stop")]
    private float brakingDistance = 10f;

    [SerializeField]
    [Tooltip("The obstacle field of view. Assign 0 to use the obstacle width")]
    private float fieldOfView;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private float currentSpeed;
    
    private Animator animator;
    private static readonly int AnimatorSpeed = Animator.StringToHash("Speed");

    private Color debugRayColor;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
        animator = GetComponentInChildren<Animator>();

        if (!forward)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * 180);
        }

        if (randomizeMaximumSpeed)
        {
            maximumSpeed = Random.Range(maximumSpeed - 2, maximumSpeed + 3);
        }
    }
   
    void FixedUpdate()
    {
        // If there's an object ahead, let the LookAhead method take care of the current speed value
        if (!LookAhead(ref currentSpeed))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maximumSpeed, Random.Range(acceleration - 0.5f, acceleration + 0.5f) * Time.deltaTime);
        }

        _rigidbody.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);

        if (animator != null)
        {
            animator.SetFloat(AnimatorSpeed, currentSpeed);
        }
    }

    private bool LookAhead(ref float speed)
    {
        bool didHit = false;

        Vector3 direction = transform.forward;
        Vector3 origin =  transform.position + (direction * _collider.bounds.extents.z);

        Ray ray = new Ray(origin, direction);
        float radius = fieldOfView > 0 ? fieldOfView / 2f : _collider.bounds.extents.x;

        if (Physics.SphereCast(ray, radius, out RaycastHit hit, brakingDistance, lookAtLayers))
        {
            speed = hit.rigidbody != null ? hit.rigidbody.velocity.magnitude : 0f;

            // Debug.Log($"{gameObject.name} is seeing object {hit.transform.name} with speed {hit.rigidbody.velocity.magnitude}");

            didHit = true;
        }

        debugRayColor = hit.collider == null ? Color.yellow : Color.red;
        Debug.DrawRay(origin, direction * (1 + brakingDistance), debugRayColor);

        return didHit;
    }

    void OnDrawGizmos()
    {
        if (_collider == null)
        {
            return;
        }
        
        Gizmos.color = debugRayColor;
        Gizmos.DrawWireSphere(transform.position, fieldOfView > 0 ? fieldOfView / 2f : _collider.bounds.extents.x);
    }
}
