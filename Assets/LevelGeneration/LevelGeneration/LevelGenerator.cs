using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Generator
{
    public List<WeightedSet> sets;
    private float[] cumulativeTotal;
    
    [Serializable]
    public class WeightedSet
    {
        public List<Segment> segments;
        public float weight;
    }

    private void Awake()
    {
        if (sets.Count <= 0)
            return;

        cumulativeTotal = new float[sets.Count];
        cumulativeTotal[0] = sets[0].weight;

        for (int i = 1; i < sets.Count; i++)
        {
            cumulativeTotal[i] = cumulativeTotal[i - 1] + sets[i].weight;
        }
    }

    protected override Segment GetNext()
    {
        float randomNumber = UnityEngine.Random.Range(0f, cumulativeTotal[sets.Count - 1]);
        int index;
        
        for (index = 0; index < sets.Count; index++)
        {
            if (randomNumber <= cumulativeTotal[index]) break;
        }

        List<Segment> segments = sets[index].segments;
        int randomSegment = UnityEngine.Random.Range(0, segments.Count);
        return segments[randomSegment];
    }
}
