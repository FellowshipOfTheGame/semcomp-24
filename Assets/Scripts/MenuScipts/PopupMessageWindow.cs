using System.Collections;
using UnityEngine;

public class PopupMessageWindow : MonoBehaviour, SubiNoOnibus.ILogger
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color errorColor;

    [SerializeField] private TMPro.TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(CloseWindow);
    }

    public void Log(object message)
    {
        textMesh.color = normalColor;
        DisplayText(message);
    }
    public void LogWarning(object message)
    {
        textMesh.color = warningColor;
        DisplayText(message);
    }

    public void LogError(object message)
    {
        textMesh.color = errorColor;
        DisplayText(message);
    }

    private void DisplayText(object message)
    {
        textMesh.SetText(message.ToString());

        gameObject.SetActive(true);
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
