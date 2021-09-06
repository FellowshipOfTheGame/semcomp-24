using UnityEngine;

public class TestPowerUp : PowerUp
{
    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        Debug.Log("TestPowerUp Active");
        renderer.ActivateBoostEffect();
    }
}
