[System.Serializable]
public struct ShopUpgrades
{
    public int gold;
    public ShopItem[] shop;
}

[System.Serializable]
public class ShopItem
{
    public int level;
    public int price;
    public string itemName;

    public override string ToString()
    {
        return $"{itemName} ({level}): {price}";
    }
}

[System.Serializable]
public class NewShopItem : ShopItem
{
    public int gold;
}