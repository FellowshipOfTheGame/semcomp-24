using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource start;
    public AudioSource loop;

    private void PlayLoop()
    {
        loop.Play();
    }

    private void Start()
    {
        Invoke(nameof(PlayLoop), start.clip.length);
        MusicPlayer p = FindObjectOfType<MusicPlayer>();
        
        if (p != null && p != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (start.time > 31)
        {
            start.mute = true;
            loop.mute = false;
        }
    }
    
}
