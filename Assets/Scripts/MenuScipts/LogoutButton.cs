using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        StartCoroutine(LogoutRequest());
    }

    private IEnumerator LogoutRequest()
    {
        using UnityEngine.Networking.UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Logout_url);

        yield return request.SendWebRequest();

        if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.result);
            mainMenu.Logout();
        }
        Debug.Log(request.downloadHandler.text);
    }
}
