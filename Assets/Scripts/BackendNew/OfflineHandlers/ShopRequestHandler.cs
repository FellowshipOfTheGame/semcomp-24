using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class ShopRequestHandler
    {
        public static IEnumerator GetShopUpgrades(Action<ShopUpgrades> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            UserData user = UserController.getUserData();
            
            ShopUpgrades shopUpgrades = new ShopUpgrades();
            shopUpgrades.gold = user.gold;
            shopUpgrades.shop = new ShopItem[user.upgrades.Length];

            for(int i = 0; i < user.upgrades.Length; i++)
            {
                shopUpgrades.shop[i] = new ShopItem()
                {
                    user.upgrades[i].level,
                    (user.upgrades[i].level+1) * 100,
                    user.upgrades[i].itemName,
                };
            }

            OnSuccess?.Invoke(shopUpgrades);

            RaycastBlockEvent.Invoke(false);
        }

        public static IEnumerator BuyShopUpgrade(ShopItem item, Action<NewShopItem> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            UserData user = UserController.getUserData();
           
            NewShopItem newItem = new NewShopItem();
            newItem.itemName = item.itemName;
            newItem.level = item.level+1;
            newItem.price = (item.level+1)*100;
            newItem.gold = user.gold;

            if(user.gold >= item.price){
                user.gold -= item.price;
                newItem.gold = user.gold;

                for(int i = 0; i < user.upgrades.Length; i++){
                    if(user.upgrades[i].itemName == item.itemName){
                        user.upgrades[i].level = item.level;
                        break;
                    }
                }

                UserController.saveUserData(user);
            }

            OnSuccess?.Invoke(shopItem);
            
            RaycastBlockEvent.Invoke(false);
        }
    }
}
