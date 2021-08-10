using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;

public class RaceStartTest : MonoBehaviour
{
    [SerializeField] private RaceData raceData;

    private IEnumerator StartRace()
    {
        using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_start_url, "{}");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            raceData = JsonUtility.FromJson<RaceData>(request.downloadHandler.text);
            Debug.Log(request.result);
        }
        Debug.Log(request.downloadHandler.text);
    }

    [ContextMenu("FinishRace")]
    public void StartCoroutineFinishRace()
    {
        raceData.sign = string.Empty;

        //TODO: ler chave do api_settings.json
        string signature = string.Empty;
        byte[] unicodeKey = Encoding.UTF8.GetBytes("MINAHSENHASUPERSECRETA");
        using (HMACSHA256 hmacSha256 = new HMACSHA256(unicodeKey))
        {
            string signString = JsonUtility.ToJson(raceData);
            Debug.Log(signString);
            Byte[] dataToHmac = Encoding.UTF8.GetBytes(signString);
            signature = Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
        }

        raceData.sign = signature;

        StartCoroutine(FinishRace());
    }

    [ContextMenu("StartRace")]
    public void StartCoroutineStartRace()
    {
        StartCoroutine(StartRace());
    }

    private IEnumerator FinishRace()
    {
        using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_finish_url, JsonUtility.ToJson(raceData));
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.result);
        }
        Debug.Log(request.downloadHandler.text);
    }

    [System.Serializable]
    private struct RaceData
    {
        public int score;
        public int gold;
        public string nonce;
        public string sign;
    }
}
