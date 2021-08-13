using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    public const string Key = "scoreTable";

    public Transform container;
    public Transform template;
    //private List<ScoreEntry> scoreEntryList;
    private List<Transform> scoreEntryTransformList;

    private void OnEnable()
    {
        StartCoroutine(GetRanking());
    }

    private void PopulateRankingList() 
    {
        template.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString(Key);
        RankingData ranking = JsonUtility.FromJson<RankingData>(jsonString);

        scoreEntryTransformList = new List<Transform>();
        
        foreach(RankingPlayerData entry in ranking.rank) 
        {
            CreateEntry(entry, container, scoreEntryTransformList);
        }      
    }

    private IEnumerator GetRanking()
    {
        using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.Ranking_url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.result);
            PlayerPrefs.SetString(Key, request.downloadHandler.text);
            PopulateRankingList();
        }
        Debug.Log(request.downloadHandler.text);
    }

    private void CreateEntry(RankingPlayerData scoreEntry, Transform container, List<Transform> transformList) 
    {
        float templateHeight = 40f;
        int rank = transformList.Count;
        Transform entry = Instantiate(template,container);
        RectTransform entryRect = entry.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0,-templateHeight * rank);

        entry.Find("PosText").GetComponent<Text>().text = (rank+1).ToString();
        entry.Find("PontosText").GetComponent<Text>().text = scoreEntry.topScore.ToString();
        entry.Find("NomeText").GetComponent<Text>().text = scoreEntry.name;
        entry.gameObject.SetActive(true);

        entry.Find("Background").gameObject.SetActive(rank % 2 == 1);

        if(rank == 0) 
        {
            entry.Find("PosText").GetComponent<Text>().color = Color.green;
            entry.Find("PontosText").GetComponent<Text>().color = Color.green;
            entry.Find("NomeText").GetComponent<Text>().color = Color.green;
        }

        switch(rank) 
        {
            default:
                entry.Find("Trophy").gameObject.SetActive(false);
                break;
            case 0:
                break;
            case 1:
                entry.Find("Trophy").GetComponent<Image>().color = new Color32(192,192,192, 255);
                break;
            case 2:
                entry.Find("Trophy").GetComponent<Image>().color = new Color32(152,89,0, 255);
                break;
        }

        transformList.Add(entry);
    }
}
