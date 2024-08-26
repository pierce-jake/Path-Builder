using System.Collections.Generic;
using UnityEngine;

public class Path
{

    [SerializeField, HideInInspector] private List<Vector2> points;
    
    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + (Vector2.left + Vector2.up) * 0.5f,
            center + (Vector2.right + Vector2.down) * 0.5f,
            center + Vector2.right
        };
    }

    public void AddSegment(Vector2 anchorPosition)
    {
        points.Add(2 * points[^1] - points[^2]);
        points.Add((points[^1] + anchorPosition) * 0.5f);
        points.Add(anchorPosition);
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        if (i % 3 == 0)
        {
            if (i - 1 >= 0)
                points[i - 1] += deltaMove;

            if (i + 1 < points.Count)
                points[i + 1] += deltaMove;

            return;
        }

        bool nextPointIsAnchor = (i + 1) % 3 == 0;
        int complementControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
        int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

        if (complementControlIndex < 0 || complementControlIndex >= points.Count)
            return;

        float dist = (points[anchorIndex] - points[complementControlIndex]).magnitude;
        Vector2 dir = (points[anchorIndex] - pos).normalized;
        points[complementControlIndex] = points[anchorIndex] + dir * dist;
    }

    public int PointCount
    {
        get
        {
            return points.Count;
        }
    }

    public int SegmentCount
    {
        get
        {
            return (points.Count - 4) / 3 + 1;
        }
    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        int segementIndex = i * 3;
        return new Vector2[] { points[segementIndex], points[segementIndex + 1], points[segementIndex + 2], points[segementIndex + 3] };
    }

}
