using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [System.Serializable] class ShopItem {
        public Sprite Image;
        public string Title;
        public string subTitle;
        public int Price;
        public bool isPurchased = false;
    }

    [SerializeField] List<ShopItem> ShopItemsList;
    public int Coins;
    GameObject ItemTemplate, g;
    Button buyButton;
    [SerializeField] Transform ShopScrollView;
    [SerializeField] Text CoinsText;
    // Start is called before the first frame update
    void Start()
    {
        int len = ShopItemsList.Count;
        ItemTemplate = ShopScrollView.GetChild(0).gameObject;  
        for(int i = 0; i < len;i++) 
        {
            g = Instantiate(ItemTemplate,ShopScrollView);
            g.transform.GetChild(0).GetComponent<Image>().sprite = ShopItemsList[i].Image;
            g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].Price.ToString();
            g.transform.GetChild(2).GetComponent<Button>().interactable = !ShopItemsList[i].isPurchased;
            g.transform.GetChild(3).GetComponent<Text>().text = ShopItemsList[i].Title;
            g.transform.GetChild(4).GetComponent<Text>().text = ShopItemsList[i].subTitle;
            buyButton = g.transform.GetChild(2).GetComponent<Button>();
            buyButton.interactable = !ShopItemsList[i].isPurchased;
            buyButton.AddEventListener(i,OnShopItemBtnClicked);
        }
        SetCoinsUI();
        Destroy(ItemTemplate);
    }

    void OnShopItemBtnClicked(int itemIndex) 
    {
        if(HasEnoughCoins(ShopItemsList[itemIndex].Price))
        {
            UseCoins(ShopItemsList[itemIndex].Price);
            ShopItemsList[itemIndex].isPurchased = true;
            buyButton = ShopScrollView.GetChild(itemIndex).GetChild(2).GetComponent<Button>();
            buyButton.interactable = false;
            buyButton.transform.GetChild(0).GetComponent<Text>().text = "PURCHASED";
            buyButton.transform.GetChild(0).GetComponent<Text>().fontSize = 16;
            SetCoinsUI();
        }
    }

    void SetCoinsUI()
    {
        CoinsText.text = Coins.ToString();
    }

    public void UseCoins(int amount) 
    {
        Coins -= amount;
    }

    public bool HasEnoughCoins(int amount) 
    {
        return (Coins >= amount);
    }
}