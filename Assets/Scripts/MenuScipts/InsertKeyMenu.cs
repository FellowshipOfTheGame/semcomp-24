using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InsertKeyMenu : MonoBehaviour
{
    [SerializeField] private LoginMenu loginMenu;

    [Header("UI Fields")]
    [SerializeField] private TMPro.TMP_InputField keyInputField;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnClick_InsertKey);
        backButton.onClick.AddListener(OnClick_SwitchToLogin);
    }
    private void OnClick_SwitchToLogin()
    {
        loginMenu.SwitchToLoginButton(gameObject);
    }
    private void SwitchToMainMenu()
    {
        loginMenu.SwitchToMainMenu(gameObject);
    }

    private void OnClick_InsertKey()
    {
#if UNITY_EDITOR
        Debug.Log("Key inserted: " + keyInputField.text);
#endif
        StartCoroutine(GetSession(keyInputField.text));   
    }

    [System.Serializable]
    private struct GetSessionData
    {
        public GetSessionData(string key) => code = key;
        public string code;
    }

    private IEnumerator GetSession(string key)
    {
        GetSessionData data = new GetSessionData(key);
        using UnityWebRequest request = WebRequestFactory.PostJson(Endpoints.Login_get_session_url, JsonUtility.ToJson(data));
        
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);   
        }
        else
        {
            string cookie = request.GetResponseHeader("Set-Cookie");
            PlayerPrefs.SetString("Auth", cookie);
            Debug.Log(cookie);

            SwitchToMainMenu();
        }
        Debug.Log(request.downloadHandler.text);
    }
}
