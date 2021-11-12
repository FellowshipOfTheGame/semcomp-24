using UnityEngine;

namespace SubiNoOnibus
{
    public class SemcompinhoCounter : MonoBehaviour
    {
        [SerializeField] private RaceManager raceManager;

        public void Increment()
        {
            raceManager.AddCoin(1);
        }

    }
}