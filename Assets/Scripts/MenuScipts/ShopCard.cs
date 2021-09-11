using UnityEngine;
using UnityEngine.UI;
using SubiNoOnibus.UI;
using UnityEngine.Events;

public class ShopCard : MonoBehaviour
{
    [SerializeField] private SpriteArraySO spriteArray;
    
    [SerializeField] private Image background;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMPro.TextMeshProUGUI itemValueTxt;
    
    private ShopItem _item;
    private ShopMenu _shopMenu;

    public void SetShopMenu(ShopMenu shopMenu)
    {
        _shopMenu = shopMenu;
        buyButton.onClick.AddListener(BuyItem);
    }

    public void SetShopItem(ShopItem item)
    {
        _item = item;
        itemValueTxt.SetText(item.price.ToString());

        int index = item.level - 1;
        try
        {
            background.sprite = spriteArray.sprites[index];
        }
        catch
        {
            Debug.LogError("Correspondent sprite for item level not found");
        }
    }

    private void BuyItem()
    {
        _shopMenu.TryBuyItem(_item);
    }
}
