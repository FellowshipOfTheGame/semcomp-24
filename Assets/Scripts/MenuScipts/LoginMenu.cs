using SubiNoOnibus.Networking.Requests;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject inserKeyPanel;

    [SerializeField] private TMPro.TMP_InputField keyInputField;

    public IEnumerator Start()
    {
        string authCookie = PlayerPrefs.GetString("Auth", string.Empty);
        
        if (string.IsNullOrEmpty(authCookie))
            yield break;

        yield return UserAuthRequestHandler.ValidateSession(Close);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void SwitchToLogin()
    {
        loginPanel.SetActive(true);
        inserKeyPanel.SetActive(false);
    }

    public void SwitchToInsertKey()
    {
        loginPanel.SetActive(false);
        inserKeyPanel.SetActive(true);
    }

    public void OnClick_Login()
    {
        Application.OpenURL(Endpoints.Login_url);

        SwitchToInsertKey();
    }
    public void InsertKey()
    {
        var getSessionEnumerator = UserAuthRequestHandler.GetSession(
            new SessionData(keyInputField.text),
            Close,
            HandleGetSessionErrors
        );
        StartCoroutine(getSessionEnumerator);
    }

    private void HandleGetSessionErrors(UnityWebRequest request)
    {
        Debug.Log("Error: " + request);
    }
}
