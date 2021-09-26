using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/ShieldPreset", fileName = "New Shield Preset")]
public class ShieldPreset : ScriptableObject
{
    public int maxHits;
    public float maxDuration;

    public override string ToString()
    {
        return $"protege contra {maxHits} batidas por {Mathf.FloorToInt(maxDuration)}s";
    }
}
