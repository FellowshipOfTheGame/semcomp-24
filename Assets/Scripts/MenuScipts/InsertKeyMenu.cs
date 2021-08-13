using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InsertKeyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LoginMenu loginMenu;
    [SerializeField] private PopupMessageWindow logInterface;

    [Header("UI Fields")]
    [SerializeField] private TMPro.TMP_InputField keyInputField;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Error Messages")]
    [SerializeField] private string invalidFieldMsg;
    [SerializeField] private string expiredCodeMsg;
    [SerializeField] private string serverErrorMsg;

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
            HandleServerError(request);
        }
        else
        {
            string cookie = request.GetResponseHeader("Set-Cookie");
            PlayerPrefs.SetString("Auth", cookie);
            
            SwitchToMainMenu();
        }
        Debug.Log(request.downloadHandler.text);
    }

    private void HandleServerError(UnityWebRequest request)
    {
        if (request.responseCode != 500L)
        {
            Debug.LogError("Get Session resulted in: " + request.responseCode);
            logInterface.LogError(request.error);
            OnClick_SwitchToLogin();
            return;
        }

        string errorMsg = JsonUtility.FromJson<ErrorMessageData>(request.downloadHandler.text);
        
        if (errorMsg.StartsWith("invalid field", StringComparison.InvariantCultureIgnoreCase))
        {
            logInterface.LogError(invalidFieldMsg);
        }
        else if (errorMsg.Equals("otp code expired", StringComparison.InvariantCultureIgnoreCase))
        {
            logInterface.LogError(expiredCodeMsg);
            OnClick_SwitchToLogin();
        }
        else if (errorMsg.Equals("original session expired", StringComparison.InvariantCultureIgnoreCase))
        {
            logInterface.LogError("original session expired");
            //TODO(tijolo): perguntar diferenca de original session para otp code
        }
        else if(errorMsg.Equals("internal server error", StringComparison.InvariantCultureIgnoreCase))
        {
            logInterface.LogError(serverErrorMsg);
            OnClick_SwitchToLogin();
        }
        
        Debug.Log("Error: " + errorMsg);
    }
}
