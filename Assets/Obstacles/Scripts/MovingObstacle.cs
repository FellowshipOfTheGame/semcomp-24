using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Space(10)]
    [Header("General")]

    [SerializeField]
    [Tooltip("This field is used only as a base and its actual value will vary randomly")]
    private int maximumSpeed;

    [SerializeField]
    [Tooltip("This field is used only as a base and its actual value will vary randomly")]
    private float acceleration = 8f;

    [SerializeField]
    [Tooltip("Obstacle orientation. If left unchecked, the obstacle will move backwards")]
    private bool forward;
    
    [SerializeField]
    [Tooltip("The objects from which the obstacle will keep distance")]
    private LayerMask lookAtLayers;

    [SerializeField]
    [Tooltip("Distance at which the obstacle starts to stop")]
    private float brakingDistance = 10f;

    [SerializeField]
    private float laneWidth;

    [Space(10)]
    [Header("Tira-Tampa")]

    //verifica se o obstáculo do qual você colocou é para ser um tiratampa
    [SerializeField]
    private bool isTiraTampa;
    
    //tempo para destruir o tiratampa
    [SerializeField]
    [Tooltip("Only for Tira-Tampa")]
    private int timeToDestroy;
    
    [Space(10)]
    [Header("Animations")]

    [SerializeField]
    private string idleAnimation;

    [SerializeField]
    private string deathAnimation;

    [SerializeField]
    private Animator animator;
    
    private Rigidbody _rigidbody;
    private float currentSpeed;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (!forward)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * 180);
        }

        if (isTiraTampa)
        {
            Destroy(gameObject, timeToDestroy);
        }
        
        maximumSpeed = Random.Range(maximumSpeed - 2, maximumSpeed + 3);
    }
   
    void FixedUpdate()
    {
        LookAhead();
        
        currentSpeed = Mathf.Lerp(currentSpeed, maximumSpeed, Random.Range(acceleration - 0.5f, acceleration + 0.5f) * Time.deltaTime);
        _rigidbody.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);
    }

    private void LookAhead()
    {
        //* DEBUG ------------
        
        Vector3 checkpoint = transform.rotation.y == 0 ? new Vector3(0, 0, brakingDistance) : new Vector3(0, 0, -brakingDistance);
        Debug.DrawRay(transform.position, transform.forward + checkpoint, Color.yellow);

        //-------------------*/
        
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, laneWidth / 2f, out RaycastHit hit, brakingDistance, lookAtLayers))
        {
            if (hit.rigidbody != null)
            {
                // float clampedSpeed = hit.distance > brakingDistance / 4f
                //     ? Mathf.Lerp(_rigidbody.velocity.magnitude, hit.rigidbody.velocity.magnitude, acceleration * 4f * Time.deltaTime)
                //     : 0;

                // _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, clampedSpeed);
                
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, hit.rigidbody.velocity.magnitude);
            }
            else
            {
                Debug.Log("The rigidbody of the object I hit is null!");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, laneWidth / 2f);
    }
}
