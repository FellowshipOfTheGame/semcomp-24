using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusTest: MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private TextMeshProUGUI currentSpeedText;

    private int speedMax = Int32.MaxValue;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        currentSpeedText.text = (_rigidbody.velocity.magnitude).ToString("00.00000") + "/" + GetComponent<VehicleController>().maximumSpeed;
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, speedMax);
    }

    private void Update()
    {
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            speedMax = (int) (_rigidbody.velocity.magnitude) - 1;
        }
        
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            speedMax = (int) (_rigidbody.velocity.magnitude) + 1;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
