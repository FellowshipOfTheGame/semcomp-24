using UnityEngine;

[CreateAssetMenu(menuName = "LevelingPreset/BusStopPreset", fileName = "New Bus Stop Preset")]
public class BusStopPreset : ScriptableObject
{
    public int healAmount;

    public override string ToString()
    {
        return $"recupera {healAmount} pontos de vida";
    }

}
