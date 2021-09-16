using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/ShieldPreset", fileName = "New Shield Preset")]
public class ShieldPreset : ScriptableObject
{
    public int maxHits;
    public float maxDuration;
}
