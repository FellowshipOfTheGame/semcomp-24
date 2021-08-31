using UnityEngine;
using SubiNoOnibus.Networking;

public class LoadApiSettings : MonoBehaviour
{   
    private const string _apiSettingsPath = "Secrets/api_settings";
    
    public void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var json = Resources.Load<TextAsset>(_apiSettingsPath);
        var settings = JsonUtility.FromJson<ApiSettings>(json.text);

        Endpoints.Base_url = settings.api_url;
        Cryptography.request_signature_key = settings.request_signature_key;
        Cryptography.response_signature_key = settings.response_signature_key;
    }

    [System.Serializable]
    public struct ApiSettings
    {
        public string api_url;
        public string response_signature_key;
        public string request_signature_key;
    }
}
