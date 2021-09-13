using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    [SerializeField] protected Transform follow;
    [SerializeField] protected float threshold;
    [SerializeField] protected Vector3 area;
    private Vector3 nextPosition;
    [SerializeField] private GameObject road;
    [SerializeField] private float roadHeight;
    [SerializeField] private Segment starterSegment;
    
    public static List<GameObject> LoadedObjects { get; private set; }
    public List<GameObject> LoadedRoads { get; private set; }

    protected abstract Segment GetNext();

    private void Start()
    {
        LoadedObjects = new List<GameObject>();
        LoadedRoads = new List<GameObject>();
        
        nextPosition = follow.position;
        BuildNext(starterSegment);
        UpdateNextPosition();
        
        StartCoroutine(ClearObjectsListEnumerator());
    }

    private void Update()
    {
        if (CheckThreshold())
        {
            BuildNext(GetNext());
            UpdateNextPosition();
        }

#if UNITY_EDITOR
        Vector3 minNext = new Vector3(nextPosition.x - area.x / 2, 0, nextPosition.z - area.z / 2);
        Vector3 maxNext = new Vector3(nextPosition.x + area.x / 2, 0, nextPosition.z+ area.z / 2);
        Debug.DrawLine(new Vector3(minNext.x, 1f, maxNext.z), new Vector3(maxNext.x, 1f, maxNext.z), Color.green);
        Debug.DrawLine(new Vector3(maxNext.x, 1f, maxNext.z), new Vector3(maxNext.x, 1f, minNext.z), Color.green);
        Debug.DrawLine(new Vector3(maxNext.x, 1f, minNext.z), new Vector3(minNext.x, 1f, minNext.z), Color.green);
        Debug.DrawLine(new Vector3(minNext.x, 1f, minNext.z), new Vector3(minNext.x, 1f, maxNext.z), Color.green);
#endif
    }

    private void BuildNext(Segment nextSegment)
    {
        Vector3 minNext = nextPosition - (area / 2);
        Vector3 maxNext = nextPosition + (area / 2);
        Vector3 offset = new Vector3(area.x / nextSegment.Width, area.y, area.z / nextSegment.Length);

        Vector3 startPosition = new Vector3(minNext.x + offset.x / 2f, nextPosition.y - roadHeight / 2, maxNext.z - offset.z / 2f);
        Vector3 currentPosition = startPosition;

        GameObject roadObject = Instantiate(road, new Vector3(nextPosition.x, nextPosition.y - roadHeight, nextPosition.z), Quaternion.identity);
        LoadedRoads.Add(roadObject);

        for (int i = 0; i < nextSegment.Length; i++)
        {
            for (int j = 0; j < nextSegment.Width; j++)
            {
                ObjectSet set = nextSegment.GetSetAt(j, i);
                if (set is null)
                {
                    currentPosition.x += offset.x;
                    continue;
                }
                
                GameObject prefab = set.GetRandom();

                // Swap objects according to current period of time 
                
                if (prefab.TryGetComponent(out SwapOnTimeTravel prefabSwap))
                {
                    prefab = TimeTravel.InThePast ? prefabSwap.PastObject : prefabSwap.PresentObject;
                }

                GameObject instance = Instantiate(prefab, currentPosition, prefab.transform.rotation);

                LoadedObjects.Add(instance);
                currentPosition.x += offset.x;
            }

            currentPosition.x = startPosition.x;
            currentPosition.z -= offset.z;
        }
    }
    
    private void UpdateNextPosition()
    {
        nextPosition += Vector3.forward * area.z;
        Debug.Log(nextPosition);
    }
    
    private bool CheckThreshold()
    {
        return Vector3.Distance(follow.position, nextPosition) <= threshold;
    }

    private IEnumerator ClearObjectsListEnumerator()
    {
        yield return new WaitForSeconds(10f);
        
        ClearObjectsList(LoadedObjects);
        ClearObjectsList(LoadedRoads);

        StartCoroutine(ClearObjectsListEnumerator());
    }

    private void ClearObjectsList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }
            
            if (follow.position.z - list[i].transform.position.z >= threshold)
            {
                GameObject obj = list[i];
                list.Remove(list[i]);
                Destroy(obj);
            }
        }
    }
}
