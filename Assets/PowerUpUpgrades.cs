using SubiNoOnibus.Networking.Requests;
using UnityEngine;

public class PowerUpUpgrades : MonoBehaviour
{
    public struct UserStatus
    {
        
    }
    
    private void Awake()
    {
        StartCoroutine(UserStatusRequestHandler.GetUserStatus(OnSuccess, OnFailure));
    }

    private void OnSuccess(UserStatus status)
    {
        
    }

    private void OnFailure()
    {
        
    }

    private void SetUpUpgrades()
    {
        
    }
    
}
