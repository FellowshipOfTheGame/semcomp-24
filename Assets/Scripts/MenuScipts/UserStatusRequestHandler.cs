using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UserStatusRequestHandler : MonoBehaviour
{
    [ContextMenu("Get user status")]
    public void GetStatus()
    {
        StartCoroutine(GetUserStatus());
    }

    public IEnumerator GetUserStatus()
    {
        using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.User_status_url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
        }
        Debug.Log(request.responseCode);
        Debug.Log(request.downloadHandler.text);
    }
}