using System;
using System.Collections;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class RankingRequestHandler
    {
        public static IEnumerator GetRanking(Action<RankingData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {   
            UserStatus user = UserController.GetUserData();
            
            RankingPlayerData[] ranking = new RankingPlayerData[]
            {
                new RankingPlayerData() { name = "Susy da Costa Dutra", topScore = 353281 },
                new RankingPlayerData() { name = "André Santana", topScore = 266398 },
                new RankingPlayerData() { name = "Sandy da Costa Dutra", topScore = 160370 },
                new RankingPlayerData() { name = user.name, topScore = user.topScore }
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
            finalRanking.personal = personalRank;
            finalRanking.rank = ranking;

            OnSuccess?.Invoke(finalRanking);
                
            yield return null;
        }
    }
}
