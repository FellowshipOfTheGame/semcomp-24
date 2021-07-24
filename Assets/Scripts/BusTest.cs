using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BusTest: MonoBehaviour
{
    private Rigidbody _rigidbody;

    public InputAction movement;

    [SerializeField]
    private int maxSpeed;

    [SerializeField]
    private TextMeshProUGUI currentSpeedText;
    
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        _rigidbody.AddForce(new Vector3(movement.ReadValue<Vector2>().x, 0, movement.ReadValue<Vector2>().y) * (maxSpeed * Time.fixedDeltaTime), ForceMode.VelocityChange);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxSpeed);
        currentSpeedText.text = (_rigidbody.velocity.magnitude).ToString("00.00000");
    }

    private void OnEnable() {
        movement.Enable();
    }
    
    private void OnDisable() {
        movement.Disable();
    }
}
