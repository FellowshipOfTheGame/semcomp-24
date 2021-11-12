using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class RankingRequestHandler
    {
        public static IEnumerator GetRanking(Action<RankingData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            
            UserData user = UserController.getUserData();
            
            RankingPlayerData[] ranking = new RankingPlayerData[]
            {
                { "Susy da Costa Dutra", 353281 },
                { "André Santana", 266398 },
                { "Sandy da Costa Dutra", 160370 },
                { user.name, user.topScore }
            };
            
            RankingPersonalData personalRank = new RankingPersonalData();
            personalRank.name = user.name;
            personalRank.topScore = user.topScore;
            personalRank.position = 4;
            
            for(int i = 2; i >= 0 && ranking[i].topScore < user.topScore; i++)
            {
                ranking[i+1] = ranking[i];
                ranking[i].name = user.name;
                ranking[i].topScore = user.topScore;
                
                personalRank.position = i+1;
            }

            RankingData finalRanking = new RankingData();
            finalRanking.personal = personalRank
            finalRanking.rank = ranking;
            
            OnSuccess?.Invoke(rankingData);

            RaycastBlockEvent.Invoke(false);
        }
    }
}
