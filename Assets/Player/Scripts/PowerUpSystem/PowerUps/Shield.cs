using UnityEngine;

public class Shield : PowerUp
{
    [SerializeField] private int protectionTimes;
    [SerializeField] private float duration;

    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
#if UNITY_EDITOR
        Debug.Log("Aquired shield");
#endif
        controller.gameObject.GetComponent<HealthSystem>().ActivateShield(protectionTimes, duration);
        renderer.ActivateShieldeEffect(duration);
    }
}
