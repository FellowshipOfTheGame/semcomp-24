using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/PlayerHealthPreset", fileName = "New Player Health Preset")]
public class PlayerHealthPreset : ScriptableObject
{
    public int starterHealth;

    public override string ToString()
    {
        return $"{starterHealth} pontos de vida";
    }
}
