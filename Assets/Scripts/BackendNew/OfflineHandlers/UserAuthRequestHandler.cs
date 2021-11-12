using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class UserAuthRequestHandler
    {
        public const string authKey = "Auth";

        public static IEnumerator GetSession(SessionData data, Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            OnSuccess?.Invoke();
            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator Logout(Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            OnSuccess?.Invoke();
            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator ValidateSession(Action OnSuccess, Action OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            OnSuccess?.Invoke();
            RaycastBlockEvent.Invoke(false);
        }
    }
}