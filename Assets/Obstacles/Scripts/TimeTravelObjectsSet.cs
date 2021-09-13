using UnityEngine;

[CreateAssetMenu(menuName = "TimeTravel/Objects Set", fileName = "NewTimeTravelObjectsSet")]
public class TimeTravelObjectsSet : ScriptableObject
{
    [SerializeField] private GameObject _presentObject;
    [SerializeField] private GameObject _pastObject;

    public GameObject PresentObject => _presentObject;
    public GameObject PastObject => _pastObject;
}