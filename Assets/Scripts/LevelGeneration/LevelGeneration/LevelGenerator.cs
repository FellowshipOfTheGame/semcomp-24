using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Generator
{
    public List<WeightedSet> sets;
    
    [Serializable]
    public class WeightedSet
    {
        public List<Segment> segments;
        public float weight;
    }

    protected override Segment GetNext()
    {
        float weightSum = 0f;
        foreach (WeightedSet set in sets)
        {
            weightSum += (set.weight > 0f) ? set.weight : 0f;
        }
        float randomNumber = UnityEngine.Random.Range(0f, weightSum);
        float cumulative = 0f;
        int index;
        
        for (index = 0; index < sets.Count; index++)
        {
            cumulative += sets[index].weight;
            if (randomNumber <= cumulative)
            {
                break;
            }
        }

        List<Segment> segments = sets[index].segments;
        int randomSegment = UnityEngine.Random.Range(0, segments.Count);
        return segments[randomSegment];
    }
}
