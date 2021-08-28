[System.Serializable]
public struct RankingPlayerData
{
    public string name;
    public string nickname;
    public int topScore;
}

[System.Serializable]
public struct RankingData
{
    public RankingPlayerData[] rank;
}
