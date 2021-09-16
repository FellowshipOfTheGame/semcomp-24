using UnityEngine;
using UnityEngine.Rendering;

public class TimeTravelPortal : MonoBehaviour
{
    [SerializeField] private float enterBlendDistance = 10f;
    [SerializeField] private float exitBlendDistance = 5f;

    public GameObject GlobalVolumePast { get; set; }

    private Volume localVolume;

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
            enteredCount++;

            if (enteredCount == 1)
            {
                GlobalVolumePast.SetActive(true);
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
                    GlobalVolumePast.SetActive(false);
                    enteredCount = 0;
                    exitedCount = 0;
                    localVolume.blendDistance = enterBlendDistance;
                    break;
            }
        }
    }
}
