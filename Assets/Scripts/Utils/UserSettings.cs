using UnityEngine;

[CreateAssetMenu(fileName = "DefaultUserSettings", menuName = "ScriptableObjects/UserSettings")]
public class UserSettings : ScriptableObject
{
    private const string Key = "UserSettings";
    
    [SerializeField] private bool isGlobalSoundMuted;

    private void OnEnable()
    {
        string json = PlayerPrefs.GetString(Key, string.Empty);
        
        if (!string.IsNullOrEmpty(json))
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        SetGlobalSoundState(isGlobalSoundMuted);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString(Key, JsonUtility.ToJson(this));
        PlayerPrefs.Save();
    }

    public void SetGlobalSoundState(bool muted)
    {
        FMODUnity.RuntimeManager.MuteAllEvents(muted);
        isGlobalSoundMuted = muted;
    }

}
