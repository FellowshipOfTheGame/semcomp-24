[System.Serializable]
public struct RankingPlayer
{
    public string name;
    public string nickname;
    public int topScore;
}

[System.Serializable]
public struct Ranking
{
    public RankingPlayer[] rank;
}
