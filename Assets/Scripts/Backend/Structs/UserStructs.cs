public struct UserStatus
{
    public string message;
    public string name;
    public int gold;
    public int runs;
    public int topScore;
    public PowerUpUpgrade[] upgrades;
    public string sign;
}

[System.Serializable]
public struct PowerUpUpgrade
{
    public string itemName;
    public int level;
}