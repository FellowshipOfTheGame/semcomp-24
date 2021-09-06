using SubiNoOnibus.Networking.Requests;
using UnityEngine;

namespace SubiNoOnibus.UI
{
    public class RankingMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private PlayerRankUI[] playerRankingsUI;

        public void Open()
        {
            gameObject.SetActive(true);

            var getRankingRequest = RankingRequestHandler.GetRanking
            (
                PopulateRankingList,
                (res) => Close()
            );

            StartCoroutine(getRankingRequest);
        }

        public void Close()
        {
            gameObject.SetActive(false);
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

            //1,2 ou 3 > 3
            if(data.personal.position > playerRankingsUI.Length - 1)
            {
                var personal = data.personal;
                playerRankingsUI[playerRankingsUI.Length - 1].SetStatus(personal.name, personal.topScore, personal.position);
                playerRankingsUI[playerRankingsUI.Length - 1].IsDisplayed(true);
                playerRankingsUI[playerRankingsUI.Length - 1].IsPersonal(true);
            }
        }
    }
}
