using System.Collections.Generic;
using UnityEngine;

public class QueueGenerator : Generator
{
    [SerializeField] private List<Segment> segments;
    private int _currentPosition = 0;

    protected override Segment GetNext()
    {
        Segment next = segments[_currentPosition];
        _currentPosition = (_currentPosition + 1) % segments.Count;
        return next;
    }
}