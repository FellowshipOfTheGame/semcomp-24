using UnityEngine;
using SubiNoOnibus.Networking;

public class LoadApiSettings : MonoBehaviour
{   
    private const string _apiSettingsPath = "Secrets/api_settings";
    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var json = Resources.Load<TextAsset>(_apiSettingsPath);
        var settings = JsonUtility.FromJson<ApiSettings>(json.text);

        Endpoints.Base_url = settings.api_url;
        Cryptography.request_signature_key = settings.signature_key;
    }

    [System.Serializable]
    public struct ApiSettings
    {
        public string api_url;
        public string signature_key;
    }
}
