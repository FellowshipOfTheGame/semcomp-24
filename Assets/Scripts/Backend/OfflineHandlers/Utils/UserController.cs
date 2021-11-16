using UnityEngine;

namespace SubiNoOnibus.Backend.Offline.Utils
{
    public struct UserController { 

        private const string _storageKey = "localuserdata";

        public static UserStatus GetUserData()
        {    
            string storedData = PlayerPrefs.GetString(_storageKey, "");
            
            if(string.IsNullOrEmpty(storedData))
            {
                UserStatus newUser = new UserStatus();
                newUser.name = "Jogador";
                newUser.gold = 0;
                newUser.runs = 0;
                newUser.topScore = 0;
                newUser.upgrades = new PowerUpUpgrade[]
                {
                    new PowerUpUpgrade() { itemName = "Max_Life", level = 1 },
                    new PowerUpUpgrade() { itemName = "Booster", level = 1 },
                    new PowerUpUpgrade() { itemName = "Nitro", level = 1 },
                    new PowerUpUpgrade() { itemName = "Bus_Stop", level = 1 },
                    new PowerUpUpgrade() { itemName = "Shield", level = 1 },
                    new PowerUpUpgrade() { itemName = "Laser", level = 1 }
                };
                newUser.sign = "";
                newUser.message = "";

                SaveUserData(newUser);
                return newUser;
            }
            
            return JsonUtility.FromJson<UserStatus>(storedData);
        }

        public static void SaveUserData(UserStatus userStatus)
        { 
            PlayerPrefs.SetString(_storageKey, JsonUtility.ToJson(userStatus));
        }
    }
}