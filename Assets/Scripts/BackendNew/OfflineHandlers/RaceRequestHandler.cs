using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class RaceRequestHandler
    {
        public static IEnumerator StartRace(Action<RaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            RaceData mockedRaceData = new RaceData();
            mockedRaceData.nonce = "mockedNonce";
            
            OnSuccess?.Invoke(mockedRaceData);

            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator FinishRace(RaceData raceData, Action<FinishRaceData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            UserData user = UserController.getUserData();
            FinishRaceData finishRaceData = new FinishRaceData();

            user.gold += raceData.gold;
            user.runs += 1;

            if(raceData.score > user.topScore)
            {
                finishRaceData.isPersonalRecord = true;
                user.topScore = raceData.score;
            }
             
            UserController.saveUserData(user);

            OnSuccess?.Invoke(finishRaceData);
            
            RaycastBlockEvent.Invoke(true);
        }
    }
}
