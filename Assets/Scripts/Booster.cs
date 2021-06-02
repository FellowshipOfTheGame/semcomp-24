using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public Material material;

    private void Update()
    {
        material.mainTextureOffset += Vector2.down * (0.5f * Time.deltaTime);
    }
}
