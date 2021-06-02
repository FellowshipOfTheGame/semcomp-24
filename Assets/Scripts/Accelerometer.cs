using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Accelerometer : MonoBehaviour
{
    public static Accelerometer Instance { get; set; }

    public RectTransform pointer;
    public Image pointerRenderer;
     public float value = 0.5f;

    public Color low = Color.red;
    public Color high = Color.green;
    
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        float angle = Mathf.LerpUnclamped(0f, 270f, value);
        pointer.rotation = Quaternion.Euler(0f, 0f, 90 - angle);

        Color.RGBToHSV(low, out float h1, out float s1, out float v1);
        Color.RGBToHSV(high, out float h2, out float _, out float _);
        
        pointerRenderer.color = Color.HSVToRGB(Mathf.Lerp(h1, h2, value), s1, v1);
    }
}
