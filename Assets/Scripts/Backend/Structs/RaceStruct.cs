[System.Serializable]
public struct RaceData
{
    public int score;
    public int gold;
    public string nonce;
    public string sign;
}

[System.Serializable]
public struct FinishRaceData
{
    public bool isPersonalRecord;
}