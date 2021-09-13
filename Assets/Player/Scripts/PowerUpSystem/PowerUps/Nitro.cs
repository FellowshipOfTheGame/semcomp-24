using UnityEngine;

public class Nitro : PowerUp
{
    [SerializeField] private float boost;
    [SerializeField] private float duration;
    
    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        controller.gameObject.GetComponent<NitrousSystem>().Activate(boost, duration);
    }
}
