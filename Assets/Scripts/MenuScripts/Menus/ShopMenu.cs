using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Networking.Requests;
using System.Collections.Generic;

namespace SubiNoOnibus.UI
{
    public class ShopMenu : MonoBehaviour, IMenu
    {
        public int Gold { get => _gold;  private set => UpdateGoldAmount(value); }
        
        [SerializeField] private PopupMessageWindow logInterface;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private TMPro.TextMeshProUGUI goldAmountTxt;

        [SerializeField]
        private ShopUpgrades _shopUpgrades;
        private int _gold = 0;
        private Dictionary<string, ShopCard> _shopCards;

        public void Open()
        {
            IEnumerator getShopUpgrades = ShopRequestHandler.GetShopUpgrades
            (
                HandleGetShopUpgradesSuccess, 
                HandleError
            );
            StartCoroutine(getShopUpgrades);
        }

        public void Close()
        {
            EnableVisuals(false);
        }

        public void TryBuyItem(ShopItem item)
        {
            IEnumerator buyShopUpgrade = ShopRequestHandler.BuyShopUpgrade
            (
                item,
                HandleBuyUpgradeSuccess,
                HandleError
            );
            StartCoroutine(buyShopUpgrade);
        }

        private void EnableVisuals(bool value)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }
        }
        private void HandleBuyUpgradeSuccess(NewShopItem newItem)
        {
            if(_shopCards.TryGetValue(newItem.itemName, out ShopCard card))
                card.SetShopItem(newItem);
            else
                Debug.LogWarning("Incoming item with no correspondent ShopCard: " + newItem);

            Gold = newItem.gold;
        }

        private void HandleGetShopUpgradesSuccess(ShopUpgrades shopUpgrades)
        {
            _shopUpgrades = shopUpgrades;
            PopulateShopList();
            EnableVisuals(true);
        }

        private void HandleError(UnityWebRequest request)
        {
            string errorMsg = JsonUtility.FromJson<ErrorMessageData>(request.downloadHandler.text);
            logInterface.LogError(errorMsg);
        }

        private void PopulateShopList()
        {
            foreach (ShopItem item in _shopUpgrades.shop)
            {
                if (_shopCards.TryGetValue(item.itemName, out ShopCard card))
                {
                    card.SetShopItem(item);
                }
                else
                {
                    Debug.LogWarning("Incoming item with no correspondent ShopCard: " + item);
                }
            }
            
            Gold = _shopUpgrades.gold;
        }

        private void UpdateGoldAmount(int amount)
        {
            _gold = amount;
            goldAmountTxt.SetText(_gold.ToString());
            UpdateBuyButtons();
        }
        
        private void UpdateBuyButtons()
        {
            foreach(var shopCard in _shopCards.Values)
            {
                shopCard.UpdateButtonInteraction(Gold);
            }
        }

        private void Awake()
        {
            PopulateShopCardDictionary();
        }

        private void PopulateShopCardDictionary()
        {
            _shopCards = new Dictionary<string, ShopCard>(cardsContainer.childCount);
            
            foreach(Transform card in cardsContainer)
            {
                ShopCard shopCard = card.GetComponent<ShopCard>();
                
                shopCard.SetShopMenu(this);
                _shopCards[card.name] = shopCard;
            }
        }

    }
}