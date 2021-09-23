using UnityEngine;

namespace SubiNoOnibus.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Instructions")]
    public class InstructionsCard : ScriptableObject
    {
        public InstructionCard[] instructions;

        public bool TryGetAt(int index, out InstructionCard? card)
        {
            if (index >= 0 && index < instructions.Length)
            {
                card = instructions[index];
                return true;
            }
            else
            {
                card = null;
                return false;
            }
        }
    }

    [System.Serializable]
    public struct InstructionCard
    {
        public string text;
        public Sprite sprite;
    }
}
