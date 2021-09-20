using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/NitroPreset", fileName = "New Nitro Preset")]
public class NitroPreset : ScriptableObject
{
    public float boost;
    public float duration;

    public override string ToString()
    {
        return $"aumenta {Mathf.FloorToInt(boost*100f)}% da velocidade máxima";
    }
}
