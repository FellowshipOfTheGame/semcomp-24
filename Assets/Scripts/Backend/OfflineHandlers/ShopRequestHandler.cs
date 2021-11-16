using System;
using System.Collections;
using UnityEngine.Networking;
using SubiNoOnibus.Backend.Offline.Utils;

namespace SubiNoOnibus.Backend.Offline.Requests
{
    public static class ShopRequestHandler
    {
        public static IEnumerator GetShopUpgrades(Action<ShopUpgrades> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            UserStatus user = UserController.GetUserData();
            
            ShopUpgrades shopUpgrades = new ShopUpgrades();
            shopUpgrades.gold = user.gold;
            shopUpgrades.shop = new ShopItem[user.upgrades.Length];

            for(int i = 0; i < user.upgrades.Length; i++)
            {
                shopUpgrades.shop[i] = new ShopItem()
                {
                    level = user.upgrades[i].level,
                    price = (user.upgrades[i].level) * 100,
                    itemName = user.upgrades[i].itemName,
                };
            }

            OnSuccess?.Invoke(shopUpgrades);
            
            yield return null;
        }

        public static IEnumerator BuyShopUpgrade(ShopItem item, Action<NewShopItem> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            UserStatus user = UserController.GetUserData();
           
            NewShopItem newItem = new NewShopItem();
            newItem.itemName = item.itemName;
            newItem.level = item.level;
            newItem.price = item.price;
            newItem.gold  = user.gold;

            if(user.gold >= item.price){
                for(int i = 0; i < user.upgrades.Length; i++){
                    if( String.Equals(user.upgrades[i].itemName, item.itemName)){
                        user.gold    -= item.price;
                        newItem.gold  = user.gold;
                        
                        newItem.level = item.level+1;
                        newItem.price = (newItem.level)*100;    
                        
                        user.upgrades[i].level += 1;
                        break;
                    }
                }

                UserController.SaveUserData(user);
            }

            OnSuccess?.Invoke(newItem);
            
            yield return null;
        }
    }
}
