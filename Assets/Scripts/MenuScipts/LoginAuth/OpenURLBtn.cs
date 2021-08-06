using UnityEngine;
using UnityEngine.UI;

public class OpenURLBtn : MonoBehaviour
{
    [SerializeField]
    private string url;

    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonCallback);
    }

    private void ButtonCallback()
    {
        Application.OpenURL(url);
        Debug.Log("Botao apertado! Yey");
    }
}
