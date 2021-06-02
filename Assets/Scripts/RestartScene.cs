using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    public void Restart()
    {
        Accelerometer.Instance = null;
        SceneManager.LoadScene(0);
    }
    
}
