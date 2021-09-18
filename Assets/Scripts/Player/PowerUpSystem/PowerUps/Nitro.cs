using UnityEngine;

public class Nitro : PowerUp
{
    [SerializeField] public float boost;
    [SerializeField] public float duration;
    
    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        base.OnActivate(controller, renderer);
        controller.gameObject.GetComponent<NitrousSystem>().Activate(boost, duration);
        renderer.ActivateSpeedLines(duration);
    }
}
