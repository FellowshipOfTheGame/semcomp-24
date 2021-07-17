using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    public Transform container;
    public Transform template;
    //private List<ScoreEntry> scoreEntryList;
    private List<Transform> scoreEntryTransformList;
    private void Awake() {
        template.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("scoreTable");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);

        scores.scoresList.Sort((x,y) => y.score.CompareTo(x.score));
        scoreEntryTransformList = new List<Transform>();
        
        foreach(ScoreEntry entry in scores.scoresList) {
            CreateEntry(entry,container,scoreEntryTransformList);
            if(scoreEntryTransformList.Count > 8)
                break;
        }      
    }

    private void CreateEntry(ScoreEntry scoreEntry,Transform container,List<Transform> transformList) {
        float templateHeight = 40f;
        int rank = transformList.Count;
        Transform entry = Instantiate(template,container);
        RectTransform entryRect = entry.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0,-templateHeight * rank);

        entry.Find("PosText").GetComponent<Text>().text = (rank+1).ToString();
        entry.Find("PontosText").GetComponent<Text>().text = scoreEntry.score.ToString();
        entry.Find("NomeText").GetComponent<Text>().text = scoreEntry.name;
        entry.gameObject.SetActive(true);

        entry.Find("Background").gameObject.SetActive(rank % 2 == 1);

        if(rank == 0) {
            entry.Find("PosText").GetComponent<Text>().color = Color.green;
            entry.Find("PontosText").GetComponent<Text>().color = Color.green;
            entry.Find("NomeText").GetComponent<Text>().color = Color.green;
        }

        switch(rank) {
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

    private void AddEntry(int score, string name) {
        ScoreEntry entry = new ScoreEntry { score = score,name = name };

        string jsonString = PlayerPrefs.GetString("scoreTable");
        Scores scores = JsonUtility.FromJson<Scores>(jsonString);

        scores.scoresList.Add(entry);

        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("scoreTable",json);
        PlayerPrefs.Save();
    }

    private class Scores {
        public List<ScoreEntry> scoresList;
    }
}
