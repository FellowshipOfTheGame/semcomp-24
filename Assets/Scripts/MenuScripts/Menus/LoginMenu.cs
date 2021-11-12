using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#if SUBI_NO_ONIBUS_ONLINE
using Backend = SubiNoOnibus.Backend.Online.Requests;
#else
using Backend = SubiNoOnibus.Backend.Offline.Requests;
#endif

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
#if !UNITY_WEBGL && SUBI_NO_ONIBUS_ONLINE
            string authCookie = PlayerPrefs.GetString(Backend::UserAuthRequestHandler.authKey, string.Empty);

            if (string.IsNullOrEmpty(authCookie))
            {
                Open();
                RaycastBlockEvent.Invoke(false);
                yield break;
            }
#endif
            yield return Backend::UserAuthRequestHandler.ValidateSession(Close, Open);
            RaycastBlockEvent.Invoke(false);
        }

        public void InsertKey()
        {
            string key = keyInputField.text.Trim();

            IEnumerator getSessionRequest = Backend::UserAuthRequestHandler.GetSession
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
#if SUBI_NO_ONIBUS_ONLINE
            WebLink.OpenLinkJSPlugin(Backend.Online.Configurations.Endpoints.Login_url);
#endif            
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