using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Networking.Requests;

public class UserStatusTest : MonoBehaviour
{
    public UserStatus userStatus;

    [ContextMenu("GetStatus")]
    public void GetStatus()
    {
        StartCoroutine(UserStatusRequestHandler.GetUserStatus(OnSuccess, OnFailure));
    }

    private void OnSuccess(UserStatus response)
    {
        userStatus = response;
        Debug.Log(JsonUtility.ToJson(userStatus, true));
    }
    private void OnFailure(UnityWebRequest request)
    {
        Debug.Log(request.error);
        Debug.Log(request.downloadHandler.text);
    }
}
