using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.UI;

public class OpenURLBtn : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OpenUrl);
    }

    private void OpenUrl()
    {
        Application.OpenURL("http://2136976e4f9d.ngrok.io/");
        Debug.Log("Abrindo..");
    }
}
