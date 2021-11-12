using UnityEngine;

public class BackendLoader : MonoBehaviour
{
#if SUBI_NO_ONIBUS_ONLINE
    private const string _apiSettingsPath = "Secrets/api_settings";

    public void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var json = Resources.Load<TextAsset>(_apiSettingsPath);
        var settings = JsonUtility.FromJson<ApiSettings>(json.text);

        SubiNoOnibus.Backend.Online.Configurations.Endpoints.Base_url = settings.api_url;
        SubiNoOnibus.Backend.Online.Utils.Cryptography.request_signature_key = settings.request_signature_key;
        SubiNoOnibus.Backend.Online.Utils.Cryptography.response_signature_key = settings.response_signature_key;
    }

    [System.Serializable]
    public struct ApiSettings
    {
        public string api_url;
        public string response_signature_key;
        public string request_signature_key;
    }
#endif
}