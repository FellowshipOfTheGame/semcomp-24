using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSystem : MonoBehaviour
{
    [SerializeField] private Image iconHUD;
    private VehicleController controller;
    private VehicleRenderer _renderer;
    
    public delegate void OnPowerUpActive(PowerUp powerUp);
    public event OnPowerUpActive OnPowerUpActiveEvent;
    
    private PowerUp currentPowerUp;

    private void Start()
    {
        controller = GetComponent<VehicleController>();
        _renderer = GetComponent<VehicleRenderer>();
        iconHUD.enabled = false;
    }

    public void ActivateItem()
    {
        if (currentPowerUp is null)
            return;

        currentPowerUp.OnActivate(controller, _renderer);
        OnPowerUpActiveEvent?.Invoke(currentPowerUp);
        currentPowerUp = null;
        iconHUD.enabled = false;
    }

    public void ReplaceItem(PowerUp newPowerUp)
    {
        currentPowerUp = newPowerUp;
        iconHUD.sprite = newPowerUp.icon;
        iconHUD.enabled = true;
    }

    public void ReplaceIfNull(PowerUp newPowerUp)
    {
        if (currentPowerUp == null)
        {
            ReplaceItem(newPowerUp);
        }
#if  UNITY_EDITOR
        else
        {
            Debug.Log("Could not replace");
        }
#endif
    }
    
}
