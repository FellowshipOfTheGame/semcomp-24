using System;
using UnityEngine;
using UnityEngine.UI;

public class SpeedLines : MonoBehaviour
{
    public static SpeedLines Instance;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public RectTransform rect;
    public Image image;
    public float intensity = 0f;

    private void Update()
    {

        rect.Rotate(0f, 0f, intensity * 2f * Time.deltaTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Min(intensity, 0.3f));
    }
}
