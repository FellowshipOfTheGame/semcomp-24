using UnityEngine;
using UnityEngine.UI;
using SubiNoOnibus.UI;

public class ShopCard : MonoBehaviour
{
    [SerializeField] private SpriteArraySO spriteArray;
    [SerializeField] private ScriptableObject[] presets;
    
    [SerializeField] private Image background;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMPro.TextMeshProUGUI itemValueTxt;
    [SerializeField] private TMPro.TextMeshProUGUI itemDescriptionTxt;
    
    private ShopItem _item;
    private ShopMenu _shopMenu;


    public bool CanBePurchased(int goldAmount) => goldAmount >= _item?.price;
    public bool IsItemLastLevel() => _item?.level >= spriteArray.sprites.Length;
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
            SetDescriptionValue(presets[index]);
        }
        catch
        {
            Debug.LogError("Correspondent sprite for item level not found");
        }
        finally
        { 
            EnableButtonInteraction( !IsItemLastLevel() );
        }
    }

    public void UpdateButtonInteraction(int goldOwned)
    {
        bool shouldEnable = CanBePurchased(goldOwned) && !IsItemLastLevel();
        EnableButtonInteraction(shouldEnable);
    }

    private void EnableButtonInteraction(bool value) => buyButton.interactable = value;
    
    private void SetDescriptionValue(ScriptableObject preset)
    {   
        itemDescriptionTxt.SetText(preset.ToString());
    }

    private void BuyItem()
    {
        _shopMenu.TryBuyItem(_item);
    }
}
