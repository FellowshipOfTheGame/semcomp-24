using UnityEngine;
using UnityEngine.UI;

public class OpenURLBtn : MonoBehaviour
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonCallback);
    }

    private void ButtonCallback()
    {
        Application.OpenURL("http://2136976e4f9d.ngrok.io/");
        Debug.Log("Botao apertado! Yey");
    }
}
