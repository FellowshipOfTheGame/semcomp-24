using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenery/ObjectSet", fileName = "NewObjectSet")]
public class ObjectSet : ScriptableObject
{
    [SerializeField] private List<GameObject> members;

    public List<GameObject> Members => members;

    public GameObject GetRandom()
    {
        int randomIndex = Random.Range(0, members.Count);
        return members[randomIndex];
    }
}
