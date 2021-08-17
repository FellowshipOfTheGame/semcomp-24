using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class UserAuthRequestHandler
    {
        public static IEnumerator GetSession(SessionData data, Action OnSuccess, Action<UnityWebRequest> OnFailure)
        {
            using UnityWebRequest request = WebRequestFactory.PostJson(Endpoints.Login_get_session_url, JsonUtility.ToJson(data));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                string cookie = request.GetResponseHeader("Set-Cookie");
                PlayerPrefs.SetString("Auth", cookie);

                OnSuccess?.Invoke();
            }
        }

        public static IEnumerator Logout(Action OnSuccess, Action<UnityWebRequest> OnFailure)
        {
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Logout_url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UnityWebRequest.ClearCookieCache();
                OnSuccess?.Invoke();
            }
        }

        public static IEnumerator ValidateSession(Action OnSuccess)
        {
            using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.Session_validate_url);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                OnSuccess?.Invoke();
            }
        }
    }
}
