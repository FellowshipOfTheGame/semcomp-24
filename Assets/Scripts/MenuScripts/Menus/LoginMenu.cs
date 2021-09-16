using SubiNoOnibus.Networking.Requests;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.UI
{
    public class LoginMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private PopupMessageWindow logger;

        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject inserKeyPanel;

        [SerializeField] private TMPro.TMP_InputField keyInputField;

        [Header("Error Messages")]
        [SerializeField] private string serverErrorMsg;
        [SerializeField] private string expiredErrorMsg;
        [SerializeField] private string invalidCodeErrorMsg;

        public IEnumerator ValidateCookie()
        {
            RaycastBlockEvent.Invoke(true);
#if !UNITY_WEBGL
            string authCookie = PlayerPrefs.GetString(UserAuthRequestHandler.authKey, string.Empty);

            if (string.IsNullOrEmpty(authCookie))
            {
                Open();
                RaycastBlockEvent.Invoke(false);
                yield break;
            }
#endif
            yield return UserAuthRequestHandler.ValidateSession(Close, Open);
            RaycastBlockEvent.Invoke(false);
        }

        public void InsertKey()
        {
            string key = keyInputField.text.Trim();

            IEnumerator getSessionRequest = UserAuthRequestHandler.GetSession
            (
                new SessionData(key),
                Close,
                HandleGetSessionErrors
            );

            StartCoroutine(getSessionRequest);
        }

        public void Open()
        {
            SwitchToLogin();
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

        public void OnPress_Login()
        {
            WebLink.OpenLinkJSPlugin(Endpoints.Login_url);
            
            SwitchToInsertKey();
        }

        private void HandleGetSessionErrors(UnityWebRequest request)
        {
            string message = string.Empty;
            bool shouldSwitchToLogin = true;

            string error = (string)JsonUtility.FromJson<ErrorMessageData>(request.downloadHandler.text);
            if (error.StartsWith("invalid "))
            {
                message = invalidCodeErrorMsg;
                shouldSwitchToLogin = false;
            }
            else if (error.StartsWith("otp ") || error.StartsWith("original "))
            {
                message = expiredErrorMsg;
            }
            else if (request.responseCode == 500)
            {
                message = serverErrorMsg;
            }

            if (shouldSwitchToLogin)
                SwitchToLogin();

            logger.LogError(message);
        }
    }
}