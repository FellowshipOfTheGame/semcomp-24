using System.Collections;
using UnityEngine.Networking;
using System;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class UserStatusRequestHandler
    {
        public static IEnumerator GetUserStatus(Action<UserStatus> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            OnSuccess?.Invoke(UserController.GetUserData());
            RaycastBlockEvent.Invoke(false);

            yield return null;
        }
    }
}