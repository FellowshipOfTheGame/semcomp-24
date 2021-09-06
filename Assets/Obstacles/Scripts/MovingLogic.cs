using UnityEngine;

public class MovingLogic : MonoBehaviour
{
    [Header("General")]

    [SerializeField]
    private int speed;

    [SerializeField]
    [Tooltip("Obstacle orientation. If left unchecked, moves backwards")]
    private bool forward;
    
    [Header("Tira-Tampa")]

    //verifica se o obstáculo do qual você colocou é para ser um tiratampa
    [SerializeField]
    private bool isTiraTampa;
    
    //tempo para destruir o tiratampa
    [SerializeField]
    [Tooltip("Only for Tira-Tampa")]
    private int timeToDestroy;
    
    [Header("Animations")]

    [SerializeField]
    private string idleAnimation;

    [SerializeField]
    private string deathAnimation;

    [SerializeField]
    private Animator animator;
    
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (!forward)
        {
            _rigidbody.rotation = Quaternion.Euler(Vector3.up * 180);
        }

        if (isTiraTampa)
        {
            Destroy(gameObject, timeToDestroy);
        }
    }
   
    void FixedUpdate()
    {
        _rigidbody.AddRelativeForce(Vector3.forward *  speed, ForceMode.Acceleration);
    }
}
