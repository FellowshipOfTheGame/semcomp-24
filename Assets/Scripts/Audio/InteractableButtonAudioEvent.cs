using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractableButtonAudioEvent : MonoBehaviour, IPointerDownHandler
{
    private Button _button;
    
    [FMODUnity.EventRef] [SerializeField] private string interactableEvent;
    [FMODUnity.EventRef] [SerializeField] private string nonInteractableEvent;

    private void Awake() => _button = GetComponent<Button>();

    public void Play()
    {
        string audioPath = _button.IsInteractable() ? interactableEvent : nonInteractableEvent;

        FMODUnity.RuntimeManager.PlayOneShot(audioPath);
    }

    public void OnPointerDown(PointerEventData eventData) => Play();
}
