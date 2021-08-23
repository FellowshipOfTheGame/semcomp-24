using System;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    [SerializeField] protected Transform follow;
    [SerializeField] protected float threshold;
    [SerializeField] protected Vector3 area;
    private Vector3 nextPosition;

    protected abstract Segment GetNext();

    private void Start()
    {
        nextPosition = follow.position;
    }

    private void Update()
    {
        if (CheckThreshold())
        {
            BuildNext();
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

    private void BuildNext()
    {
        Segment nextSegment = GetNext();

        Vector3 minNext = nextPosition - (area / 2);
        Vector3 maxNext = nextPosition + (area / 2);
        Vector3 offset = new Vector3(area.x / nextSegment.Width, area.y, area.z / nextSegment.Length);

        Vector3 startPosition = new Vector3(minNext.x + offset.x, nextPosition.y, maxNext.z - offset.z);
        Vector3 currentPosition = startPosition;

        for (int i = 0; i < nextSegment.Width; i++)
        {
            for (int j = 0; j < nextSegment.Length; j++)
            {
                ObjectSet set = nextSegment.GetSetAt(j, i);
                if (set is null)
                {
                    currentPosition.x += offset.x;
                    continue;
                }

                GameObject prefab = set.GetRandom();
                GameObject instance = Instantiate(prefab, currentPosition, Quaternion.identity);
                Destroy(instance, 15); // TODO Pooling
                currentPosition.x += offset.x;
            }

            currentPosition.x = startPosition.x;
            currentPosition.z -= offset.z;
        }
    }
    
    private void UpdateNextPosition()
    {
        nextPosition += Vector3.forward * area.z;
    }
    
    private bool CheckThreshold()
    {
        return Vector3.Distance(follow.position, nextPosition) <= threshold;
    }

}
