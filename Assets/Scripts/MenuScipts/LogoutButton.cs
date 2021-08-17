using UnityEngine;
using UnityEngine.UI;
using SubiNoOnibus.Networking.Requests;

[RequireComponent(typeof(Button))]
public class LogoutButton : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick_Logout);
    }

    public void OnClick_Logout()
    {
        var logoutEnumerator = UserAuthRequestHandler.Logout(
            () => mainMenu.Logout(),
            (req) => Debug.Log(req.error)
        );
        StartCoroutine(logoutEnumerator);
    }
}
