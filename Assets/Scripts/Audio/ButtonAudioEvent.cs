using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioEvent : MonoBehaviour, IPointerClickHandler
{
    private FMODUnity.StudioEventEmitter _eventEmitter;

    private void Awake()
    {
        _eventEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Play();
    }

    public void Play()
    {
        _eventEmitter.Play();
    }
}
