using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLogic : MonoBehaviour
{
    [SerializeField]
    private int speed;

    [SerializeField]
    private GameObject movingObstacle;

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    [Range(0, 360)]
    private int rotation;

    [SerializeField]
    [Range(-1, 1)]
    private int upOrDown;

    [SerializeField]
    [Range(-1, 1)]
    private int leftOrRight;

    //verifica se o obstáculo do qual você colocou é para ser um tiratampa
    [SerializeField]
    private bool isTiraTampa;

    [SerializeField]
    private bool moveUpDown;

    [SerializeField]
    private bool moveLeftRight;

    //tempo para destruir o tiratampa
    [SerializeField]
    private int timeToDestroy;

    [SerializeField]
    private string idleAnimation;

    [SerializeField]
    private string deathAnimation;

    [SerializeField]
    private Animator animation;

    void Start()
    {
        movingObstacle.transform.Rotate(0.0f, rotation, 0,0f);

        //se for o tiratampa...
        if(isTiraTampa)
        {
            //chama a rotina para destruir o tiratampa depois de X segundos
            StartCoroutine(destroyAfterSeconds(timeToDestroy));
        }
    }
   
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(moveUpDown)
        {
            _rigidbody.AddForce(upOrDown * Vector3.forward * speed);
        }
        else
        {
            if(moveLeftRight)
            {
                _rigidbody.AddForce(leftOrRight * Vector3.right * speed);
            }
        }
    }

    //rotina que quando chamada destroi o obstaculo após X segundos
    IEnumerator destroyAfterSeconds(int timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);

        Destroy(gameObject);
    }
}
