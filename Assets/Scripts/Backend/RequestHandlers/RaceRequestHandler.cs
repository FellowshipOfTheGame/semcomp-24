﻿using System;
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
            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator FinishRace(RaceData raceData, Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            raceData.sign = Cryptography.GetSignature(raceData);
            string data = JsonUtility.ToJson(raceData);
            Debug.Log(data);
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Race_finish_url, data);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                OnSuccess?.Invoke();
            }
            RaycastBlockEvent.Invoke(false);
        }
    }
}
