using UnityEngine;

/**
 * Constants used for math by VehicleController
 */
[CreateAssetMenu(menuName = "Vehicle/VehicleStatPreset", fileName = "NewVehicleStatPreset")]
public class VehicleStatPreset : ScriptableObject
{
    public float speed;
    public float handling;
}
