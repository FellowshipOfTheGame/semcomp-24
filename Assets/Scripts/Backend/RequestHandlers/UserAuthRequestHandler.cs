using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class UserAuthRequestHandler
    {
        public const string authKey = "Auth";

        public static void SaveAuthCookie(UnityWebRequest request)
        {
            string cookie = request.GetResponseHeader("Set-Cookie");
            
            if (!string.IsNullOrEmpty(cookie))
            {
                PlayerPrefs.SetString(authKey, cookie);
            }
        }

        public static IEnumerator GetSession(SessionData data, Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.PostJson(Endpoints.Login_get_session_url, JsonUtility.ToJson(data));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                SaveAuthCookie(request);

                OnSuccess?.Invoke();
            }
            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator Logout(Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Logout_url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UnityWebRequest.ClearCookieCache();
                PlayerPrefs.DeleteKey(authKey);
                
                OnSuccess?.Invoke();
            }
            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator ValidateSession(Action OnSuccess, Action OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.AuthGet(Endpoints.Session_validate_url);

            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);
            if (request.result == UnityWebRequest.Result.Success)
            {
                OnSuccess?.Invoke();
            }
            else
            {
                OnFailure?.Invoke();
            }
        }
    }
}