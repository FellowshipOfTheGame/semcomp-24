using UnityEngine;

public class DynamicMusic : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioEventEmitter eventEmitter;

    public void BeginTransition()
    {
        eventEmitter.SetParameter(eventEmitter.Params[0].ID, 1);
    }

    public void EndTransition()
    {
        eventEmitter.SetParameter(eventEmitter.Params[0].ID, 0);
    }

    public void BeginMusic()
    {
        if (!eventEmitter.IsPlaying())
        {
            eventEmitter.Play();
        }
        else
        {
            PauseMusic(false);
        }
    }

    public void PauseMusic(bool value)
    {
        eventEmitter.EventInstance.setPaused(value);
    }
}
