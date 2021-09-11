using System.Collections;
using UnityEngine;

public class TiraTampa : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time until the obstacle is destroyed")]
    private int destroyCountdown;
    
    private Animator animator;
    private static readonly int AnimatorDieTrigger = Animator.StringToHash("Die");

    private Rigidbody _rigidbody;
    private Obstacle obstacle;

    private Coroutine destroyCoroutine;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        obstacle = GetComponent<Obstacle>();
        animator = GetComponentInChildren<Animator>();

        destroyCoroutine = StartCoroutine(DestroyCountdown(destroyCountdown));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obstacle.StopCoroutine(obstacle.DestroyObstacleCoroutine);

            if (destroyCoroutine != null)
            {
                StopCoroutine(destroyCoroutine);
            }
            
            StartCoroutine(DestroyNow());
        }
    }

    private IEnumerator DestroyCountdown(float countdown)
    {
        yield return new WaitForSeconds(countdown);

        StartCoroutine(DestroyNow());
    }

    private IEnumerator DestroyNow()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        
        animator.SetTrigger(AnimatorDieTrigger);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        obstacle.FadeOut();
    }
}
