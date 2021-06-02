using System;
using TMPro;
using UnityEngine;

public class BusController : MonoBehaviour
{
    public Camera cam;

    public TMP_Text score;

    public static bool started = false;
    
    public float currentSpeed = 10f;
    public float targetSpeed = 15f;
    public float targetGroundSpeed = 8f;
    public float maxSpeed = 25f;

    public float acceleration = 30f;
    public float deacceleration = 50f;
    
    public float speedLinesThreshold = 10f;
    
    private Rigidbody _rb;

    public AudioSource normal;
    public AudioSource mud;
    public AudioSource crash;

    public GameObject a;
    public GameObject b;

    private bool end = false;
    
    private bool ground;

    private float time;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        time = 0f;
    }

    private void Update()
    {
        if (!started || end)
            return;

        if (!end)
            time += Time.deltaTime;
        
        if (!(Accelerometer.Instance is null))
            Accelerometer.Instance.value = currentSpeed / maxSpeed;

        // if (ground)
        // {
        //     mud.mute = false;
        //     normal.mute = true;
        // }
        // else
        // {
        //     mud.mute = true;
        //     normal.mute = false;
        // }

        normal.pitch = 1 + (currentSpeed/maxSpeed) * 0.5f;
        
        // normal.pitch = 1 + (currentSpeed/maxSpeed) * 0.5f;
        // mud.pitch = 1 + (currentSpeed/maxSpeed) * 0.5f;
        
        // if (currentSpeed > speedLinesThreshold)
        //     SpeedLines.Instance.intensity = 3f * (currentSpeed - speedLinesThreshold) / (maxSpeed - speedLinesThreshold);
        // else
        //     SpeedLines.Instance.intensity = 0f;

        // Updates position
        var t = transform;
        var pos = t.position;
        _rb.velocity = t.forward * currentSpeed;
        _rb.angularVelocity = Vector3.zero;

        // Updates camera
        var camT = cam.transform;
        var camPos = camT.position;
        camPos.x = pos.x;
        camPos.z = pos.z - 6;
        camT.position = camPos;

        float target = ground ? targetGroundSpeed : targetSpeed;
        
        if (currentSpeed < target)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, acceleration * Time.deltaTime);
        }
        else
        {
            float deacc = deacceleration;
            if (ground)
                deacc *= 8;
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, deacc * Time.deltaTime);
        }

        int seconds = (int) time;
        int ms =(int) ((time - seconds) * 1000);
        score.text = $"{seconds}s{ms:D3}ms";
    }

    private void OnCollisionEnter(Collision _)
    {
        currentSpeed = Mathf.Max(0f, currentSpeed - 10f);
        crash.Play();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 9)
        {
            ground = true;
        }
        else if (col.gameObject.layer == 10)
        {
            a.SetActive(true);
            b.SetActive(true);
            end = true;
        }
        else
        {
            currentSpeed += 15f;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
            ground = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            ground = false;
        }
    }
}
