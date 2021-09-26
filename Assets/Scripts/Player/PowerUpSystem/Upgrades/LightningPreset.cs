using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/LightningPreset", fileName = "New Lightning Preset")]
public class LightningPreset : ScriptableObject
{
    public Vector3 size;
    public int maxTargets;

    public override string ToString()
    {
        // return $"destrói {maxTargets} obstáculos em até {Mathf.FloorToInt(size.z)}m";
        return $"destr\u00F3i {maxTargets} obst\u00E1culos em at\u00E9 {Mathf.FloorToInt(size.z)}m";
    }
}
