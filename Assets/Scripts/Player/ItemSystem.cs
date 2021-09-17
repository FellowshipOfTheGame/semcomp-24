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
    private bool hasPowerUp;

    private void Start()
    {
        controller = GetComponent<VehicleController>();
        _renderer = GetComponent<VehicleRenderer>();
        iconHUD.enabled = false;
        hasPowerUp = false;
    }

    public void ActivateItem()
    {
        if (currentPowerUp is null)
            return;
        hasPowerUp = false;
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
        if (!hasPowerUp)
        {
            ReplaceItem(newPowerUp);
            hasPowerUp = true;
        }
    }
    
}
