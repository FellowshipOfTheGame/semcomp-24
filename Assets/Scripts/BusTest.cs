using TMPro;
using UnityEngine;

public class BusTest: MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]
    private TextMeshProUGUI currentSpeedText;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        currentSpeedText.text = (_rigidbody.velocity.magnitude).ToString("00.00000");
    }
    
    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
