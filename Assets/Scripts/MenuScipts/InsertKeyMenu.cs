using SubiNoOnibus.Networking.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public partial class InsertKeyMenu : MonoBehaviour
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
        var getSessionEnumerator = UserAuthRequestHandler.GetSession(
            new SessionData(keyInputField.text),
            SwitchToMainMenu,
            HandleGetSessionErrors
        );

        StartCoroutine(getSessionEnumerator);
    }

    private void HandleGetSessionErrors(UnityWebRequest request)
    {
        string errorMsg = JsonUtility.FromJson<ErrorMessageData>(request.downloadHandler.text);
        
        if (errorMsg.StartsWith("invalid field"))
        {
            logInterface.LogError(invalidFieldMsg);
        }
        else if (errorMsg.Equals("otp code expired"))
        {
            logInterface.LogError(expiredCodeMsg);
            OnClick_SwitchToLogin();
        }
        else if (errorMsg.Equals("original session expired"))
        {
            logInterface.LogError("original session expired");
            OnClick_SwitchToLogin();
        }
        else if(errorMsg.Equals("internal server error"))
        {
            logInterface.LogError(serverErrorMsg);
            OnClick_SwitchToLogin();
        }
        else
        {
            Debug.LogError("Get Session resulted in: " + request.responseCode);
            logInterface.LogError(request.error);
            OnClick_SwitchToLogin();
        }
        
        Debug.Log("Error: " + errorMsg);
    }
}
