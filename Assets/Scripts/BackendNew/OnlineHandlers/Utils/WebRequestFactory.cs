using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace namespace SubiNoOnibus.Backend.Online.Utils
{
    static class WebRequestFactory
    {
        public static UnityWebRequest PostJson(string url, string data = "{}")
        {
            //UnityWebRequest request = new UnityWebRequest(url, "POST");
            UnityWebRequest request = new UnityWebRequest(new System.Uri(url), "POST");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            request.timeout = 10;
            return request;
        }

        public static UnityWebRequest AuthPostJson(string url, string data = "{}")
        {
            UnityWebRequest request = PostJson(url, data);

#if !UNITY_WEBGL
            string authCookie = PlayerPrefs.GetString(UserAuthRequestHandler.authKey, string.Empty);
            request.SetRequestHeader("Cookie", authCookie);
#endif
            request.timeout = 10;
            return request;
        }

        public static UnityWebRequest AuthGet(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

#if !UNITY_WEBGL
            string authCookie = PlayerPrefs.GetString(UserAuthRequestHandler.authKey, string.Empty);
            request.SetRequestHeader("Cookie", authCookie);
#endif
            request.timeout = 10;
            return request;
        }
    }
}