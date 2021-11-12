using System.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine;
using SubiNoOnibus.Backend.Online.Utils;
using SubiNoOnibus.Backend.Online.Configurations;

namespace SubiNoOnibus.Backend.Online.Requests
{
    public static class UserStatusRequestHandler
    {
        public static IEnumerator GetUserStatus(Action<UserStatus> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.AuthGet(Endpoints.User_status_url);

            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UserAuthRequestHandler.SaveAuthCookie(request);
                var userStatus = JsonUtility.FromJson<UserStatus>(request.downloadHandler.text);

                if (Cryptography.IsSignatureValid(userStatus))
                    OnSuccess?.Invoke(userStatus);
                else
                    OnFailure?.Invoke(request);
            }
        }
    }
}