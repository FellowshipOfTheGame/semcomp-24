using System;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeTravelPortal : MonoBehaviour
{
    [SerializeField] private VolumeProfile pastVolumeProfile;
    [SerializeField] private float enterBlendDistance = 10f;
    [SerializeField] private float exitBlendDistance = 5f;

    public Volume GlobalVolume { get; set; }

    private Volume localVolume;
    
    private VolumeProfile defaultVolumeProfile;

    private int enteredCount;
    private int exitedCount;

    public bool Out => (enteredCount == 0);

    void Start()
    {
        localVolume = GetComponent<Volume>();
        localVolume.blendDistance = enterBlendDistance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (defaultVolumeProfile == null)
            {
                defaultVolumeProfile = GlobalVolume.profile;
            }
            
            enteredCount++;

            if (enteredCount == 1)
            {
                GlobalVolume.profile = pastVolumeProfile;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exitedCount++;

            switch (exitedCount)
            {
                case 1:
                    localVolume.blendDistance = exitBlendDistance;
                    break;
                case 2:
                    GlobalVolume.profile = defaultVolumeProfile;
                    enteredCount = 0;
                    exitedCount = 0;
                    localVolume.blendDistance = enterBlendDistance;
                    break;
            }
        }
    }
}
