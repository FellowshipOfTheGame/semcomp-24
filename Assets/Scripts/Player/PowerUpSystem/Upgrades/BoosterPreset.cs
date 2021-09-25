using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/BoosterPreset", fileName = "New Booster Preset")]
public class BoosterPreset : ScriptableObject
{
    public float boost;
    public float duration;

    public override string ToString()
    {
        //return $"aumenta a aceleração por {Mathf.FloorToInt(duration)}s";
        return $"aumenta a acelera\u00E7\u00E3o por {Mathf.FloorToInt(duration)}s";
    }
}
