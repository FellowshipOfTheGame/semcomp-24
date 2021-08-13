using UnityEngine;
using System.IO;

public class LoadApiSettings : MonoBehaviour
{
    private const string _apiSettingsPath = "Secrets/api_settings";

    [SerializeField] private TMPro.TextMeshProUGUI textMesh;

    private void Awake()
    {
        var json = Resources.Load<TextAsset>(_apiSettingsPath);
        var settings = JsonUtility.FromJson<ApiSettings>(json.text);

        Endpoints.Base_url = settings.api_url;
        textMesh.SetText(settings.api_url);
    }

    [System.Serializable]
    private struct ApiSettings
    {
        public string api_url;
        public string signature_key;
    }
}
