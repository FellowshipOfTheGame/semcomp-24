using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class RaceRequestHandler
    {
        public static IEnumerator StartRace(Action<RaceData> OnSuccess, Action<UnityWebRequest> OnFailure)
        {
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_start_url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                var raceData = JsonUtility.FromJson<RaceData>(request.downloadHandler.text);
                OnSuccess?.Invoke(raceData);
            }
        }

        public static IEnumerator FinishRace(RaceData raceData, Action OnSuccess, Action<UnityWebRequest> OnFailure)
        {
            Cryptography.SetSignature(raceData);
            
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_finish_url, JsonUtility.ToJson(raceData));
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                OnSuccess?.Invoke();
            }
        }
    }
}
