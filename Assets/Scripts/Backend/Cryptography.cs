using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SubiNoOnibus.Networking
{
    public static class Cryptography
    {
        public static string request_signature_key;
        //public static string response_signature_key;

        public static void SetSignature(RaceData raceData)
        {
            raceData.sign = string.Empty;

            string signature = string.Empty;
            byte[] unicodeKey = Encoding.UTF8.GetBytes(request_signature_key);
            using (HMACSHA256 hmacSha256 = new HMACSHA256(unicodeKey))
            {
                string signString = JsonUtility.ToJson(raceData);
                byte[] dataToHmac = Encoding.UTF8.GetBytes(signString);
                signature = Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }

            raceData.sign = signature;
        }
    }
}
