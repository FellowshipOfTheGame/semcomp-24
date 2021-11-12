using UnityEngine;

namespace SubiNoOnibus.Backend.Offline.Utils
{
    public struct UserController { 

        private static string _storageKey = 'localuserdata';

        public static UserStatus getUserData()
        {    
            string storedData = PlayerPrefs.GetString(UserController._storageKey, "");
            
            if(string.IsNullOrEmpty(storedData))
            {
                UserStatus newUser = new UserStatus();
                newUser.name = "Jogador";
                newUser.gold = 0;
                newUser.runs = 0;
                newUser.topScore = 0;
                newUser.upgrades = new PowerUpUpgrade[]{
                    new PowerUpUpgrade() { 'Max_Life', 0 },
                    new PowerUpUpgrade() { 'Booster', 0 },
                    new PowerUpUpgrade() { 'Nitro', 0 },
                    new PowerUpUpgrade() { 'Bus_Stop', 0 },
                    new PowerUpUpgrade() { 'Shield', 0 },
                    new PowerUpUpgrade() { 'Laser', 0 }
                };
                newUser.sign = "";
                newUser.message = "";

                UserController.saveUserData(newUser);
                return newUser;
            }
            
            return JsonUtility.FromJson<UserStatus>(storedData);
        }

        public static void saveUserData(UserStatus userStatus)
        { 
            PlayerPrefs.SetString(UserController._storageKey, JsonUtility.ToJson(userStatus));
        }
    }
}