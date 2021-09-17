using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemcompinhoCounter : MonoBehaviour
{
    [SerializeField] private RaceManager raceManager;

    public void Increment()
    {
        raceManager.AddCoin(1);
    }
    
}
