using System.Collections;
using UnityEngine.Networking;
using System;

namespace SubiNoOnibus.Networking.Requests
{
    public static class UserStatusRequestHandler
    {
        public static IEnumerator GetUserStatus(Action OnSuccess, Action OnFailure = null)
        {
            using UnityWebRequest request = WebRequestFactory.AuthGet(Endpoints.User_status_url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke();
            }
            else
            {
                UserAuthRequestHandler.SaveAuthCookie(request);
                OnSuccess?.Invoke();
            }
        }
    }
}