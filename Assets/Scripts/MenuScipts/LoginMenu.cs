using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private GameObject inserKeyPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject initPanel;

    public IEnumerator Start()
    {
        var authCookie = PlayerPrefs.GetString("Auth", string.Empty);
        
        if (string.IsNullOrEmpty(authCookie))
            yield break;

        yield return GetSessionValidate();
    }

    private IEnumerator GetSessionValidate()
    {
        using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.Session_validate_url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            loginPanel.SetActive(false);
            initPanel.SetActive(true);
        }
    }

    public void SwitchToLoginButton(GameObject caller)
    {
        loginPanel.SetActive(true);
        caller.SetActive(false);
    }

    public void SwitchToMainMenu(GameObject caller)
    {
        initPanel.SetActive(true);
        caller.SetActive(false);
    }
    
    public void OnClick_Login()
    {
        Application.OpenURL(Endpoints.Login_url);

        inserKeyPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
}
