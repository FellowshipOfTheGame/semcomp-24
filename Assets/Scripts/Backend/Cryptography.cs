using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SubiNoOnibus.Networking
{
    public static class Cryptography
    {
        public static string request_signature_key;
        public static string response_signature_key;

        public static string GetSignature(RaceData raceData)
        {
            raceData.sign = string.Empty;
            return GetSignature(raceData, request_signature_key);
        }

        public static string GetSignature(object data, string signature_key)
        {
            string signature = string.Empty;
            byte[] unicodeKey = Encoding.UTF8.GetBytes(signature_key);
            using (HMACSHA256 hmacSha256 = new HMACSHA256(unicodeKey))
            {
                string signString = JsonUtility.ToJson(data);
                byte[] dataToHmac = Encoding.UTF8.GetBytes(signString);
                signature = Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }

            return signature;
        }

        public static bool IsSignatureValid(UserStatus userStatus)
        {
            string incomingSignature = userStatus.sign;
            userStatus.sign = string.Empty;

            string ourSignature = GetSignature(userStatus, response_signature_key);
            
            return incomingSignature == ourSignature;
        }
    }
}
