using System.Collections.Generic;
using UnityEngine;

public class SceneryGenerator : Generator
{
    [Space(10)]
    [Header("Scenery")]

    [SerializeField] private Segment segment;
    [SerializeField] private Segment sceneryTrainSegment;

    [Space(10)]
    [Header("Level")]
    
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private List<Segment> levelTrainSegments;

    protected override Segment GetNext()
    {
        if (levelTrainSegments.Contains(levelGenerator.currentSegment))
        {
            return sceneryTrainSegment;
        }

        return segment;
    }
}
