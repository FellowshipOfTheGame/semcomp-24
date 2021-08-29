using UnityEngine;
using SubiNoOnibus.Networking.Requests;

public class RaceStartTest : MonoBehaviour
{
    [SerializeField] private RaceData raceData;

    [ContextMenu("FinishRace")]
    public void StartCoroutineFinishRace()
    {
        var finishRaceEnumerator = RaceRequestHandler.FinishRace(raceData, 
            () => Debug.Log("Success"), 
            (req) =>
            {
                Debug.Log(req.responseCode);
            }
        );
        StartCoroutine(finishRaceEnumerator);
    }

    [ContextMenu("StartRace")]
    public void StartCoroutineStartRace()
    {
        var startRaceEnumerator = RaceRequestHandler.StartRace(
            (raceData) => this.raceData = raceData,
            (req) => Debug.Log(req.error)
        );
        StartCoroutine(startRaceEnumerator);
    }
}
