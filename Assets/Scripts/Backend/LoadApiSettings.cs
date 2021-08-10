using UnityEngine;
using System.IO;

public class LoadApiSettings : MonoBehaviour
{
    private static readonly string _apiSettingsPath = Path.Combine("Assets", "Resources", "Secrets", "api_settings.json");

    private void Awake()
    {
        var json = File.ReadAllText(_apiSettingsPath);
        var settings = JsonUtility.FromJson<ApiSettings>(json);

        Endpoints.Base_url = settings.api_url;
    }

    [System.Serializable]
    private struct ApiSettings
    {
        public string api_url;
        public string signature_key;
    }
}
