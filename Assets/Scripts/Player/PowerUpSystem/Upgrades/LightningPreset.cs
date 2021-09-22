using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/LightningPreset", fileName = "New Lightning Preset")]
public class LightningPreset : ScriptableObject
{
    public Vector3 size;
    public int maxTargets;

    public override string ToString()
    {
        return $"destr�i {maxTargets} obst�culos em at� {Mathf.FloorToInt(size.z)}m";
    }
}
