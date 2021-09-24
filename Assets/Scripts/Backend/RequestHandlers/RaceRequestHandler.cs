using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class RaceRequestHandler
    {
        public static IEnumerator StartRace(Action<RaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_start_url);
            
            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);
            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UserAuthRequestHandler.SaveAuthCookie(request);
                var raceData = JsonUtility.FromJson<RaceData>(request.downloadHandler.text);
                OnSuccess?.Invoke(raceData);
            }
        }

        public static IEnumerator FinishRace(RaceData raceData, Action<FinishRaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            raceData.sign = Cryptography.GetSignature(raceData);
            string data = JsonUtility.ToJson(raceData);
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_finish_url, data);
            
            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                try
                {
                    FinishRaceData finishRaceData = JsonUtility.FromJson<FinishRaceData>(request.downloadHandler.text);
                    OnSuccess?.Invoke(finishRaceData);
                }
                catch
                {
                    OnFailure?.Invoke(request);
                }
            }
        }
    }
}
