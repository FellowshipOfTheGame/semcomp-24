public struct UserStatus
{
    public string message;
    public string name;
    public int gold;
    public int runs;
    public string topScore;
    public PowerUpUpgrade[] upgrades;
    public string sign;
}

public struct PowerUpUpgrade
{
    public string itemName;
    public int level;
}