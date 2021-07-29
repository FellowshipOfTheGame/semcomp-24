using UnityEngine;

// Parameters used in Obstacle scripts

[CreateAssetMenu(menuName = "Obstacle/Obstacle Properties Preset", fileName = "NewObstaclePropertiesPreset")]
public class ObstaclePropertiesPreset : ScriptableObject
{
    [SerializeField] [Tooltip("Base damage dealt by the obstacle")]
    private int baseDamage = 3;
    
    [SerializeField] [Tooltip("The force applied against the player on collision (speed decrease)")]
    private int vehicleSpeedDecrease = 50;
    
    [SerializeField] [Range(0, 5)] [Tooltip("Seconds until the obstacle is destroyed after collision with player")]
    private float destroyCountdownPlayer = 1f;
    
    [SerializeField] [Range(0, 10)] [Tooltip("Seconds until the obstacle is destroyed after collision with another obstacle")]
    private float destroyCountdownObstacle = 3f;

    public int GetBaseDamage()
    {
        return baseDamage;
    }
    
    public int GetVehicleSpeedDecrease()
    {
        return vehicleSpeedDecrease;
    }

    public float GetDestroyCountdownPlayer()
    {
        return destroyCountdownPlayer;
    }
    
    public float GetDestroyCountdownObstacle()
    {
        return destroyCountdownObstacle;
    }
}
