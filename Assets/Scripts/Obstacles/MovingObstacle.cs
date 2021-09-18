using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Space(10)]
    [Header("General")]
    
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

    [Space(10)]
    [Header("Brake")]
    
    [SerializeField]
    [Tooltip("Should the obstacle look ahead and brake when it sees an obstacle?")]
    private bool brake;

    [SerializeField]
    [Tooltip("Should the obstacle wait for the player to get closer in order to start moving?")]
    private bool wakeOnProximity;

    [SerializeField]
    [Tooltip("Distance from the player at which the object starts moving (wake on proximity must be checked)")]
    private float wakeDistance;

    [SerializeField]
    [Tooltip("Check this if you have nested objects")]
    private bool useParentPosition;
    
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
    private GameObject player;

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
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * 180);
        }

        if (randomizeMaximumSpeed)
        {
            maximumSpeed = Random.Range(maximumSpeed - 2, maximumSpeed + 3);
        }

        if (wakeOnProximity)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
   
    void FixedUpdate()
    {
        Vector3 position = useParentPosition ? transform.parent.position : transform.position; 
        
        // If the obstacle is set to wake on proximity, check the distance
        if (wakeOnProximity && (position.z - player.transform.position.z) > wakeDistance)
        {
            return;
        }
        
        // If there's an object ahead, let the LookAhead method take care of the current speed value
        if (!LookAhead(ref currentSpeed) || !brake)
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
            speed = hit.rigidbody != null && Vector3.Angle(hit.rigidbody.velocity, transform.forward) == 0f ? hit.rigidbody.velocity.magnitude : 0f;

            // Debug.Log($"{gameObject.name} is seeing object {hit.transform.name} with speed {hit.rigidbody.velocity.magnitude}");

            didHit = true;
        }

        if (brake)
        {
            debugRayColor = hit.collider == null ? Color.yellow : Color.red;
            Debug.DrawRay(origin, direction * (1 + brakingDistance), debugRayColor);
        }

        return didHit;
    }

    void OnDrawGizmos()
    {
        if (_collider == null || !brake)
        {
            return;
        }
        
        Gizmos.color = debugRayColor;
        Gizmos.DrawWireSphere(transform.position, fieldOfView > 0 ? fieldOfView / 2f : _collider.bounds.extents.x);
    }
}
