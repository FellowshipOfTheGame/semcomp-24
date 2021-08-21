using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SubiNoOnibus.UI
{
    public class ShopMenu : MonoBehaviour
    {
        [SerializeField] private PopupMessageWindow logInterface;

        private ShopUpgrades shopUpgrades;
        private int gold;

        private void OnEnable()
        {
            StartCoroutine(GetShopUpgrades());
        }

        private IEnumerator GetShopUpgrades()
        {
            using UnityWebRequest request = WebRequestFactory.AuthGetJson(Endpoints.Shop_url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                HandleGetShopUpgradesError(request);
            }
            else
            {
                Debug.Log(request.result);
                Debug.Log(request.downloadHandler.text);

                shopUpgrades = JsonUtility.FromJson<ShopUpgrades>(request.downloadHandler.text);
                PopulateShopList();
            }
            Debug.Log(request.downloadHandler.text);
        }

        private void HandleGetShopUpgradesError(UnityWebRequest request)
        {
            string errorMsg = JsonUtility.FromJson<ErrorMessageData>(request.downloadHandler.text);

            if (request.responseCode == 401)
            {

            }
            else if (request.responseCode == 404)
            {

            }

            logInterface.LogError(errorMsg);
        }

        private void PopulateShopList()
        {
            gold = shopUpgrades.gold;
            Debug.Log("gold: " + gold);
            foreach (var item in shopUpgrades.shop)
            {
                Debug.Log($"{item.itemName} ({item.level}): {item.price}");
            }
        }
    }
}