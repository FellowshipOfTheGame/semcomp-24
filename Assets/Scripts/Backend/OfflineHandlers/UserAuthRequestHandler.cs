using System;
using System.Collections;
using UnityEngine.Networking;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class UserAuthRequestHandler
    {
        public const string authKey = "Auth";

        public static IEnumerator GetSession(SessionData data, Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            OnSuccess?.Invoke();

            yield return null;
        }

        public static IEnumerator Logout(Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            OnSuccess?.Invoke();
            
            yield return null;
        }

        public static IEnumerator ValidateSession(Action OnSuccess, Action OnFailure = null)
        {
            OnSuccess?.Invoke();

            yield return null;
        }
    }
}