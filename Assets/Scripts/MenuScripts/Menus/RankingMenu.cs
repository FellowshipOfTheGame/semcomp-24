using SubiNoOnibus.Backend;
using UnityEngine;

#if SUBI_NO_ONIBUS_ONLINE
using Backend = SubiNoOnibus.Backend.Online.Requests;
#else
using Backend = SubiNoOnibus.Backend.Offline.Requests;
#endif

namespace SubiNoOnibus.UI
{
    public class RankingMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private PlayerRankUI[] playerRankingsUI;

        public void Open()
        {
            var getRankingRequest = Backend::RankingRequestHandler.GetRanking
            (
                PopulateRankingList,
                (req) => DefaultErrorHandling.OnMenuScene(req, this, FindObjectOfType<MainMenu>())
            );

            StartCoroutine(getRankingRequest);
        }

        public void Close()
        {
            EnableUI(false);
        }


        public void PopulateRankingList(RankingData data)
        {
            foreach(var player in playerRankingsUI)
            {
                player.IsDisplayed(false);
            }

            for(int i = 0; i < data.rank.Length && i < playerRankingsUI.Length - 1; i++)
            {
                RankingPlayerData playerData = data.rank[i];
                playerRankingsUI[i].SetStatus(playerData.name, playerData.topScore, i + 1);
                
                bool isPersonal = (i+1 == data.personal.position);
                playerRankingsUI[i].IsPersonal(isPersonal);
                playerRankingsUI[i].IsDisplayed(true);
            }
            
            if(data.personal.position > playerRankingsUI.Length - 1)
            {
                var personal = data.personal;
                playerRankingsUI[playerRankingsUI.Length - 1].SetStatus(personal.name, personal.topScore, personal.position);
                playerRankingsUI[playerRankingsUI.Length - 1].IsDisplayed(true);
                playerRankingsUI[playerRankingsUI.Length - 1].IsPersonal(true);
            }

            EnableUI(true);
        }

        private void EnableUI(bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }
        }
    }
}
