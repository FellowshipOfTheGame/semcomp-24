using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/LightningPreset", fileName = "New Lightning Preset")]
public class LightningPreset : ScriptableObject
{
    public Vector3 size;
    public int maxTargets;
}
