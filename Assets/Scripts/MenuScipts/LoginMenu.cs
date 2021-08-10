using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private GameObject inserKeyPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject initPanel;

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
