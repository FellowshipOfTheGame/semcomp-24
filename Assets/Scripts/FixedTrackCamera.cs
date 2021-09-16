using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copies X and Z position with an offset
public class FixedTrackCamera : MonoBehaviour
{
    [SerializeField] private Transform track;
    [SerializeField] private float height;
    [SerializeField] private float distance;
    
    private void Update()
    {
        Vector3 position;
        position.x = track.position.x;
        position.y = track.position.y + height;
        position.z = track.position.z - distance;
        transform.position = position;

        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, Vector3.SignedAngle(Vector3.forward, track.forward, Vector3.up) / 3f, transform.localEulerAngles.z);
        
    }
    
}
