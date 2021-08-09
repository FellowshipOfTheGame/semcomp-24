using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private GameObject inserKeyPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject initPanel;
    
    public void OnClick_Login() 
    {
        Application.OpenURL(Endpoints.login_url);

        inserKeyPanel.SetActive(true);
        loginPanel.SetActive(false);
    }
}
