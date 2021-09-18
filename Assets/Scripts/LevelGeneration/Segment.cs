using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenery/Segment", fileName = "NewSegment")]
public class Segment : ScriptableObject
{
    [SerializeField] public int trainSegment;
    [SerializeField] [Min(1)] private int width = 3;
    [SerializeField] [Min(1)] private int length = 2;
    [SerializeField] private ObjectSet[] segment = new ObjectSet[6];

    public int Width => width;
    public int Length => length;
    
    public ObjectSet GetSetAt(int x, int y)
    {
        if (x >= width || x < 0 || y >= length || y < 0)
        {
            return null;
        }

        return segment[y * width + x];
    }
}
