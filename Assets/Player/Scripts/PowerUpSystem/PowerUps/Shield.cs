using UnityEngine;

public class Shield : PowerUp
{
    [SerializeField] public int protectionTimes;
    [SerializeField] public float duration;

    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
#if UNITY_EDITOR
        Debug.Log("Aquired shield");
#endif
        controller.gameObject.GetComponent<HealthSystem>().ActivateShield(protectionTimes, duration);
        renderer.ActivateShieldeEffect(duration);
    }
}
