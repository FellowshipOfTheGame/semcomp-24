using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WheelController : MonoBehaviour
{
    public RectTransform wheel;
    public Transform bus;

    public float angle = 0f;
    public float maxAngle = 60f;
    private float _currAngle = 0f;
    
    private Vector2 _mouseInitialPos;
    private bool _drag;
    
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            BusController.started = true;
            
            _drag = true;
            _mouseInitialPos = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _drag = false;
            angle = 0f;
        }

        if (_drag)
        {
            Vector2 anchor = new Vector2(Screen.width * .5f, 0f);

            Vector2 from = _mouseInitialPos - anchor;
            Vector2 to = Mouse.current.position.ReadValue() - anchor;

            angle = Vector2.SignedAngle(from, to);
            angle *= 0.5f;
            angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
        }

        _currAngle = Mathf.MoveTowards(_currAngle, angle, 120f * Time.deltaTime);
        
        wheel.rotation = Quaternion.Euler(0f, 0f, _currAngle);
        bus.rotation = Quaternion.Euler(0, -_currAngle, 0);
    }
}
