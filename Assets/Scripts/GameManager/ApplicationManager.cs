using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private void OnApplicationFocus(bool focus)
    {
        FMODUnity.RuntimeManager.PauseAllEvents(!focus);
    }
}
