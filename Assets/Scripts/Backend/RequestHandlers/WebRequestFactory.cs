using System.Text;
using UnityEngine;
using UnityEngine.Networking;

static class WebRequestFactory
{
    public static UnityWebRequest PostJson(string url, string data = "{}")
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        
        return request;
    }

    public static UnityWebRequest AuthPostJson(string url, string data = "{}")
    {
        UnityWebRequest request = PostJson(url, data);

        string authCookie = PlayerPrefs.GetString("Auth", string.Empty);
        request.SetRequestHeader("Cookie", authCookie);

        return request;
    }

    public static UnityWebRequest AuthGetJson(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        string authCookie = PlayerPrefs.GetString("Auth", string.Empty);
        request.SetRequestHeader("Cookie", authCookie);
        
        return request;
    }
}