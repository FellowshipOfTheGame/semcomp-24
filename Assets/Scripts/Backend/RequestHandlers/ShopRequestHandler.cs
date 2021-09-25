using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.Networking.Requests
{
    public static class ShopRequestHandler
    {
        public static IEnumerator GetShopUpgrades(Action<ShopUpgrades> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            using UnityWebRequest request = WebRequestFactory.AuthGet(Endpoints.Shop_url);

            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);
            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UserAuthRequestHandler.SaveAuthCookie(request);
                var shopUpgrades = JsonUtility.FromJson<ShopUpgrades>(request.downloadHandler.text);
                OnSuccess?.Invoke(shopUpgrades);
            }
        }

        public static IEnumerator BuyShopUpgrade(ShopItem item, Action<NewShopItem> OnSuccess, Action<UnityWebRequest> OnFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            string data = JsonUtility.ToJson(item);
            using UnityWebRequest request = WebRequestFactory.AuthPostJson(Endpoints.Shop_buy_url, data);

            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);
            if (request.result != UnityWebRequest.Result.Success)
            {
                OnFailure?.Invoke(request);
            }
            else
            {
                UserAuthRequestHandler.SaveAuthCookie(request);
                var shopItem = JsonUtility.FromJson<NewShopItem>(request.downloadHandler.text);
                OnSuccess?.Invoke(shopItem);
            }
        }
    }
}
