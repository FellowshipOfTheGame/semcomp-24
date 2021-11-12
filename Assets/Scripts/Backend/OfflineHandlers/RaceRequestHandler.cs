using System;
using System.Collections;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class RaceRequestHandler
    {
        public static IEnumerator StartRace(Action<RaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaceData mockedRaceData = new RaceData();
            mockedRaceData.nonce = "mockedNonce";
            
            OnSuccess?.Invoke(mockedRaceData);

            yield return null;
        }

        public static IEnumerator FinishRace(RaceData raceData, Action<FinishRaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            UserStatus user = UserController.GetUserData();
            FinishRaceData finishRaceData = new FinishRaceData();

            user.gold += raceData.gold;
            user.runs += 1;

            if(raceData.score > user.topScore)
            {
                finishRaceData.isPersonalRecord = true;
                user.topScore = raceData.score;
            }
             
            UserController.SaveUserData(user);

            OnSuccess?.Invoke(finishRaceData);
            
            yield return null;
        }
    }
}
