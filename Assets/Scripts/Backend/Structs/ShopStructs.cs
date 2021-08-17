[System.Serializable]
public struct ShopUpgrades
{
    public int gold;
    public ShopItem[] shop;
}

[System.Serializable]
public struct ShopItem
{
    public int level;
    public int price;
    public string itemName;
}