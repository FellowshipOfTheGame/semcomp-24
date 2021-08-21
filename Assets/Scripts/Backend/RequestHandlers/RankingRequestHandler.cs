﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class RankingRequestHandler
    {
        public static IEnumerator GetRanking(Action<RankingData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.Ranking_url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                var rankingData = JsonUtility.FromJson<RankingData>(request.downloadHandler.text);
                OnSuccess?.Invoke(rankingData);
            }
        }
    }
}